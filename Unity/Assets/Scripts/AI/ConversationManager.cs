﻿using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour {

    public ConversationNode m_CurrentConversationNode;

    public bool HaveSomethingToSay(GameObject npc)
    {
        if (m_CurrentConversationNode == null)
            return false;
        //Check for speech
        string item = m_CurrentConversationNode.m_requiresObject;
        if (    m_CurrentConversationNode.m_Speaker == npc &&
           (    string.IsNullOrEmpty(item)
                || true) ) //Player.GetInstance().HasItem(item)))
            {
            return true;
        }
        return false;
    }

    public void DoSpeech()
    {
        if (m_CurrentConversationNode.isBranchNode())
        {
            ShowOptions();
            return;
        }
        else
        { 
            SaySpeech(m_CurrentConversationNode);
            if(string.IsNullOrEmpty(m_CurrentConversationNode.m_sGiveItem) == false)
            {
                Debug.Log("Give player item" + m_CurrentConversationNode.m_sGiveItem);
            }
        }

        ConversationNode newNode = m_CurrentConversationNode.GetNext();
        //if null, find location to put the conversation
        if (newNode == null)
        {
            bool foundCheckpoint = false;
            while (foundCheckpoint == false)
            {
                if (m_CurrentConversationNode.transform.parent == null)
                    foundCheckpoint = true;

                ConversationNode parentC = m_CurrentConversationNode.transform.parent.GetComponent<ConversationNode>();
                if (parentC == null)
                {
                    foundCheckpoint = true; //stop looking
                }
                else
                {
                    m_CurrentConversationNode = parentC;
                    foundCheckpoint = m_CurrentConversationNode.m_bConvCheckpoint;
                }
            }
        }
        else
        {
            m_CurrentConversationNode = newNode;
        }
    }

    public void SaySpeech(ConversationNode c)
    {
        ConversationOverlord.GetInstance().SendText(c.m_Speaker + " : " + c.m_sSpeech);
    }

    public void ShowOptions()
    {
        int count = m_CurrentConversationNode.transform.childCount;
        string[] options = new string[count];

        for(int i=0; i< count; ++i)
        {
            ConversationNode c = m_CurrentConversationNode.transform.GetChild(i).GetComponent<ConversationNode>();
            options[i] = c.m_sSpeech;
        }

        ConversationOverlord.GetInstance().ShowOptions(options);
    }


    public void SelectOption(int option)
    {
        //move node
        m_CurrentConversationNode = m_CurrentConversationNode.GetBranchOption(option);
        //and speak
        DoSpeech();
    }
}