using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("COMPLETE ALL QUESTS"))
        {
            QuestManager myTarget = (QuestManager)target;


            myTarget.m_Quests.RemoveRange(0, myTarget.m_Quests.Count);
        }
        DrawDefaultInspector();
    }
}