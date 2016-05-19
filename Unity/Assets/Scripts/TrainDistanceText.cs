using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Just for setting the distance to the end.
/// </summary>
public class TrainDistanceText : MonoBehaviour
{
    private static Text m_Text;


    // Use this for initialization
    void Start()
    {
        m_Text = gameObject.GetComponent<Text>();
    }

    /// <summary>
    /// Sets the text in the train distance.
    /// </summary>
    /// <param name="text">Distance to show.</param>
    public static void Set( string text )
    {
        if( m_Text != null )
        {
            m_Text.text = text;
        }
    }
}
