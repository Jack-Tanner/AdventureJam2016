using UnityEngine;
using System.Collections;

public class CinematicBars : MonoBehaviour {

    public GameObject m_Top;
    public GameObject m_Bottom;

    private Vector2 m_StartPositionTop;
    private Vector2 m_StartPositionBottom;

    private Vector2 m_EndPositionTop;
    private Vector2 m_EndPositionBottom;

    public float distance = 0.01f;
    public bool m_bShowBars = true;
    public float speed = 0.4f;

    private float t = 0;
    void Awake()
    {
        t = 0;
        m_StartPositionTop      = m_Top.transform.position;
        m_StartPositionBottom   = m_Bottom.transform.position;


        m_EndPositionTop =      m_StartPositionTop - Vector2.up*distance;
        m_EndPositionBottom =   m_StartPositionBottom + Vector2.up *distance;
    }

    public void StartBars()
    {
        m_bShowBars = true;
    }

    public void StopBars()
    {
        m_bShowBars = false;
    }


    void Update()
    {
        if (m_bShowBars && t < 1.0f)
        {
            m_Top.transform.position = Vector2.Lerp(m_StartPositionTop, m_EndPositionTop, t);
            m_Bottom.transform.position = Vector2.Lerp(m_StartPositionBottom, m_EndPositionBottom, t);
            t += Time.deltaTime*speed;
        }


        if ((m_bShowBars == false) && t > 0.0f)
        {
            m_Top.transform.position = Vector2.Lerp(m_StartPositionTop, m_EndPositionTop, t);
            m_Bottom.transform.position = Vector2.Lerp(m_StartPositionBottom, m_EndPositionBottom, t);
            t -= Time.deltaTime * speed;
        }
    }
}
