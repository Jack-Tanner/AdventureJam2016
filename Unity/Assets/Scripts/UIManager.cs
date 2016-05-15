using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Sprite   m_StopTrainImage;
    public Sprite   m_StartTrainImage;
    public Image    m_BreakButtonImage;

    private bool    m_TrainRunning = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Toggles the train from being stopped and started.
    /// </summary>
    public void OnBreakPressed()
    {
        m_TrainRunning = !m_TrainRunning;
        
        if( m_TrainRunning )
        {
            m_BreakButtonImage.sprite = m_StopTrainImage;
        }
        else
        {
            m_BreakButtonImage.sprite = m_StartTrainImage;
        }

    }
}
