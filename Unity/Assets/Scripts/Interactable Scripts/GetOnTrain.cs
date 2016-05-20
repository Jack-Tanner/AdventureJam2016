using UnityEngine;
using System.Collections;

public class GetOnTrain : Interactable
{

    public override void ThenDo()
    {
        TrainJourneyManager.GetInstance().GoToTrain();
    }

    public override bool Highlightable()
    {
        return true;
    }
}
