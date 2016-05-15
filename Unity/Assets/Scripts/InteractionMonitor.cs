using UnityEngine;
using System.Collections;
/// <summary>
/// Takes in all of the player's input.
/// </summary>
public class InteractionMonitor : MonoBehaviour
{
    public Player m_Player;

    Interactable m_CurrentInteractable = null;
    GameObject m_CurrentHighlight = null;
    Vector3     m_HighlightScale;

    bool m_ThenDoCalled = false;

    // Update is called once per frame
    void Update()
    {
        if( ConversationOverlord.GetInstance().current_conversation == null )
        {
            // Find where the player clicked in the world.
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

            RaycastHit2D hit = Physics2D.Raycast( worldMousePosition, Vector2.zero );

            if( hit.collider != null )
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

                if( interactable != null )
                {
                    // Highlight mechanism.
                    if( m_CurrentHighlight != interactable.gameObject )
                    {
                        // We we were just highlighing an object then shrink it.
                        if( m_CurrentHighlight != null )
                        {
                            m_CurrentHighlight.transform.localScale = m_HighlightScale;
                        }

                        m_CurrentHighlight = interactable.gameObject;
                        m_HighlightScale = m_CurrentHighlight.transform.localScale;

                        if( interactable.Highlightable() )
                        {
                            m_CurrentHighlight.transform.localScale = m_HighlightScale * 1.2f;
                        }
                    }


                    // Clicking mechanism.
                    if( Input.GetMouseButtonUp( 0 ) && m_CurrentInteractable == null )
                    {
                        m_CurrentInteractable = interactable;
                        m_CurrentInteractable.OnClicked();

                        switch( interactable.m_OnClickedBehaviour )
                        {
                            case InteractableBehaviour.None:
                                // Intentionally empty.
                                break;

                            case InteractableBehaviour.WalkToLocation:
                                m_Player.WalkToLocation( interactable.GetWalkToLocation() );
                                break;

                            case InteractableBehaviour.WalkToClick:
                                m_Player.WalkToLocation( worldMousePosition.x );
                                break;

                            case InteractableBehaviour.WalkThenDo:
                                m_Player.WalkToLocation( interactable.GetWalkToLocation() );
                                break;

                            case InteractableBehaviour.WalkThenTalk:
                                m_Player.WalkToLocation( interactable.GetWalkToLocation() );
                                break;

                            default:
                                Debug.Log( "Unimplemented interactable behaviour" );
                                break;
                        }
                    }
                }
            }
            else
            {
                // Release the interacting object.
                if( m_CurrentHighlight != null )
                {
                    m_CurrentHighlight.transform.localScale = m_HighlightScale;
                    m_CurrentHighlight = null;
                }
            }

            // Wait for the player to move to the new object.
            if( m_CurrentInteractable != null && m_Player.GetState() == PlayerState.Ready )
            {
                switch( m_CurrentInteractable.m_OnClickedBehaviour )
                {
                    case InteractableBehaviour.WalkThenDo:
                    case InteractableBehaviour.WalkThenTalk:

                        // Check to see if this interactable has had it's ThenDo called.
                        if( m_ThenDoCalled == false )
                        {
                            // It's not so set the flag.
                            m_ThenDoCalled = true;

                            // Then tell it to do.
                            m_CurrentInteractable.ThenDo();
                        }
                        else
                        {
                            // The ThenDo has been called before, so we have completed this interaction.
                            m_CurrentInteractable = null;
                            m_ThenDoCalled = false;
                        }

                        break;

                    default:
                        m_CurrentInteractable = null;
                        break;
                }
            }
        }
    }
}
