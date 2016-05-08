using UnityEngine;
using System.Collections;

/// <summary>
/// What will happen when the player clicks on an item.
/// </summary>
public enum InteractableBehaviour
{
    None,
    WalkToLocation,
    WalkToClick
}

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// What will happen when the player clicks on this item.
    /// </summary>
    public InteractableBehaviour    m_OnClickedBehaviour;

    /// <summary>
    /// Where the player should walk to.
    /// </summary>
    public float                    m_LocationOffset;

    void OnDrawGizmos()
    {
        if( m_OnClickedBehaviour == InteractableBehaviour.WalkToLocation )
        {
            Vector3 position = transform.position;
            position.x += m_LocationOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawLine( transform.position, position );
            Gizmos.DrawSphere( position, 0.2f );
        }
    }

    /// <summary>
    /// Returns the place that the player should walk to.
    /// </summary>
    /// <returns></returns>
    public float GetGotoLocation()
    {
        return transform.position.x + m_LocationOffset;
    }
}
