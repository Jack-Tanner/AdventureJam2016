﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class TrainJourneyManager : MonoBehaviour
{

    public Scene currentlyLoadedScene;
    public float m_fTrainPosition = 0.0f;
    public bool m_bUseOtherTrack = false;
    public float m_fTrainSpeed = 0.0f;
    public float m_fTrainMaxSpeed = 0.1f;
    public float m_fTrainAcceleration = 0.001f;
    public bool m_bTrainMoving = false;


    public Vector3 m_vCameraOffset;
    public static TrainJourneyManager m_instance;

    public TrainJourney[] trainJourney;

    public Dictionary<int, TrainJourney> m_RouteAScenes = new Dictionary<int, TrainJourney>();
    public Dictionary<int, TrainJourney> m_RouteBScenes = new Dictionary<int, TrainJourney>();

    private AsyncOperation m_AsyncSceneLoad;
    private GameObject m_Camera;

    [System.Serializable]
    public class TrainJourney
    {
        [System.Serializable]
        public enum SceneLocation
        {
            Station,
            Crossing,
            RockyDesert,
            SwitchBox,
            Canyon,
            Bridge,
            River,
            Tunel,
            SignalTower,
            CrashSite
        }

        public SceneLocation location;
        public string scene;
        public int distance;
        public bool othersideOfTrack = false;
        public bool isDayTime = true;
        public bool isPuzzle = false;
    }

    public void Awake()
    {
        m_instance = this;
        m_Camera = GameObject.Find("Main Camera");
    }

    public static TrainJourneyManager GetInstance()
    {
        return m_instance;
    }

    public void Start()
    {
        for(int i=0; i<trainJourney.Length; ++i)
        {
            TrainJourney tJ = trainJourney[i];
            if (tJ.othersideOfTrack == false)
            {
                m_RouteAScenes[tJ.distance] = tJ;
            }
            else
            {
                m_RouteBScenes[tJ.distance] = tJ;
            }
        }
    }

    public void ResetTrainPosition()
    {
        //reset use other track? or is that a puzzle
        //m_bUseOtherTrack = false;

        m_fTrainPosition = 0.0f;
    }

    public void Update()
    {
        if(m_bTrainMoving)
        {
            if(m_fTrainSpeed < m_fTrainMaxSpeed)
            {
                m_fTrainSpeed += m_fTrainAcceleration;
                
            }
            m_fTrainSpeed = Mathf.Min(m_fTrainMaxSpeed, m_fTrainSpeed);
            m_fTrainPosition += Time.deltaTime * m_fTrainSpeed;
        }
        else
        {
            if (m_fTrainSpeed > 0.0f)
            {
                m_fTrainSpeed -= m_fTrainAcceleration;
            }
            m_fTrainSpeed = Mathf.Max(0.0f, m_fTrainSpeed);
            m_fTrainPosition += Time.deltaTime * m_fTrainSpeed;
        }

        Train.SetBobAmount( m_fTrainSpeed );
    }

    public void GetOffTrain()
    {
        if(HasTrainStopped() == false)
        {
            Debug.Log("TRAIN HAS NOT STOPPED");
            return;
        }

        int distanceTraveled = Mathf.RoundToInt(m_fTrainPosition);
        TrainJourney tJ = GetTrainStop(distanceTraveled);
        GoToLocationOnJourney(tJ);
    }

    private TrainJourney GetTrainStop(int position)
    {
        //if other side, try B, otherwise return A
        if(m_bUseOtherTrack)
        {
            TrainJourney tJB = null;
            m_RouteBScenes.TryGetValue(position, out tJB);
            if(tJB != null)
            {
                return tJB;
            }
        }

        TrainJourney tJ = null;
        m_RouteAScenes.TryGetValue(position, out tJ);

        return tJ;
    }

    public void GoToTrain()
    {
        GoToLocationOnJourney(null);
    }

    //tJ == null for train scene
    private void GoToLocationOnJourney(TrainJourney tJ)
    {
        if (m_AsyncSceneLoad != null)
        {
            Debug.Log("TRYING TO LOAD SCENE WHILE BUSY LOADING SOMETHING ELSE!");
            return;
        }

        if (tJ == null)
        {
            StartCoroutine(DoTransition("TrainScene", true));
        }
        else
        {
            m_AsyncSceneLoad = SceneManager.LoadSceneAsync(tJ.scene, LoadSceneMode.Additive);
            StartCoroutine(DoTransition(tJ.scene));
        }
    }

    public IEnumerator DoTransition(string scene, bool trainScene = false)
    {
        //start fade
        Fade.GetInstance().FadeOn();
        while ((trainScene == false && m_AsyncSceneLoad.isDone == false) || Fade.GetInstance().IsFading())
        {
            yield return null;
        }

        if (currentlyLoadedScene != null && currentlyLoadedScene.isLoaded)
        {
            SceneManager.UnloadScene(currentlyLoadedScene.name);
        }

        Scene s = SceneManager.GetSceneByName(scene);
        PositionPlayerInScene(s);

        m_Camera.transform.position = Player.GetInstance().transform.position + m_vCameraOffset;

        if (trainScene == false)
        { 
            currentlyLoadedScene = s;
        }  

        Fade.GetInstance().FadeOff();
        while (Fade.GetInstance().IsFading())
        {
            yield return null;
        }

        m_AsyncSceneLoad = null;
    }


    public void PositionPlayerInScene(Scene s)
    {
        GameObject[] rootObjects = s.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; ++i)
        {
            if (rootObjects[i].name == "SpawnPoint")
            {
                Player.GetInstance().transform.position = rootObjects[i].transform.position;
            }
        }
    }

    public void StartTrain()
    {
        m_bTrainMoving = true;
    }

    public void StopTrain()
    {
        m_bTrainMoving = false;
    }

    public bool HasTrainStopped()
    {
        return m_fTrainSpeed <= float.Epsilon;
    }
}
