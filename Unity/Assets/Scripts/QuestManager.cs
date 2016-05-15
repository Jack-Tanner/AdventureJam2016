using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Monitors what's added to the player's inventory and decides if a quest is compete.
/// </summary>
public class QuestManager : MonoBehaviour
{
    /// <summary>
    /// All of the quests that this player has. Completed quests will be removed from this list.
    /// </summary>
    public List<Quest> m_Quests = new List<Quest>();

    /// <summary>
    /// The static instance of QuestManager.
    /// </summary>
    private static QuestManager m_Instance = null;

    /// <summary>
    /// Global access to the QuestManager.
    /// </summary>
    /// <returns>QuestManager Instance</returns>
    public static QuestManager GetInstance()
    {
        return m_Instance;
    }

    /// <summary>
    /// Called when the scene is loaded.
    /// </summary>
    void Awake()
    {
        m_Instance = this;
        
    }

    void Start()
    {
        Inventory.GetInstance().m_OnItemCollected += OnItemAddedToInventory;
    }

    /// <summary>
    /// Called from Quest to say that it's completed.
    /// </summary>
    /// <param name="completeQuest">The complete quest.</param>
    public void CompleteQuest( Quest completeQuest )
    {
        m_Quests.Remove( completeQuest );
    }

    /// <summary>
    /// Called when something is added to the player's inventroy.
    /// </summary>
    /// <param name="itemName">What was added to the inventroy.</param>
    public void OnItemAddedToInventory( string itemName )
    {
        int count = m_Quests.Count;
        for( int i = 0; i < count;  ++i )
        {
            m_Quests[i].CheckItem( itemName );
        }
    }

    /// <summary>
    /// Returns true when all quests are complete.
    /// </summary>
    /// <returns>true, complete false not complete.</returns>
    public bool GameIsComplete()
    {
        return m_Quests.Count == 0;
    }
}
