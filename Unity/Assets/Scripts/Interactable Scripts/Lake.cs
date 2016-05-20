using UnityEngine;
using System.Collections;

public class Lake : Interactable
{

    /// <summary>
    /// Called when the player has walked over to the item.
    /// </summary>
    public override void ThenDo()
    {
        // Just add it to the inventory for now.
        //Pickup();
        Character m_Character = GetComponent<Character>();
        m_Character.InteractWithNPC();
    }

    /// <summary>
    /// We want to show that the player can click on an item when it's in the world
    /// </summary>
    /// <returns></returns>
    public override bool Highlightable()
    {
        return true;
    }
}
