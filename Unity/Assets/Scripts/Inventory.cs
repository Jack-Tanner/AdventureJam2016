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


    public delegate void OnItemCollected( string item );
    /// <summary>
    /// Called when an item is collected.
    /// </summary>
    /// <param name="item">Name of item collected.</param>
    public OnItemCollected m_OnItemCollected;

    /// <summary>
    /// Use this for initalization.
    /// </summary>
    void Awake()
    {
        m_Instance = this;
    }

    void Start()
    {
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
        item.gameObject.SetActive( false );
        Debug.Log( "Player picked up item " + item.m_ItemName );

        m_OnItemCollected( item.m_ItemName );

        ArrangeUIItems();
    }

    /// <summary>
    /// Pass in the name of the item you want to create, it will be
    /// added as an info item.
    /// </summary>
    /// <param name="itemName"></param>
    public void GiveInfoItem(string itemName)
    {
        GameObject newItemObject = new GameObject();
        Item newItem = newItemObject.AddComponent<Item>();
        newItem.m_ItemName = itemName;
        newItem.m_IsDataItem = true;
        // Hide everything under the quest manager.
        newItemObject.transform.parent = QuestManager.GetInstance().transform;
        PickupItem(newItem);
    }

    /// <summary>
    /// Returns true if the player has the item name passed in in their inventory.
    /// Includes InfoItems.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public bool HasItem( string itemName )
    {
        int count = m_PlayersItems.Count;
        for( int i = 0; i < count; ++i )
        {
            if( m_PlayersItems[i].m_ItemName == itemName )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Takes an item off the player.
    /// </summary>
    /// <param name="itemName"></param>
    public void RemoveItem( string itemName )
    {
        int count = m_PlayersItems.Count;

        for( int i = 0; i < count;  ++i )
        {
            if( m_PlayersItems[i].m_ItemName == itemName )
            {
                m_PlayersItems.RemoveAt( i );
                break;
            }
        }
    }

    /// <summary>
    /// Check if an item is a data item
    /// </summary>
    /// <param name="itemName">Name of item in players inventory.</param>
    /// <returns>true for data item.</returns>
    public bool IsDataItem( string itemName )
    {
        int count = m_PlayersItems.Count;

        for( int i = 0; i < count; ++i )
        {
            if( m_PlayersItems[i].m_ItemName == itemName )
            {
                return m_PlayersItems[i].m_IsDataItem;
            }
        }

        return false;
    }

    /// <summary>
    /// Arranges the UI items.
    /// </summary>
    private void ArrangeUIItems()
    {
        Image[] UIItems = gameObject.GetComponentsInChildren<Image>( true );
        int index = 0;
        int visibleItemCount = GetVisibleItemCount();
        int uiitemcount = UIItems.Length;
        for( int i = 0; i < uiitemcount; ++i )
        {
            Image currentItem = UIItems[i];
            if( currentItem != null && currentItem.gameObject != gameObject )
            {
                if( index < visibleItemCount )
                {
                    Item playersItem = GetVisibleItemByIndex( index );
                    if( playersItem != null )
                    {
                        currentItem.gameObject.SetActive( true );
                        if( currentItem != null )
                        {
                            SpriteRenderer sr = playersItem.GetComponent<SpriteRenderer>();
                            if( sr != null )
                            {
                                currentItem.sprite = sr.sprite;
                            }
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

    /// <summary>
    /// Returns a visible item by index as if there were no hidden items.
    /// </summary>
    /// <param name="index">index to get at</param>
    /// <returns>A valid item, or null if out of range index.</returns>
    private Item GetVisibleItemByIndex( int index )
    {
        int count = m_PlayersItems.Count;
        int visibleCount = 0;
        for( int i = 0; i < count; ++i )
        {
            if( m_PlayersItems[i].IsDataItem() == false )
            {
                if( index == visibleCount )
                {
                    return m_PlayersItems[i];
                }

                ++visibleCount;
            }
        }

        // unable to find this item.
        return null;
    }

    /// <summary>
    /// Returns the number of visible items.
    /// </summary>
    /// <returns>Visible item count.</returns>
    private int GetVisibleItemCount()
    {
        int count = m_PlayersItems.Count;
        int number = 0;
        for( int i = 0; i < count;  ++i )
        {
            if( m_PlayersItems[i].IsDataItem() == false )
            {
                ++number;
            }
        }

        return number;
    }
}
