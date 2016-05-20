using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrackImageToggle : MonoBehaviour
{
    /// <summary>
    /// The image shown when this track is on
    /// </summary>
    public Sprite m_OnImage;
    /// <summary>
    /// The image shown when this tack is off
    /// </summary>
    public Sprite m_OffImage;
    /// <summary>
    /// What route this belongs to.
    /// </summary>
    public bool m_IsTopRoute = false;

    /// <summary>
    /// The image on this game object.
    /// </summary>
    private Image m_image = null;

    // Use this for initialization
    void Start()
    {
        m_image = gameObject.GetComponent<Image>();
        if( TrainJourneyManager.GetInstance() != null )
        {
            TrainJourneyManager.GetInstance().OnTrackChanged += OnRouteChanged;
            OnRouteChanged( TrainJourneyManager.GetInstance().GetIsUsingOtherTrack() );
        }
        else
        {
            Debug.LogError( "NO TRAIN JOURNEY MANAGER." );
        }
    }

    void OnDestroy()
    {
        TrainJourneyManager.GetInstance().OnTrackChanged -= OnRouteChanged;
    }

    /// <summary>
    /// Called when tracks are changed.
    /// </summary>
    /// <param name="useOtherTrack"></param>
    void OnRouteChanged( bool useOtherTrack )
    {
        if( m_IsTopRoute )
        {
            // other track is the bottom one.
            if( useOtherTrack )
            {
                m_image.sprite = m_OffImage;
            }
            else
            {
                m_image.sprite = m_OnImage;
            }
        }
        else
        {
            // other track is the bottom one.
            if( useOtherTrack )
            {
                m_image.sprite = m_OnImage;
            }
            else
            {
                m_image.sprite = m_OffImage;
            }
        }
    }
}
