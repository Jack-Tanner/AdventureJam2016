using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConversationOverlord : MonoBehaviour {

    public static ConversationOverlord instance;
    public ConversationManager current_conversation;
    public Text m_textBox;
    public Canvas m_Canvas;

    public GameObject m_optionsGo;
    public Vector3 m_SpeechBoxOffset;
    public Camera m_Camera;


    public static ConversationOverlord GetInstance()
    {
        return instance;
    }

    public void Awake()
    {
        instance = this;
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void SendText(string text)
    {
        m_textBox.text = text;
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
            m_textBox.transform.parent.gameObject.SetActive(false);
            m_bDoneTalking = false;
        }

        if (current_conversation == null)
        {

        }
        else
        {
            bool autoTick = current_conversation.m_CurrentConversationNode.m_bIsSilent;
            m_bDoneTalking = current_conversation.DoSpeech();
            m_textBox.transform.parent.gameObject.SetActive(true);
            //Vector3 speakerPosition = current_conversation.m_CurrentConversationNode.m_Speaker.transform.position;
            //speakerPosition += m_SpeechBoxOffset;
            //Vector3 cameraPos = m_Camera.WorldToViewportPoint(speakerPosition);
            Vector3 cameraPos = Vector3.zero;
            m_textBox.transform.parent.gameObject.transform.position = cameraPos;

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
