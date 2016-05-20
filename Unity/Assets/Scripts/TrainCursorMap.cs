using UnityEngine;
using System.Collections;

public class TrainCursorMap : MonoBehaviour
{
    /// <summary>
    /// The canvas this is rendered on.
    /// </summary>
    public RectTransform m_CurrentCanvas = null;
    
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
        currentPosition -= 0.5f;
        Debug.Log( m_CurrentCanvas.rect.width );
        Vector3 newPos = m_transform.position;
        newPos.x = currentPosition * m_CurrentCanvas.rect.width;
        m_transform.position = newPos;
    }
}
