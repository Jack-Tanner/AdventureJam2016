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
    public bool m_bTrainMoving = true;

    public Camera m_Camera;
    public Vector3 m_CameraSpawnOffsetOnTrain;
    public Vector3 m_CameraSpawnOffsetOffTrain;

    public GameObject m_TrainCarrage3;

    public static TrainJourneyManager m_instance;

    public delegate void OnTransition();
    public event OnTransition m_OnTransition;

    public GameObject OverlaySpawnPoint = null;
    public GameObject CanyonOverlay = null;
    public GameObject BridgeOverlay = null;
    public GameObject TunnelOverlay = null;
    public bool m_bStartedOutro = false;
    
    //Force text to speak, no matter the conditions.
    //also lol hacky
    public ConversationManager m_OutroConversation = null;

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
        public bool othersideOfTrack = true;
        public bool isDayTime = true;
        public bool isPuzzle = false;
    }

    public TrainJourney[] trainJourney;

    public Dictionary<int, TrainJourney> m_RouteAScenes = new Dictionary<int, TrainJourney>();
    public Dictionary<int, TrainJourney> m_RouteBScenes = new Dictionary<int, TrainJourney>();

    private AsyncOperation m_AsyncSceneLoad;
    private bool m_bIsOnTrain = true;
    
    public delegate void TrackChanged(bool otherTrack);
    public TrackChanged OnTrackChanged;

    private GameObject LastSpawnedOverlay = null;
    private TrainJourney.SceneLocation LastLocation = TrainJourney.SceneLocation.Station;


    public void Awake()
    {
        m_instance = this;
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_TrainCarrage3 = GameObject.Find("Train 3");
        m_bStartedOutro = false;
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


    public void ResetTrain()
    {
        //reset use other track? or is that a puzzle
        //m_bUseOtherTrack = false;
        GoToTrain(true);
    }
    
    /// <summary>
    /// Called to change the track/route.
    /// </summary>
    /// <param name="bOtherTrack">should we use the other track.</param>
    public void ChangeTrack( bool bOtherTrack )
    {
        m_bUseOtherTrack = bOtherTrack;
        OnTrackChanged( bOtherTrack );
    }

    public void UpdateOverlays()
    {
        int distanceTraveled = Mathf.FloorToInt(m_fTrainPosition);
        TrainJourney currentTJ = GetTrainStop(distanceTraveled);

        if (currentTJ == null || (currentTJ.location != TrainJourney.SceneLocation.Bridge &&
            currentTJ.location != TrainJourney.SceneLocation.Canyon &&
            currentTJ.location != TrainJourney.SceneLocation.Tunel) )
        {
            if ((m_fTrainPosition + 0.2f) >= ((float)distanceTraveled + 1.0f) )
            { 
                currentTJ = GetTrainStop(distanceTraveled + 1);

                if (currentTJ == null || (currentTJ.location != TrainJourney.SceneLocation.Bridge &&
                     currentTJ.location != TrainJourney.SceneLocation.Canyon &&
                     currentTJ.location != TrainJourney.SceneLocation.Tunel))
                    return;

            }
            else
                return;
        }

        if( currentTJ != null )
        {
            GameObject ToSpawn = null;
            bool bStartOver = false;

            switch( currentTJ.location )
            {
                case TrainJourney.SceneLocation.Bridge:
                    ToSpawn = BridgeOverlay;
                    break;
                case TrainJourney.SceneLocation.Canyon:
                    ToSpawn = CanyonOverlay;
                    break;
                case TrainJourney.SceneLocation.Tunel:
                    ToSpawn = TunnelOverlay;
                    break;
                default:
                    break;
            }

            if (currentTJ.location != LastLocation)
            {
                LastLocation = currentTJ.location;
                bStartOver = true;
            }

            if (ToSpawn != null && OverlaySpawnPoint != null )
            {
                if (LastSpawnedOverlay == null || bStartOver)
                {
                    bStartOver = false;
                    LastSpawnedOverlay = Instantiate(ToSpawn);
                    LastSpawnedOverlay.transform.position = OverlaySpawnPoint.transform.position;
                }
                else
                {
                    if( LastSpawnedOverlay.transform.position.x <= (OverlaySpawnPoint.transform.position.x - 3.4f) )
                    {
                        Vector3 LastPosition = LastSpawnedOverlay.transform.position;
                        SpriteRenderer renderer = LastSpawnedOverlay.GetComponent<SpriteRenderer>();
                        if (renderer)
                            LastPosition.x += renderer.bounds.size.x - 0.1f;

                        LastSpawnedOverlay = Instantiate(ToSpawn);
                        LastSpawnedOverlay.transform.position = LastPosition;
                    }
                }
            }
        }

      
    }

    /// <summary>
    /// returns if we're using the other track.
    /// </summary>
    public bool GetIsUsingOtherTrack()
    {
        return m_bUseOtherTrack;
    }

    public void Update()
    {
        if (QuestManager.GetInstance() && QuestManager.GetInstance().GameIsComplete())
        {
            if (m_bTrainMoving)
            {
                StopTrain();
            }
            else
            {
                if( m_bStartedOutro == false)
                {
                    m_bStartedOutro = true;
                    ConversationOverlord.GetInstance().current_conversation = m_OutroConversation;
                    ConversationOverlord.GetInstance().TickConversation();
                }
            }


        }


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

        UpdateOverlays();

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
        if( tJ != null )
            return tJ;

        return null;
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

    public void GoToTrain(bool isReset = false)
    {
        GoToLocationOnJourney("TrainScene", true, isReset);
    }

    public void GetOffTrain()
    {
        if (HasTrainStopped() == false)
        {
            Debug.Log("TRAIN HAS NOT STOPPED");
            return;
        }
        
        int distanceTraveled = Mathf.FloorToInt(m_fTrainPosition);
        TrainJourney tJ = GetTrainStop(distanceTraveled);
        GoToLocationOnJourney(tJ.scene, false);
    }

    private void GoToLocationOnJourney(string scene, bool isTrainScene, bool isReset = false)
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

        StartCoroutine(DoTransition(scene, isTrainScene, isReset));

    }


    public IEnumerator DoTransition(string scene, bool isTrainScene, bool isReset = false)
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

        if(m_OnTransition != null)
        {
            m_OnTransition();
        }
        PositionPlayerInScene(s, isTrainScene, isReset);


        if (isReset)
        {
            m_fTrainPosition = 0.0f;
            GetComponent<AudioSource>().Play();
            m_fTrainSpeed = 0.0f;
            m_bTrainMoving = true;
        }


        Fade.GetInstance().FadeOff();

        while (Fade.GetInstance().IsFading())
        {
            yield return null;
        }

        m_AsyncSceneLoad = null;
    }


    public void PositionPlayerInScene(Scene s, bool isTrainScene, bool reset)
    {
        GameObject[] rootObjects = s.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; ++i)
        {

            string placement = reset ? "StartingPos" : "SpawnPoint";
            if (rootObjects[i].name == placement)
            {

                if (isTrainScene)
                {
                    Player.GetInstance().transform.parent = m_TrainCarrage3.transform;
                    if (reset)
                    {
                        m_Camera.transform.position = new Vector3(0.0f,0.0f,-26.77f);
                    }
                    else
                    {
                        m_Camera.transform.position = new Vector3(-8.568f, 0.0f, -26.77f);
                    }
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
        int distanceTraveled = Mathf.FloorToInt(m_fTrainPosition);
        TrainJourney tJ = GetTrainStop(distanceTraveled);

        return tJ.location;
    }

    public bool IsDayTime()
    {
        int distanceTraveled = Mathf.FloorToInt(m_fTrainPosition);

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
