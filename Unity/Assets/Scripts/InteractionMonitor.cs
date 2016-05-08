using UnityEngine;
using System.Collections;
/// <summary>
/// Takes in all of the player's input.
/// </summary>
public class InteractionMonitor : MonoBehaviour
{
    public Player m_Player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonUp( 0 ) )
        {
            // Find where the player clicked in the world.
            Vector3 worldClickPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            Debug.Log( "Clicked at " + worldClickPosition.x );
            //m_Player.GotoLocation( worldClickPosition.x );
            RaycastHit2D hit = Physics2D.Raycast( worldClickPosition, Vector2.zero );

            if( hit.collider != null )
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                if( interactable != null )
                {
                    switch( interactable.m_OnClickedBehaviour )
                    {
                        case InteractableBehaviour.None:
                            // Intentionally empty.
                            break;

                        case InteractableBehaviour.WalkToLocation:
                            m_Player.GotoLocation( interactable.GetGotoLocation() );
                            break;

                        case InteractableBehaviour.WalkToClick:
                            m_Player.GotoLocation( worldClickPosition.x );
                            break;

                        default:
                            Debug.Log( "Unimplemented interactable behaviour" );
                            break;
                    }
                }
            }
        }
    }
}
