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
            // We should have a interactable script on us.
            Interactable myInteractable = gameObject.GetComponent<Interactable>();
            if( myInteractable != null )
            {
                player.SetPosition( myInteractable.GetGotoLocation() );
            }
        }
    }
}
