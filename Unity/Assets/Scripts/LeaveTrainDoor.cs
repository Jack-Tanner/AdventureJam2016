using UnityEngine;
using System.Collections;

public class LeaveTrainDoor : Interactable
{
    public override void ThenDo()
    {
        Debug.Log( "Get off train." );

        Fade.GetInstance().FadeOn();
    }

    public override bool Highlightable()
    {
        return true;
    }
}
