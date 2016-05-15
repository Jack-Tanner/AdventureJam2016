using UnityEngine;
using System.Collections;

public class ChangeSpriteOnItem : MonoBehaviour
{
    /// <summary>
    /// The name of the item needed to change the sprite.
    /// </summary>
    public string m_ItemName;

    /// <summary>
    /// The new sprite to change to.
    /// </summary>
    public Sprite m_NewSprite;

    // Use this for initialization
    void Start()
    {
        Inventory.GetInstance().m_OnItemCollected += OnItemCollected;
    }

    private void OnItemCollected( string itemName )
    {
        if( m_ItemName == itemName )
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = m_NewSprite;
        }
    }
}
