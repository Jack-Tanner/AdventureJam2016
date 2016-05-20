using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour
{
    /// <summary>
    /// This object's transform.
    /// </summary>
    private RectTransform m_transform;

    // Use this for initialization
    void Start()
    {
        m_transform = gameObject.GetComponent<RectTransform>();

        // turn off all children.
        SetChildrenActive( false );
    }


    /// <summary>
    /// Turns on / off all children.
    /// </summary>
    /// <param name="bActive"></param>
    private void SetChildrenActive( bool bActive )
    {
        RectTransform[] kids = gameObject.GetComponentsInChildren<RectTransform>( true );
        if( kids != null )
        {
            int count = kids.Length;
            for( int i = 0; i < count; ++i )
            {
                if( kids[i].gameObject != gameObject )
                {
                    kids[i].gameObject.SetActive( bActive );
                }
            }
        }
    }

    /// <summary>
    /// Called when the mouse hovers over this item.
    /// </summary>
    public void OnMouseIn()
    {
        if( m_transform != null )
        {
            m_transform.localScale = Vector3.one * 1.2f;

            // turn on all children.
            SetChildrenActive( true );
        }
    }

    /// <summary>
    /// Called when the mouse leaves this item.
    /// </summary>
    public void OnMouseOut()
    {
        if( m_transform != null )
        {
            m_transform.localScale = Vector3.one;
        }

        SetChildrenActive( false );
    }
}
