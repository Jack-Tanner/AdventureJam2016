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
            m_Player.GotoLocation( worldClickPosition.x );
        }
    }
}
