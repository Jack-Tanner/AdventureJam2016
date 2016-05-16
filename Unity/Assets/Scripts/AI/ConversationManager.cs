﻿using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour {

    public ConversationNode m_CurrentConversationNode;

    public bool HaveSomethingToSay(GameObject npc)
    {
        if (m_CurrentConversationNode == null)
            return false;
       
        //Check we are at the right location
        bool atRequiredLocation = (m_CurrentConversationNode.m_requiredLocation == TrainJourneyManager.TrainJourney.SceneLocation.NONE ||
                                    (m_CurrentConversationNode.m_requiredLocation == TrainJourneyManager.GetInstance().GetCurrentLocation()));

        //Check we have the right item
        string item = m_CurrentConversationNode.m_requiresObject;
        bool hasRequiredItem = string.IsNullOrEmpty(item) || Inventory.GetInstance().HasItem(item);

        //check we are the one to talk
        bool isRequiredSpeaker = m_CurrentConversationNode.m_Speaker == npc;

        if ( isRequiredSpeaker && atRequiredLocation && hasRequiredItem )
        {
            if( string.IsNullOrEmpty( item ) == false && Inventory.GetInstance().IsDataItem( item ) == false )
            {
                Inventory.GetInstance().RemoveItem( item );
            }
            return true;
        }
        return false;
    }

    public bool DoSpeech()
    {
        if (m_CurrentConversationNode.isBranchNode())
        {
            ShowOptions();
            return false;
        }
        else
        {
            if (m_CurrentConversationNode.m_bIsSilent == false)
            {
                SaySpeech(m_CurrentConversationNode);
            }
            if((m_CurrentConversationNode.m_sGiveItem) != null)
            {
                Inventory.GetInstance().PickupItem(m_CurrentConversationNode.m_sGiveItem);
            }
            if(string.IsNullOrEmpty(m_CurrentConversationNode.m_sObjective) == false)
            {
                Inventory.GetInstance().GiveInfoItem(m_CurrentConversationNode.m_sObjective);
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

            return true;
        }
        else
        {
            m_CurrentConversationNode = newNode;
        }
        return false;
    }

    public void SaySpeech(ConversationNode c)
    {
        ConversationOverlord.GetInstance().SendText(c.m_Speaker.name + " : " + c.m_sSpeech);
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
