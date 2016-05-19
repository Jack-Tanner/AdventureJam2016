using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {

    public GameObject Sky = null;
    public GameObject Sun = null;
    public GameObject Horizon = null;
    public GameObject Rocks = null;
    public GameObject Floor = null;
    public GameObject Rails = null;
    public GameObject CloudsTop = null;
    public GameObject CloudsMid = null;
    public GameObject CloudsBottom = null;

    public float SkySpeed = 1.0f;
    public float HorizonSpeed = 1.0f;
    public float RocksSpeed = 1.0f;
    public float FloorSpeed = 1.0f;
    public float RailsSpeed = 1.0f;
    public float CloudsTopSpeed = 1.0f;
    public float CloudsMidSpeed = 1.0f;
    public float CloudsBottomSpeed = 1.0f;

    public float StationarySkyMultiplier = 0.5f;


    public bool NightTime = false;

    private static BackgroundManager m_Instance;

    void Awake()
    {
        m_Instance = this;
    }

    public static BackgroundManager GetInstance()
    {
        return m_Instance;
    }

    private void ScrollObject(ref GameObject gameObject, float fSpeed )
    {
        if( gameObject )
        {
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child == gameObject.transform)
                    continue;

                Vector3 newPos = child.position;
                newPos.x -= fSpeed * Time.deltaTime;
                child.position = newPos;
            }
        }
    }

    private void ScrollSkyAndClouds()
    {
        TrainJourneyManager journeyManager = TrainJourneyManager.GetInstance();
        float fSpeedOffset = journeyManager.GetTrainSpeedPercentage();
        fSpeedOffset = Mathf.Clamp(fSpeedOffset, StationarySkyMultiplier, 1.0f);
        ScrollObject(ref Sky, SkySpeed * fSpeedOffset);
        ScrollObject(ref CloudsTop, CloudsTopSpeed * fSpeedOffset);
        ScrollObject(ref CloudsMid, CloudsMidSpeed * fSpeedOffset);
        ScrollObject(ref CloudsBottom, CloudsBottomSpeed * fSpeedOffset);
        ScrollObject(ref Horizon, HorizonSpeed * fSpeedOffset);
    }

    private void ScrollBackground()
    {
        TrainJourneyManager journeyManager = TrainJourneyManager.GetInstance();
        float fSpeedOffset = journeyManager.GetTrainSpeedPercentage();
        ScrollObject(ref Rocks, RocksSpeed * fSpeedOffset);
        ScrollObject(ref Floor, FloorSpeed * fSpeedOffset);
        ScrollObject(ref Rails, RailsSpeed * fSpeedOffset);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        TrainJourneyManager journeyManager = TrainJourneyManager.GetInstance();
        if (journeyManager)
        {
            ScrollSkyAndClouds();

            if (!journeyManager.HasTrainStopped())
            {
                ScrollBackground();
            }
        }

    }
}
