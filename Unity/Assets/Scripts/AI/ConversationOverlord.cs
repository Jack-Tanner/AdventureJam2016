using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConversationOverlord : MonoBehaviour
{

    public static ConversationOverlord instance;
    public ConversationManager current_conversation;
    public RectTransform m_textPosition;
    
    public GameObject m_optionsGo;
    public Vector3 m_SpeechBoxOffset;
    public Camera m_Camera;

    /// <summary>
    /// Must be a child of m_textPosition.
    /// </summary>
    private Text m_textBox;

    public static ConversationOverlord GetInstance()
    {
        return instance;
    }

    public void Awake()
    {
        instance = this;
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Start()
    {
        if( m_textPosition != null )
        {
            m_textBox = m_textPosition.gameObject.GetComponentInChildren<Text>( true );
            if( m_textBox == null )
            {
                Debug.LogError( "Unable to find the text for conversations." );
            }
        }
        else
        {
            Debug.LogError( "No text attached to the Conversation Overloard." );
        }
    }

    /// <summary>
    /// Called to show someone is talking. Sets the text and position of the text box.
    /// </summary>
    /// <param name="text">What text to show.</param>
    /// <param name="textPosition">Where to show it in world space.</param>
    public void SendText(string text, Vector3 textPosition)
    {
        m_textBox.text = text;
        m_textPosition.position = textPosition;

        // Make sure we turn off the name plate.
        InteractionMonitor.GetInstance().TurnOffNamePlate();
    }

    public void ShowOptions(string[] options)
    {
        m_optionsGo.SetActive(true);
        int count = m_optionsGo.transform.childCount;
        for(int i=0; i<count; ++i)
        {
            Transform t = m_optionsGo.transform.GetChild(i);
            if(i < options.Length)
            {
                t.gameObject.SetActive(true);
                t.GetComponentInChildren<Text>().text = options[i];
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    public void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            TickConversation();
        }
    }

    public void SelectOption(int index)
    {
        current_conversation.SelectOption(index);
        m_optionsGo.SetActive(false);
    }


    private bool m_bDoneTalking = false;

    public void TickConversation()
    {
        if (m_bDoneTalking)
        {
            m_textPosition.gameObject.SetActive(false);
            m_bDoneTalking = false;
        }

        if (current_conversation == null)
        {

        }
        else
        {
            bool autoTick = current_conversation.m_CurrentConversationNode.m_bIsSilent;
            m_bDoneTalking = current_conversation.DoSpeech();
            m_textPosition.gameObject.SetActive(true);
            //Vector3 speakerPosition = current_conversation.m_CurrentConversationNode.m_Speaker.transform.position;
            //speakerPosition += m_SpeechBoxOffset;
            //Vector3 cameraPos = m_Camera.WorldToViewportPoint(speakerPosition);
            //Vector3 cameraPos = Vector3.zero;
           // m_textBox.transform.parent.gameObject.transform.position = cameraPos;

            if (m_bDoneTalking)
            {
                current_conversation = null;
            }

            if (autoTick)
            {
                TickConversation();
            }
        }
    }

}
