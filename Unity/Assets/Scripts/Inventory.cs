using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The player's inventory.
/// </summary>
public class Inventory : MonoBehaviour
{
    /// <summary>
    /// How far apart the items appear in the UI.
    /// </summary>
    public float m_ItemSpacing = 100;

    /// <summary>
    /// Global instance for the inventory.
    /// </summary>
    static private Inventory m_Instance;

    /// <summary>
    /// All of the items that the player has collected.
    /// </summary>
    public List<Item> m_PlayersItems;

    /// <summary>
    /// The items as the are shown in the UI.
    /// </summary>
    public List<GameObject> m_UIItems;

    /// <summary>
    /// Template for items in the UI.
    /// </summary>
    public GameObject m_ExampleUIItem;

    /// <summary>
    /// Use this for initalization.
    /// </summary>
    void Start()
    {
        m_Instance = this;
        m_ExampleUIItem.SetActive( false );
        ArrangeUIItems();
    }

    /// <summary>
    /// Global access to the inventory.
    /// </summary>
    /// <returns>The Inventory.</returns>
    public static Inventory GetInstance()
    {
        return m_Instance;
    }

    /// <summary>
    /// Called when the player has obtained an item.
    /// </summary>
    /// <param name="item">What was picked up.</param>
    public void PickupItem( Item item )
    {
        m_PlayersItems.Add( item );

        ArrangeUIItems();

        Debug.Log( "Player picked up item " + item.m_ItemName );
    }

    /// <summary>
    /// Arranges the UI items.
    /// </summary>
    private void ArrangeUIItems()
    {
        Image[] UIItems = gameObject.GetComponentsInChildren<Image>( true );
        int index = 0;
        for( int i = 0; i < UIItems.Length; ++i )
        {
            if( UIItems[i].gameObject != gameObject )
            {
                Image currentItem = UIItems[i];
                if( index < m_PlayersItems.Count )
                {
                    currentItem.gameObject.SetActive( true );
                    if( currentItem != null )
                    {
                        SpriteRenderer sr = m_PlayersItems[index].GetComponent<SpriteRenderer>();
                        if( sr != null )
                        {
                            currentItem.sprite = sr.sprite;
                        }
                    }
                }
                else
                {
                    currentItem.gameObject.SetActive( false );
                }

                ++index;
            }
        }
    }
}
