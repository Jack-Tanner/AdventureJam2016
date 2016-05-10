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

    public override void ThenDo()
    {
        m_Character.SpeakToNPC();
    }

    public override bool Highlightable()
    {
        return true;
    }
}
