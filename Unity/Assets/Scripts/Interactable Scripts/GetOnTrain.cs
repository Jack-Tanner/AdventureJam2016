using UnityEngine;
using System.Collections;

public class GetOnTrain : Interactable
{
    public AudioSource DoorSound;
    public override void ThenDo()
    {
        TrainJourneyManager.GetInstance().GoToTrain();
        DoorSound.Play();
    }

    public override bool Highlightable()
    {
        return true;
    }
}
