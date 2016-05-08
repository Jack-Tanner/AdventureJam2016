using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Is only populated when the player clicks on something it can interact with.
    /// </summary>
    public GameObject m_ClickedOnItem;

    /// <summary>
    /// What I am interacting with.
    /// </summary>
    public GameObject m_InteractingObject;

    /// <summary>
    /// How fast the player walks.
    /// </summary>
    public float m_WalkSpeed = 10.0f;

    /// <summary>
    /// The position the I was at in the last frame.
    /// </summary>
    public float m_LastPosition = 0.0f;

    /// <summary>
    /// Where the player is walking to.
    /// </summary>
    float m_TargetXPosition;

    /// <summary>
    /// If the player is currently moving.
    /// </summary>
    bool m_Moving;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( m_Moving )
        {
            Vector3 position = transform.position;

            if( Mathf.Abs( m_TargetXPosition - position.x ) < 0.1f )
            {
                position.x = m_TargetXPosition;
                m_Moving = false;
            }
            else
            {
                if( position.x < m_TargetXPosition )
                {
                    position.x += Time.deltaTime * m_WalkSpeed;
                }
                else
                {
                    position.x -= Time.deltaTime * m_WalkSpeed;
                }
            }

            transform.position = position;
        }
    }


    void FixedUpdate()
    {
        m_InteractingObject = null;
        m_LastPosition = transform.position.x;
    }

    void LateUpdate()
    {
    }


    /// <summary>
    /// Moves the player to a location.
    /// </summary>
    /// <param name="xPosition">The position to move to.</param>
    public void GotoLocation( float xPosition )
    {
        m_TargetXPosition = xPosition;

        m_Moving = true;
    }

    /// <summary>
    /// Moves the player to the last safe position and stops it
    /// from moving.
    /// </summary>
    public void SetLastPosition()
    {
        m_Moving = false;
        transform.position.Set( m_LastPosition, transform.position.y, transform.position.z );
    }

    /// <summary>
    /// Called when the player finishes moving.
    /// </summary>
    private void OnFinishedMoving()
    {
        if( m_ClickedOnItem != null )
        {
            Debug.Log( "Ready to interact with " + m_ClickedOnItem.name );
        }
    }
}
