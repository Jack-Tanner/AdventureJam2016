using UnityEngine;
using System.Collections;

public class SpeechPosition : MonoBehaviour
{
    /// <summary>
    /// Where to put the speech object.
    /// </summary>
    public float m_speechOffsetY = 0.0f;
    /// <summary>
    /// Where to put the speech object.
    /// </summary>
    public float m_speechOffsetX = 0.0f;

    /// <summary>
    /// Returns the position to set the speech object at.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        Vector3 speechPosition = transform.position;

        // May need to compensate for the train bob.

        speechPosition.y += m_speechOffsetY;
        speechPosition.x += m_speechOffsetX;

        return speechPosition;
    }

    /// <summary>
    /// When the Editor draws in the Scene View.
    /// </summary>
    void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        position.y += m_speechOffsetY;
        position.x += m_speechOffsetX;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine( transform.position, position );
        Gizmos.DrawCube( position, Vector3.one * 0.1f );
    }
}
