using UnityEngine;
using System.Collections;

public class Switch : Interactable
{

    public SpriteRenderer m_SpriteRenderer;
    public Sprite m_RouteA;
    public Sprite m_RouteB;

    void Start()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (TrainJourneyManager.GetInstance().m_bUseOtherTrack == false)
        {
            m_SpriteRenderer.sprite = m_RouteA;
        }
        else
        {
            m_SpriteRenderer.sprite = m_RouteB;
        }
    }

    /// <summary>
    /// Called when the player has walked over to the switch.
    /// </summary>
    public override void ThenDo()
    {
        TrainJourneyManager.GetInstance().m_bUseOtherTrack = !TrainJourneyManager.GetInstance().m_bUseOtherTrack;
        UpdateSprite();
        GetComponent<AudioSource>().Play();
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
