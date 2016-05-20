using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrainCursorMap : MonoBehaviour
{
    /// <summary>
    /// The canvas this is rendered on.
    /// </summary>
    public RectTransform m_Start;
    public RectTransform m_End;

    /// <summary>
    /// UI transform.
    /// </summary>
    private RectTransform m_transform;

    void Start()
    {
        m_transform = gameObject.GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        float currentPosition = TrainJourneyManager.GetInstance().m_fTrainPosition / 30.0f;
        float pos  = Mathf.Lerp( m_Start.position.x, m_End.position.x, currentPosition );
        Vector3 newPos = m_transform.position;
        newPos.x = pos;
        m_transform.position = newPos;
    }
}
