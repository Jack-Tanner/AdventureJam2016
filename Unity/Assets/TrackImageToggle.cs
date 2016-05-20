using UnityEngine;
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

    // Use this for initialization
    void Start()
    {

    }
}
