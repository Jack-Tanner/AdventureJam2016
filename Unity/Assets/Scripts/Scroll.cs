using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

    public float MoveSpeed = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position = transform.position;
        position.x -= MoveSpeed * Time.deltaTime;
        transform.position = position;

    }
}
