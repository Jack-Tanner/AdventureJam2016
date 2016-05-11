using UnityEngine;
using System.Collections;

public class ConversationNode : MonoBehaviour {

    public bool m_bConvCheckpoint;
    public GameObject m_Speaker;

    public string m_requiresObject;

    public string m_sSpeech;
    public bool m_bIsSilent = false;


    public Item m_sGiveItem;
    public string m_sObjective;

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

}
