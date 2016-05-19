using UnityEngine;
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

    public Camera m_Camera;
    public Vector3 m_CameraSpawnOffsetOnTrain;
    public Vector3 m_CameraSpawnOffsetOffTrain;

    public GameObject m_TrainCarrage3;

    public static TrainJourneyManager m_instance;

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
            CrashSite,
            NONE
        }

        public SceneLocation location;
        public string scene;
        public int distance;
        public bool othersideOfTrack = false;
        public bool isDayTime = true;
        public bool isPuzzle = false;
    }

    public TrainJourney[] trainJourney;

    public Dictionary<int, TrainJourney> m_RouteAScenes = new Dictionary<int, TrainJourney>();
    public Dictionary<int, TrainJourney> m_RouteBScenes = new Dictionary<int, TrainJourney>();

    private AsyncOperation m_AsyncSceneLoad;
    private bool m_bIsOnTrain = true;

    public void Awake()
    {
        m_instance = this;
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_TrainCarrage3 = GameObject.Find("Train 3");
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
        TrainDistanceText.Set( m_fTrainPosition.ToString("0.0") + " km" );
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

    public bool IsOnTrain()
    {
        return m_bIsOnTrain;
    }

    public void GoToTrain()
    {
        GoToLocationOnJourney("TrainScene", true);
    }

    public void GetOffTrain()
    {
        if (HasTrainStopped() == false)
        {
            Debug.Log("TRAIN HAS NOT STOPPED");
            return;
        }

        int distanceTraveled = Mathf.RoundToInt(m_fTrainPosition);
        TrainJourney tJ = GetTrainStop(distanceTraveled);
        GoToLocationOnJourney(tJ.scene, false);
    }

    private void GoToLocationOnJourney(string scene, bool isTrainScene)
    {
        if (m_AsyncSceneLoad != null)
        {
            Debug.Log("TRYING TO LOAD SCENE WHILE BUSY LOADING SOMETHING ELSE!");
            return;
        }

        if (isTrainScene == false)
        {
            m_AsyncSceneLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }

        StartCoroutine(DoTransition(scene, isTrainScene));

    }


    public IEnumerator DoTransition(string scene, bool isTrainScene)
    {

        Fade.GetInstance().FadeOn();
        while ((m_AsyncSceneLoad != null && m_AsyncSceneLoad.isDone == false) || Fade.GetInstance().IsFading())
        {
            yield return null;
        }

        if (currentlyLoadedScene.isLoaded)
        {
            SceneManager.UnloadScene(currentlyLoadedScene.name);
        }

        Scene s = SceneManager.GetSceneByName(scene);
        if (isTrainScene == false)
        {
            m_bIsOnTrain = false;
            currentlyLoadedScene = s;
        }
        else
        {
            m_bIsOnTrain = true;
        }

        PositionPlayerInScene(s, isTrainScene);

        Fade.GetInstance().FadeOff();

        while (Fade.GetInstance().IsFading())
        {
            yield return null;
        }

        m_AsyncSceneLoad = null;
    }


    public void PositionPlayerInScene(Scene s, bool isTrainScene)
    {
        GameObject[] rootObjects = s.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; ++i)
        {
            if (rootObjects[i].name == "SpawnPoint")
            {

                if (isTrainScene)
                {
                    Player.GetInstance().transform.parent = m_TrainCarrage3.transform;
                    m_Camera.transform.position = rootObjects[i].transform.position + m_CameraSpawnOffsetOnTrain;
                }
                else
                { 
                    Player.GetInstance().transform.parent = Player.GetInstance().transform.root;
                    m_Camera.transform.position = rootObjects[i].transform.position + m_CameraSpawnOffsetOffTrain;
                }
                Player.GetInstance().transform.position = rootObjects[i].transform.position;

            }
        }
    }


    public TrainJourney.SceneLocation GetCurrentLocation()
    {
        int distanceTraveled = Mathf.RoundToInt(m_fTrainPosition);
        TrainJourney tJ = GetTrainStop(distanceTraveled);

        return tJ.location;
    }

    public bool IsDayTime()
    {
        int distanceTraveled = Mathf.RoundToInt(m_fTrainPosition);

        //if other side, try B, otherwise return A
        if (m_bUseOtherTrack)
        {
            TrainJourney tJB = null;
            m_RouteBScenes.TryGetValue(distanceTraveled, out tJB);
            if (tJB != null)
            {
                return tJB.isDayTime;
            }
        }

        TrainJourney tJ = null;
        m_RouteAScenes.TryGetValue(distanceTraveled, out tJ);

        return tJ.isDayTime;
    }

    public float GetTrainSpeedPercentage()
    {
        if (m_fTrainSpeed == 0.0f || m_fTrainMaxSpeed == 0.0f)
            return 0.0f;
        
        return m_fTrainSpeed / m_fTrainMaxSpeed;
    }
}
