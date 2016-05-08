using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    static CameraController m_Instance = null;

    Vector3 m_MoveToLocation;
    bool m_Move = false;
    float m_MoveStartTime;

    // Use this for initialization
    void Start()
    {
        m_Instance = this;

    }

    static public CameraController GetInstance() { return m_Instance; }

    // Update is called once per frame
    void Update()
    {
        if( m_Move == true )
        {
            float delta = Time.time - m_MoveStartTime;
            if( delta >= 1.0f )
            {
                m_Move = false;
                
                transform.position = m_MoveToLocation;
            }
            else
            {
                Vector3 position = Vector3.Lerp( transform.position, m_MoveToLocation, delta );
                Debug.Log( position.x );
                transform.position = position;
            }
        }
    }

    public void MoveToPosition( Vector2 location )
    {
        m_MoveToLocation = location;
        m_MoveToLocation.z = transform.position.z;
        m_Move = true;
        m_MoveStartTime = Time.time;
    }
}
