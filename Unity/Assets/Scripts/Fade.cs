using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
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

    private static Fade m_Instance;

    void Start()
    {
        m_Instance = this;
        m_FadeImage = gameObject.GetComponent<Image>();
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
                    m_FadeImage.color = new Color( 0.0f, 0.0f, 0.0f, alphaValue );
                }
                else
                {
                    m_FadeImage.color = new Color( 0.0f, 0.0f, 0.0f, 1.0f );
                    m_state = FadeEnum.eOn;
                }

                break;

            case FadeEnum.eFadingOff:

                if( complete == false )
                {
                    float alphaValue = Mathf.Lerp( 0.0f, 1.0f, fadeTimeSeconds / m_FadeTime );

                    int blockyFade = (int)( alphaValue * 10.0f );
                    alphaValue = (float)blockyFade * 0.1f;
                    m_FadeImage.color = new Color( 0.0f, 0.0f, 0.0f, alphaValue );
                }
                else
                {
                    m_FadeImage.color = new Color( 0.0f, 0.0f, 0.0f, 0.0f );
                    m_state = FadeEnum.eOff;
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
}
