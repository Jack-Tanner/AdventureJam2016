using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConversationOverlord : MonoBehaviour {

    public static ConversationOverlord instance;
    public static ConversationOverlord GetInstance()
    {
        return instance;
    }
    public void Awake()
    {
        instance = this;
    }


    public ConversationManager current_conversation;
    public Text m_textBox;
    public GameObject m_optionsGo;

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

    public void SelectOption(int index)
    {
        current_conversation.SelectOption(index);
        m_optionsGo.SetActive(false);
    }

    public void TickConversation()
    {
        if (current_conversation == null)
        {
            Debug.Log("Not in conversation");
        }
        else
        {
            current_conversation.DoSpeech();
        }
    }

}
