using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    public void InteractWithNPC()
    {
        ConversationManager cM = null;
        bool hadSomethingToSay = false;
        int count = ConversationOverlord.GetInstance().transform.childCount;
        for (int i = 0; i < count; ++i)
        {
            cM = ConversationOverlord.GetInstance().transform.GetChild(i).GetComponent<ConversationManager>();
            if (cM == null)
                continue;

            hadSomethingToSay = cM.HaveSomethingToSay(gameObject);

            if (hadSomethingToSay)
            {
                break;
            }
        }

        if ((hadSomethingToSay == false) || cM == null)
        {
            Debug.Log("I have nothing to say right now");
        }
        else
        {
            ConversationOverlord.GetInstance().current_conversation = cM;
            ConversationOverlord.GetInstance().TickConversation();
        }
    }



}
