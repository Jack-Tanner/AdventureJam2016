using UnityEngine;
using System.Collections;

public class LeaveTrainDoor : Interactable
{
    public override void ThenDo()
    {
        Debug.Log( "Get off train." );

        TrainJourneyManager.GetInstance().GetOffTrain();
    }

    public override bool Highlightable()
    {
        return true;
    }
}
