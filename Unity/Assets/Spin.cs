using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	private int spinSpeed;

	// Use this for initialization
	void Start () {

		spinSpeed = Random.Range(360,480);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0,0,spinSpeed *Time.deltaTime);

	}
}
