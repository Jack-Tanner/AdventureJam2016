﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ConversationManager : MonoBehaviour
{
    public ConversationNode m_CurrentConversationNode;

    public bool HaveSomethingToSay(GameObject npc)
    {
        if (m_CurrentConversationNode == null)
            return false;
       
        //Check we are at the right location
        bool atRequiredLocation = (m_CurrentConversationNode.m_requiredLocation == TrainJourneyManager.TrainJourney.SceneLocation.NONE ||
                                    (m_CurrentConversationNode.m_requiredLocation == TrainJourneyManager.GetInstance().GetCurrentLocation()));

        bool shouldBeOnTrainOrOutside = m_CurrentConversationNode.m_requiredToBeOnTrain == TrainJourneyManager.GetInstance().IsOnTrain();

        bool trainStopCheck = (m_CurrentConversationNode.m_trainMustBeStopped == false) || TrainJourneyManager.GetInstance().HasTrainStopped();

        //Check we have the right item
        string item = m_CurrentConversationNode.m_requiresObject;
        bool hasRequiredItem = string.IsNullOrEmpty(item) || Inventory.GetInstance().HasItem(item);
        
        string otheritem = m_CurrentConversationNode.m_requiresAnotherObject;
        bool hasRequiredOtherItem = string.IsNullOrEmpty(otheritem) || Inventory.GetInstance().HasItem(otheritem);



        //check we are the one to talk
        bool isRequiredSpeaker = m_CurrentConversationNode.m_Speaker == npc;

        if (isRequiredSpeaker && atRequiredLocation && hasRequiredItem && shouldBeOnTrainOrOutside && trainStopCheck && hasRequiredOtherItem)
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

            if (string.IsNullOrEmpty(m_CurrentConversationNode.m_setAnimation) == false)
            {
                if (m_CurrentConversationNode.m_Speaker != null)
                {
                    Animator animator = m_CurrentConversationNode.m_Speaker.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetTrigger(m_CurrentConversationNode.m_setAnimation);
                    }
                }
            }

            //#lazy
            if (m_CurrentConversationNode.m_bDoTheSpookyThing)
            {
                //lol sorry dan
                Quest q = m_CurrentConversationNode.m_Speaker.GetComponent<Quest>();
                q.CheckItem(q.m_CompleteQuestItem);

                ConversationOverlord.GetInstance().SendText("", Vector3.one*9001); //far too lazy to do this correctly
            }

            if (m_CurrentConversationNode.m_bFadeToWhite)
            {
                Fade.GetInstance().FadeToWhite();
                Fade.GetInstance().OnFadeComplete += LoadOutroScene;
                ConversationOverlord.GetInstance().SendText("", Vector3.one * 9001);//far too lazy to do this correctly
            }

            if (m_CurrentConversationNode.m_AudioToPlay != null)
            {
                ConversationOverlord.GetInstance().m_AudioSourceSFX.PlayOneShot(m_CurrentConversationNode.m_AudioToPlay);
            }

            if (m_CurrentConversationNode.m_fPauseTime > 0.0f)
            {
                ConversationOverlord.GetInstance().PauseInput(m_CurrentConversationNode.m_fPauseTime);
            }

            if (m_CurrentConversationNode.m_bNotText)
            {
                ConversationOverlord.GetInstance().SendText("", Vector3.one * 9001);//far too lazy to do this correctly
            }

            if (m_CurrentConversationNode.m_destroyThis != null)
            {
                GameObject.Destroy(m_CurrentConversationNode.m_destroyThis);
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

    public void LoadOutroScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void SaySpeech(ConversationNode c)
    {
        Vector3 textPosition = Vector3.zero;
        if( c.TryGetConversationLocation( ref textPosition ) == false )
        {
            Debug.LogError( "NO INTERACTION SCRIPT SET ON CONVERSATION NODE " + c.m_Speaker.name );
        }
        if(string.IsNullOrEmpty(c.m_sSpeech))
        {
            return;
        }

        string prefix = "";
        Interactable interactable = c.m_Speaker.GetComponent<Interactable>();
        if (interactable != null)
        {
            prefix = interactable.m_HightlightName + " : ";
        }

        ConversationOverlord.GetInstance().SendText( prefix + c.m_sSpeech, textPosition);
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
