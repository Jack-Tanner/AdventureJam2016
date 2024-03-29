﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Takes in all of the player's input.
/// </summary>
public class InteractionMonitor : MonoBehaviour
{
    public Player m_Player;

    Interactable    m_CurrentInteractable = null;
    GameObject      m_CurrentHighlight = null;
    //Vector3         m_HighlightScale;
    public RectTransform   m_HoverOver;
    public Text            m_NamePlate;

    bool m_ThenDoCalled = false;

    private static InteractionMonitor m_Instance;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;

    /// <summary>
    /// Global Accessor.
    /// </summary>
    /// <returns></returns>
    public static InteractionMonitor GetInstance()
    {
        return m_Instance;
    }

    void Awake()
    {
        m_Instance = this;
        //Cursor.SetCursor( cursorTexture, Vector2.zero, cursorMode );
    }

    void OnDestroy()
    {
        //Cursor.SetCursor( null, Vector2.zero, cursorMode );
    }

    /// <summary>
    /// Called on every logic frame.
    /// </summary>
    void Update()
    {
        if( TrainJourneyManager.GetInstance().m_fTrainPosition > 30.0f )
        {
            // Don't continue to interact.
            Player.GetInstance().StopWalk();
            return;
        }

        // Check that the player is able to click on things in the world.
        if (UIManager.GetInstance().InputAllowed() && ConversationOverlord.GetInstance().DoneTalking () )
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
                            //m_CurrentHighlight.transform.localScale = m_HighlightScale;
                            m_HoverOver.gameObject.SetActive( false );
                        }

                        m_CurrentHighlight = interactable.gameObject;
                        //m_HighlightScale = m_CurrentHighlight.transform.localScale;
                        
                        if( interactable.Highlightable() )
                        {
                            m_HoverOver.gameObject.SetActive( true );
                            m_NamePlate.text = interactable.GetHighlightName();
                            //m_CurrentHighlight.transform.localScale = m_HighlightScale * 1.2f;
                            m_HoverOver.transform.position = interactable.GetNamePlatePosition();
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
                    //m_CurrentHighlight.transform.localScale = m_HighlightScale;
                    m_CurrentHighlight = null;
                    m_HoverOver.gameObject.SetActive( false );
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

                            // Turn the player to face what the're doing.
                            m_Player.SetFacingPosition( m_Player.transform.position.x > m_CurrentInteractable.transform.position.x );
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


    /// <summary>
    /// Hides the name plate that's shown when hovering over things.
    /// </summary>
    public void TurnOffNamePlate()
    {
        if( m_HoverOver != null )
        {
            m_HoverOver.gameObject.SetActive( false );
        }
    }
}
