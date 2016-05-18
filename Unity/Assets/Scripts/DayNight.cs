using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

    public Sprite DaySprite = null;
    public Sprite NightSprite = null;

    private SpriteRenderer m_SpriteRenderer;

    private bool m_CurrentlyNight = false;

	// Use this for initialization
	void Start () {

        if (DaySprite == null)
        {
            Debug.Log(gameObject.name + " has no day sprite!");
        }

        if (NightSprite == null)
        {
            Debug.Log(gameObject.name + " has no night sprite!");
        }

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        
	}
	
	// Update is called once per frame
	void Update () {

        BackgroundManager backgroundManager = BackgroundManager.GetInstance();
        if (backgroundManager != null && m_SpriteRenderer != null)
        {
            if (backgroundManager.NightTime && m_CurrentlyNight == false )
            {
                m_CurrentlyNight = true;
                m_SpriteRenderer.sprite = NightSprite;
            }
            else if (backgroundManager.NightTime == false && m_CurrentlyNight == true)
            {
                m_CurrentlyNight = false;
                m_SpriteRenderer.sprite = DaySprite;
            }
        }
	
	}
}
