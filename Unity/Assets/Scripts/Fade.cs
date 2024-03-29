﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{

    public delegate void FadeComplete();
    public event FadeComplete OnFadeComplete;

    enum FadeEnum
    {
        eFadingOff,
        eFadingOn,
        eOn,
        eOff
    }

    
    public float m_FadeTime = 1.0f;
    private Image m_FadeImage;

    private float m_FadeEndTime;

    private FadeEnum m_state = FadeEnum.eOff;

    public float m_fFadeColour = 0.0f;


    private static Fade m_Instance;

    void Start()
    {
        m_Instance = this;
        m_FadeImage = gameObject.GetComponent<Image>();
        m_fFadeColour = 0.0f;
        OnFadeComplete = null;

        FadeOff();
    }

    /// <summary>
    /// Returns the fade instance.
    /// </summary>
    /// <returns></returns>
    public static Fade GetInstance()
    {
        return m_Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float fadeTimeSeconds = m_FadeEndTime - Time.time;
        bool complete = fadeTimeSeconds < 0.0f;

        switch( m_state )
        {

            case FadeEnum.eOn:
            case FadeEnum.eOff:
                // Do Nowt.
                break;

            case FadeEnum.eFadingOn:

                if( complete == false )
                {
                    float alphaValue = Mathf.Lerp( 1.0f, 0.0f, fadeTimeSeconds / m_FadeTime );

                    int blockyFade = (int)( alphaValue * 10.0f );
                    alphaValue = (float)blockyFade * 0.1f;
                    m_FadeImage.color = new Color(m_fFadeColour, m_fFadeColour, m_fFadeColour, alphaValue);
                }
                else
                {
                    m_FadeImage.color = new Color(m_fFadeColour, m_fFadeColour, m_fFadeColour, 1.0f);
                    m_state = FadeEnum.eOn;

                    if (OnFadeComplete != null)
                    {
                        OnFadeComplete();
                    }
                }

                break;

            case FadeEnum.eFadingOff:

                if( complete == false )
                {
                    float alphaValue = Mathf.Lerp( 0.0f, 1.0f, fadeTimeSeconds / m_FadeTime );

                    int blockyFade = (int)( alphaValue * 10.0f );
                    alphaValue = (float)blockyFade * 0.1f;
                    m_FadeImage.color = new Color(m_fFadeColour, m_fFadeColour, m_fFadeColour, alphaValue);
                }
                else
                {
                    m_FadeImage.color = new Color(m_fFadeColour, m_fFadeColour, m_fFadeColour, 0.0f);
                    m_state = FadeEnum.eOff;
                    m_fFadeColour = 0.0f;
                }

                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Returns true when the fade is in action.
    /// </summary>
    /// <returns></returns>
    public bool IsFading()
    {
        return FadeEnum.eFadingOn == m_state || FadeEnum.eFadingOff == m_state;
    }

    /// <summary>
    /// Returns true when the screen is occluded by the fade.
    /// </summary>
    /// <returns></returns>
    public bool IsOn()
    {
        return FadeEnum.eOn == m_state;
    }

    /// <summary>
    /// Starts the screen fading.
    /// </summary>
    public void FadeOn()
    {
        m_FadeEndTime = Time.time + m_FadeTime;
        m_state = FadeEnum.eFadingOn;
    }

    /// <summary>
    /// Removes the fade.
    /// </summary>
    public void FadeOff()
    {
        m_FadeEndTime = Time.time + m_FadeTime;
        m_state = FadeEnum.eFadingOff;
    }
    public void SetFadeWhite()
    {
        m_fFadeColour = 1.0f;
    }
    public void FadeToWhite()
    {
        m_fFadeColour = 1.0f;
        FadeOn();
    }
}
