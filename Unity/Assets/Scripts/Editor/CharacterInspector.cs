using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Character myTarget = (Character)target;

        if(GUILayout.Button("TALK TO ME BOI"))
        {
           if( myTarget.SpeakToNPC() == false)
            {
                ;
            }
        }

        if (GUILayout.Button("RESOLVE"))
        {
            myTarget.GiveItem(myTarget.GetItemOfInterest());
        }

            DrawDefaultInspector();
    }
}