using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A single quest item.
/// </summary>
public class Quest : MonoBehaviour
{
    /// <summary>
    /// The ghost that this quest saves.
    /// </summary>
    public GameObject m_Ghost;

    /// <summary>
    /// The item that completes this quest.
    /// </summary>
    public string m_CompleteQuestItem;

    /// <summary>
    /// The name of this quest.
    /// </summary>
    public string m_QuestName;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called to check if anything needs to happen when an item is added to this quest.
    /// </summary>
    /// <param name="itemName">name of item added</param>
    public void CheckItem( string itemName )
    {
        if( itemName == m_CompleteQuestItem )
        {
            QuestManager.GetInstance().CompleteQuest( this );
            StartCoroutine( Fade() );
        }
    }


    private IEnumerator Fade()
    {
        
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float fadeTime = 4.0f;
        float completeTime = Time.time + fadeTime;
        Vector3 startScale = transform.localScale;

        while( completeTime > Time.time )
        {
            float delta = Mathf.Lerp( 0.0f, 1.0f, ( completeTime - Time.time )/fadeTime );
            spriteRenderer.material.color = new Color( delta, delta, delta, delta );
            transform.localScale = startScale * ( delta );

            yield return new WaitForEndOfFrame();
        }

        // Remove this object now.
       // Destroy( gameObject );
        transform.position = new Vector3(9001.0f, 9001.0f, 9001.0f);
    }

}
