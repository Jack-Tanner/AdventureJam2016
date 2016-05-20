using UnityEngine;
using System.Collections;

public class ConversationNode : MonoBehaviour
{

    public bool m_bConvCheckpoint;
    public GameObject m_Speaker;

    public string m_requiresObject;
    public string m_requiresAnotherObject;
    public TrainJourneyManager.TrainJourney.SceneLocation m_requiredLocation = TrainJourneyManager.TrainJourney.SceneLocation.NONE;
    public bool m_requiredToBeOnTrain = true;
    public bool m_trainMustBeStopped = false;

    
    public string m_sSpeech;
    public bool m_bIsSilent = false;


    public Item m_sGiveItem;
    public string m_sObjective;

    public string m_setAnimation;

    public bool m_bDoTheSpookyThing = false;
    public bool m_bFadeToWhite = false;

    public ConversationNode GetNext()
    {
        return GetBranchOption(0);
    }

    public ConversationNode GetBranchOption(int index)
    {
        if (transform.childCount == 0)
            return null;

        Transform child = transform.GetChild(index);
        if(child == null)
            return null;
        return child.GetComponent<ConversationNode>();
    }

    public bool isBranchNode()
    {
        return transform.childCount > 1;
    }

    /// <summary>
    /// Returns true and assigns to the vector if we were able to get the location to place the speech text.
    /// </summary>
    /// <param name="inVector">reference to where the conversation location will go.</param>
    /// <returns>true if the vector was populated.</returns>
    public bool TryGetConversationLocation( ref Vector3 inVector )
    {
        if( m_Speaker != null )
        {
            SpeechPosition interactable = m_Speaker.gameObject.GetComponent<SpeechPosition>();
            if( interactable != null )
            {
                inVector = interactable.GetPosition();
                return true;
            }
        }

        // Failed to get location.
        return false;
    }
}
