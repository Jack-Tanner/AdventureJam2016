using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour {
    private float fDestroyXPosition = -10.0f;
    private float fSpriteWidth = 0.0f;

    public GameObject RespawnObject = null;
    private GameObject CloneObject = null;
    
	// Use this for initialization
	void Start ()
    {
        if (RespawnObject) 
        {
            CloneObject = Instantiate(RespawnObject);
            if (CloneObject) 
            {
                CloneObject.transform.SetParent(transform);
                SpriteRenderer originalRenderer = RespawnObject.GetComponent<SpriteRenderer>();
                if (originalRenderer)
                {
                    fSpriteWidth = originalRenderer.bounds.size.x;

                    Vector3 pos = RespawnObject.transform.position;
                    pos.x += fSpriteWidth;
                    CloneObject.transform.position = pos;
                    fDestroyXPosition = RespawnObject.transform.position.x - fSpriteWidth;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        if( RespawnObject && CloneObject )
        {
            if (RespawnObject.transform.position.x <= fDestroyXPosition)
            {
                Vector3 pos = RespawnObject.transform.position;
                pos.x = CloneObject.transform.position.x + fSpriteWidth;
                RespawnObject.transform.position = pos;
            }

            if (CloneObject.transform.position.x <= fDestroyXPosition)
            {
                Vector3 pos = CloneObject.transform.position;
                pos.x = RespawnObject.transform.position.x + fSpriteWidth;
                CloneObject.transform.position = pos;
            }
        }
	
	}
}
