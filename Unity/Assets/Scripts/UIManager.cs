using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Sprite   m_StopTrainImage;
    public Sprite   m_StartTrainImage;
    public Image    m_BreakButtonImage;
    

    /// <summary>
    /// Toggles the train from being stopped and started.
    /// </summary>
    public void OnBreakPressed()
    {

        TrainJourneyManager trainJourneyManager = TrainJourneyManager.GetInstance();
        if( trainJourneyManager != null )
        {
            if( trainJourneyManager.HasTrainStopped() )
            {
                m_BreakButtonImage.sprite = m_StopTrainImage;

                trainJourneyManager.StartTrain();
            }
            else
            {
                m_BreakButtonImage.sprite = m_StartTrainImage;

                trainJourneyManager.StopTrain();
            }
        }
    }
}
