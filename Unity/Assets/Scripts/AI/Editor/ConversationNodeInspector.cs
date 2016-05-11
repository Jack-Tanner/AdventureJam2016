using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ConversationNode))]
public class ConversationNodeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ConversationNode convNode = (ConversationNode)target;

        if (convNode.transform.childCount > 1)
        {
            EditorGUILayout.LabelField("Player Choice Node");
            convNode.name = "Player Choice Node";

            int count = convNode.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                if(GUILayout.Button(convNode.transform.GetChild(i).name))
                {
                    convNode.GetComponentInParent<ConversationManager>().SelectOption(i);
                }
            }
        }
        else
        {
            GameObject speaker = convNode.m_Speaker;
            convNode.name = ((speaker == null) ? "■■■" : speaker.name) + ": " + convNode.m_sSpeech;
            DrawDefaultInspector();
        }

        if(convNode.m_bConvCheckpoint)
        {
            convNode.name = "★ " + convNode.name;
        }
    }
}