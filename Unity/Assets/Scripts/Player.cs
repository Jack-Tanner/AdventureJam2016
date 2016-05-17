﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Shows what the player is currently doing.
/// </summary>
public enum PlayerState
{
    Ready,
    Walking,
    Interacting
}

public class Player : SpeechPosition
{
    /// <summary>
    /// Is only populated when the player clicks on something it can interact with.
    /// </summary>
    public GameObject m_ClickedOnItem;

    /// <summary>
    /// This is my instance.
    /// </summary>
    static Player m_Instance;

    /// <summary>
    /// Returns my instance.
    /// </summary>
    /// <returns>Me</returns>
    static public Player GetInstance()
    {
        return m_Instance;
    }

    /// <summary>
    /// How fast the player walks.
    /// </summary>
    public float m_WalkSpeed = 10.0f;

    /// <summary>
    /// Where the player is walking to.
    /// </summary>
    float m_TargetXPosition;

    /// <summary>
    /// My Current state.
    /// </summary>
    PlayerState m_State;

    /// <summary>
    /// Returns my current state.
    /// </summary>
    /// <returns></returns>
    public PlayerState GetState() { return m_State; }

    /// <summary>
    /// Sets my current state.
    /// </summary>
    /// <param name="state">My new state</param>
    public void SetState( PlayerState state ) { m_State = state; }


    void Start()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_State == PlayerState.Walking )
        {
            Vector3 position = transform.position;

            if( Mathf.Abs( m_TargetXPosition - position.x ) < 0.1f )
            {
                position.x = m_TargetXPosition;
                m_State = PlayerState.Ready;
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

    }


    /// <summary>
    /// Moves the player to a location.
    /// </summary>
    /// <param name="xPosition">The position to move to.</param>
    public void WalkToLocation( float xPosition )
    {
        m_TargetXPosition = xPosition;

        m_State = PlayerState.Walking;
    }

    /// <summary>
    /// Moves the player to the last safe position and stops it
    /// from moving.
    /// </summary>
    public void SetPosition( float position )
    {
        m_State = PlayerState.Ready;
        transform.position.Set( position, transform.position.y, transform.position.z );
    }
}
