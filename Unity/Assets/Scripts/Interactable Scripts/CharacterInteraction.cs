using UnityEngine;
using System.Collections;

public class CharacterInteraction : Interactable
{
    private Character m_Character;
    
    // Use this for initialization
    void Start()
    {
        m_Character = gameObject.GetComponent<Character>();
        if( m_Character == null )
        {
            Debug.LogError( "CharacterInteraction::Character component not found for object " + name );
        }
    }
    /// <summary>
    /// Called when the player arrives at this player to talk to them.
    /// </summary>
    public override void ThenDo()
    {
        m_Character.InteractWithNPC();
    }

    /// <summary>
    /// Show the character's name.
    /// </summary>
    /// <returns>return true for name plate.</returns>
    public override bool Highlightable()
    {
        return true;
    }
}
