using UnityEngine;
using System.Collections;

public class SceneOverlay : MonoBehaviour {

    public float MoveSpeed = 1.0f;
    public float DestroyXPos = -20.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float fSpeedOffset = 1.0f;
        TrainJourneyManager journeyManager = TrainJourneyManager.GetInstance();
        if (journeyManager)
        {
            fSpeedOffset = journeyManager.GetTrainSpeedPercentage();
        }

        Vector3 position = gameObject.transform.position;
        position.x -= ( MoveSpeed * fSpeedOffset ) * Time.deltaTime;
        gameObject.transform.position = position;

        if (position.x <= DestroyXPos)
        {
            Destroy(gameObject);
        }

	}
}
