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

        Vector3 position = gameObject.transform.position;
        position.x -= MoveSpeed * Time.deltaTime;
        gameObject.transform.position = position;

        if (position.x <= DestroyXPos)
        {
            Destroy(gameObject);
        }

	}
}
