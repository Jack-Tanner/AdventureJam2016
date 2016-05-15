using UnityEngine;
using System.Collections;

public class LeaveTrainDoor : Interactable
{
    public override void ThenDo()
    {
        Debug.Log( "Get off train." );
        TrainJourneyManager trainJourneyManager = TrainJourneyManager.GetInstance();
        if( trainJourneyManager != null && trainJourneyManager.HasTrainStopped() == true )
        {
            trainJourneyManager.GetOffTrain();
        }
    }

    public override bool Highlightable()
    {
        return true;
    }
}
