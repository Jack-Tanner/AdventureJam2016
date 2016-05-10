using UnityEngine;
using System.Collections;

public class Item : Interactable
{
    /// <summary>
    /// The name of the item, things will search for this item by name.
    /// </summary>
    public string m_ItemName;

    /// <summary>
    /// Called when the player clicks on the item and pics it up.
    /// </summary>
    public void Pickup()
    {
        // Hide the world object.
        gameObject.SetActive( false );
        Inventory.GetInstance().PickupItem( this );
    }

    /// <summary>
    /// Called when the player has walked over to the item.
    /// </summary>
    public override void ThenDo()
    {
        // Just add it to the inventory for now.
        Pickup();
    }
}
