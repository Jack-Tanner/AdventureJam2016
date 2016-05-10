using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor( typeof( Inventory ) )]
public class InventroyInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if( GUILayout.Button( "Space Items" ) )
        {
            Inventory myTarget = (Inventory)target;


            RectTransform[] images = myTarget.gameObject.GetComponentsInChildren<RectTransform>();
            List<RectTransform> UIImages = new List<RectTransform>();
            for( int i = 0; i < images.Length;  ++i )
            {
                if( images[i].gameObject != myTarget.gameObject )
                {
                    UIImages.Add( images[i] );
                }
            }

            UIImages.Sort( ( x, y ) => x.localPosition.x.CompareTo( y.localPosition.x ) );

            float start = UIImages[0].localPosition.x;
            float yPos = UIImages[0].localPosition.y;
            for( int i = 0; i < UIImages.Count; ++i )
            {
                Vector3 position = UIImages[i].localPosition;
                position.x = start + ( (float)i * myTarget.m_ItemSpacing ); 
                position.y = yPos;
                UIImages[i].localPosition = position;
                UIImages[i].name = "Item " + (i + 1);
            }
        }

        DrawDefaultInspector();
    }
}