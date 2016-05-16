using UnityEngine;
using System.Collections;

/// <summary>
/// What will happen when the player clicks on an item.
/// </summary>
public enum InteractableBehaviour
{
    None,
    WalkToLocation,
    WalkToClick,
    WalkThenDo,
    WalkThenTalk,
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

    /// <summary>
    /// How far away the name should show.
    /// </summary>
    public float                    m_NamePlateOffset;

    /// <summary>
    /// The string shown in the name plate.
    /// </summary>
    public string                   m_HightlightName;

    /// <summary>
    /// When the Editor draws in the Scene View.
    /// </summary>
    void OnDrawGizmos()
    {
        switch( m_OnClickedBehaviour )
        {
            case InteractableBehaviour.WalkThenTalk:
            case InteractableBehaviour.WalkThenDo:
            case InteractableBehaviour.WalkToLocation:
                {
                    Vector3 position = transform.position;
                    position.x += m_LocationOffset;
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine( transform.position, position );
                    Gizmos.DrawSphere( position, 0.03f );
                }
                break;
        }

        if( Highlightable() )
        {
            Vector3 position = transform.position;
            position.y += m_NamePlateOffset;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine( transform.position, position );
            Gizmos.DrawSphere( position, 0.03f );
        }
    }

    /// <summary>
    /// Returns the place that the player should walk to.
    /// </summary>
    /// <returns></returns>
    public float GetWalkToLocation()
    {
        return transform.position.x + m_LocationOffset;
    }

    public virtual void OnClicked()
    {
        Debug.Log( gameObject.name + " clicked." );
    }

    public virtual void ThenDo()
    {
        Debug.Log( "Nothing to do." );
    }

    public virtual bool Highlightable()
    {
        return false;
    }

    public virtual Vector3 GetNamePlatePosition()
    {
        Vector3 position = transform.position;
        position.y += m_NamePlateOffset;
        return position;
    }

    public string GetHighlightName()
    {
        return m_HightlightName;
    }
}
