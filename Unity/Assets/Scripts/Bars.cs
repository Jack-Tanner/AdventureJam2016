using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Bars : MonoBehaviour
{
    public bool m_TweenIn = false;
    public bool m_FadeOut = false;
    public float m_EndTime;
    private float m_TimeIn = 0.2f;
    public float m_FadeEndTime;
    private float m_FadeTimeOut = 1.0f;
    public RectTransform m_transform;
    static List<Bars> AllBars = new List<Bars>();
    public Image m_image;
    void Start()
    {
        m_transform = gameObject.GetComponent<RectTransform>();
        m_image = gameObject.GetComponent<Image>();
        AllBars.Add( this );
    }

    // Update is called once per frame
    void Update()
    {
        if( m_TweenIn )
        {
            float delta = Mathf.Lerp( 0.0f, 1.0f, (Time.time - m_EndTime)/m_TimeIn );
            Vector3 scale = Vector3.one;
            scale.y = delta;
            m_transform.localScale = scale;

            if( delta >= 1.0f )
            {
                m_TweenIn = false;
            }
        }

        if( m_FadeOut )
        {
            float delta = Mathf.Lerp( 1.0f, 0.0f, (Time.time - m_FadeEndTime)/m_FadeTimeOut );

            Color col = m_image.color;
            col.a = delta;
            m_image.color = col;
            if( delta <= 0.0f )
            {
                m_FadeOut = false;
                Vector2 scale = m_transform.localScale;
                scale.y = 0.0f;
                m_transform.localScale = scale;
                col.a = 1.0f;
                m_image.color = col;
            }
        }
    }

    public void StartIn()
    {
        m_EndTime = Time.time + m_TimeIn;
        m_TweenIn = true;
    }

    public void FadeOut()
    {
        m_FadeOut = true;
        
        m_FadeEndTime = Time.time + m_FadeTimeOut;
    }

    public static void StartAll()
    {
        int count = AllBars.Count;
        for( int i = 0; i < count;  ++i )
        {
            AllBars[i].StartIn();
        }
    }

    public static void FadeAll()
    {
        int count = AllBars.Count;
        for( int i = 0; i < count; ++i )
        {
            AllBars[i].FadeOut();
        }
    }
}
