using UnityEngine;
using System.Collections;

/// <summary>
/// Sets the animation on this character when an item is collected.
/// </summary>
public class ChangeAnimOnItem : MonoBehaviour
{
    /// <summary>
    /// The name of the item this is listening for.
    /// </summary>
    public string m_NameOfItem;

    /// <summary>
    /// The name of the bool used to change the animation.
    /// </summary>
    public string m_NameOfTransition;

    /// <summary>
    /// The Animator for this item.
    /// </summary>
    private Animator m_animator = null;

    /// <summary>
    /// Called when this object is loaded.
    /// </summary>
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        if( m_animator == null )
        {
            Debug.LogError( gameObject.name + " does not have an animattor!" );
        }

        Inventory.GetInstance().m_OnItemCollected += OnItemCollected;
    }
    
    /// <summary>
    /// Called when an item is added to the player's inventory.
    /// </summary>
    /// <param name="name">Name of item added.</param>
    private void OnItemCollected( string name )
    {
        if( name == m_NameOfItem && m_animator != null )
        {
            m_animator.SetBool( m_NameOfTransition, true );
        }
    }

    void OnDestroy()
    {
        Inventory.GetInstance().m_OnItemCollected -= OnItemCollected;
    }
}
