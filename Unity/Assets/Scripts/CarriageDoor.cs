using UnityEngine;
using System.Collections;

public class CarriageDoor : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D( Collider2D other )
    {
        Player player = other.gameObject.GetComponent<Player>();
        if( player != null )
        {
            player.SetLastPosition();
        }
    }
}
