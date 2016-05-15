using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(TrainJourneyManager))]
public class TrainJourneyManagerInspector : Editor
{

    private TrainJourneyManager tM;

    public override void OnInspectorGUI()
    {
        tM = (TrainJourneyManager)target;
        if (tM.m_bTrainMoving)
        {
            if (GUILayout.Button("STOP TRAIN"))
            {
                tM.StopTrain();
            }
        }
        else
        {
            if (GUILayout.Button("START TRAIN"))
            {
                tM.StartTrain();
            }
            if (tM.HasTrainStopped())
            {
                if (GUILayout.Button("GET OFF TEH TRAINZ"))
                {
                    tM.GetOffTrain();
                }
                if (GUILayout.Button("GET ON TRAINZ"))
                {
                    tM.GoToTrain();
                }
            }
        }
        DrawDefaultInspector();
    }
}