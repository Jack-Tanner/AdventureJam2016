using UnityEngine;
using System.Collections;

public class Item : Interactable
{
    /// <summary>
    /// The name of the item, things will search for this item by name.
    /// </summary>
    public string m_ItemName;

    public AudioSource PickupSound;

    /// <summary>
    /// If true then this item will be shown in the inventory. Set to false for information items.
    /// </summary>
    public bool m_IsDataItem = false;

    /// <summary>
    /// Accessor to item visiblility.
    /// </summary>
    /// <returns></returns>
    public bool IsDataItem() { return m_IsDataItem; }

    /// <summary>
    /// Called when the player clicks on the item and pics it up.
    /// </summary>
    public void Pickup()
    {
        // Hide the world object.
        gameObject.SetActive( false );
        Inventory.GetInstance().PickupItem( this );

        PickupSound.Play();
    }

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
