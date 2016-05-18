using UnityEngine;
using System.Collections.Generic;

public class Respawner : MonoBehaviour {
    private float fDestroyXPosition = -10.0f;
    private float fSpriteWidth = 0.0f;

    public GameObject RespawnObject = null;
    private List<GameObject> ClonedObjects = new List<GameObject>();

    private GameObject lastOne = null;
    
	// Use this for initialization
	void Start ()
    {
        if (RespawnObject) 
        {
            SpriteRenderer originalRenderer = RespawnObject.GetComponent<SpriteRenderer>();
            if (originalRenderer)
            { 
                fSpriteWidth = originalRenderer.bounds.size.x;
            }

            fDestroyXPosition = RespawnObject.transform.position.x - (fSpriteWidth * 4);


            // Spawn 3 left
            for (int i = 0; i < 3; i++)
            {
                GameObject newObject = Instantiate(RespawnObject);
                if (newObject)
                {
                    newObject.transform.SetParent(transform);
                    Vector3 pos = RespawnObject.transform.position;
                    pos.x -= fSpriteWidth * (i + 1);
                    newObject.transform.position = pos;
                    ClonedObjects.Add(newObject);
                }
            }

            // Spawn 1 right
            GameObject CloneObject = Instantiate(RespawnObject);
            if (CloneObject) 
            {
                CloneObject.transform.SetParent(transform);
                Vector3 pos = RespawnObject.transform.position;
                pos.x += fSpriteWidth;
                CloneObject.transform.position = pos;
                ClonedObjects.Add(CloneObject);
                lastOne = CloneObject;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (RespawnObject)
        {
            if (RespawnObject.transform.position.x <= fDestroyXPosition)
            {
                Vector3 pos = RespawnObject.transform.position;
                pos.x = lastOne.transform.position.x + fSpriteWidth;
                RespawnObject.transform.position = pos;
                lastOne = RespawnObject;
            }
        }

        foreach(GameObject obj in ClonedObjects )
        {
            if (obj.transform.position.x <= fDestroyXPosition)
            {
                Vector3 pos = obj.transform.position;
                pos.x = lastOne.transform.position.x + fSpriteWidth;
                obj.transform.position = pos;
                lastOne = obj;
            }
        }	
	}
}
