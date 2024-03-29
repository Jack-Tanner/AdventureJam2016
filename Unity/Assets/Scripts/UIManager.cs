﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Sprite   m_StopTrainImage;
    public Sprite   m_StartTrainImage;
    public Image    m_BreakButtonImage;
    public Image    m_MapImage;

    public AudioSource MapOpenSound;
    public AudioSource MapCloseSound;
    public AudioSource RestartSound;

    private bool    m_MapShown = false;

    private static UIManager m_Instance = null;

    /// <summary>
    /// Called at the start.
    /// </summary>
    void Start()
    {
        m_Instance = this;

        TrainJourneyManager trainJourneyManager = TrainJourneyManager.GetInstance();
        if( trainJourneyManager != null )
        {
            if( trainJourneyManager.HasTrainStopped() )
            {
                if(m_BreakButtonImage != null)
                    m_BreakButtonImage.sprite = m_StopTrainImage;

                trainJourneyManager.StartTrain();
            }
            else
            {
                if (m_BreakButtonImage != null)
                    m_BreakButtonImage.sprite = m_StartTrainImage;

                trainJourneyManager.StopTrain();
            }
        }
    }


    /// <summary>
    /// Global Access
    /// </summary>
    /// <returns>The UI Manager</returns>
    public static UIManager GetInstance()
    {
        return m_Instance;
    }

    /// <summary>
    /// Returns false if the UI is taking all of the input.
    /// </summary>
    /// <returns>True if the player can click on things in the world.</returns>
    public bool InputAllowed()
    {
        if( m_MapShown == true )
        {
            return false;
        }


        // There are no UI elements being shown.
        return true;
    }

    /// <summary>
    /// Toggles the train from being stopped and started.
    /// </summary>
    public void OnBreakPressed()
    {
        TrainJourneyManager trainJourneyManager = TrainJourneyManager.GetInstance();
        if( trainJourneyManager != null )
        {
            if( trainJourneyManager.IsOnTrain() )
            {
                if( trainJourneyManager.HasTrainStopped() )
                {
                    if (m_BreakButtonImage != null)
                        m_BreakButtonImage.sprite = m_StopTrainImage;

                    trainJourneyManager.StartTrain();
                }
                else
                {
                    if (m_BreakButtonImage != null)
                        m_BreakButtonImage.sprite = m_StartTrainImage;

                    trainJourneyManager.StopTrain();
                }
            }
        }
    }

    /// <summary>
    /// Called when the player presses the map button.
    /// </summary>
    public void OnMapButtonPressed()
    {
        if( m_MapImage != null )
        {
            m_MapShown = !m_MapShown;
            
            if( m_MapShown )
            {
                MapOpenSound.Play();
            }
            else
            {
                MapCloseSound.Play();
            }

            //m_MapImage.color  = new Color( 1.0f, 1.0f, 1.0f, ( m_MapShown ? 1.0f : 0.0f ) );
            m_MapImage.gameObject.SetActive( m_MapShown );
        }
    }

    /// <summary>
    /// Called when the restart button is pressed.
    /// </summary>
    public void OnRestartPressed()
    {
        Debug.Log( "Restart Pressed." );
        if (ConversationOverlord.GetInstance().DoneTalking())
        {
            TrainJourneyManager.GetInstance().ResetTrain();
            RestartSound.Play();
        }
    }
}
