using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportNPC : MonoBehaviour
{

    public string NameOfNPCToTeleport = "";
    public string m_RequiredObjective = "";

    public GameObject foundNPC;
    public Transform m_oldTransformParent;
    public Vector3 m_oldPosition;

    void Awake()
    {
        if (string.IsNullOrEmpty(m_RequiredObjective) == false)
        {
            if (Inventory.GetInstance().HasItem(m_RequiredObjective) == false)
                return;
        }

        foundNPC = GameObject.Find(NameOfNPCToTeleport);
        if(foundNPC == null)
        {
            Debug.LogError("COULD NOT FIND NPC " + NameOfNPCToTeleport + " TO TELEPORT");
        }
        m_oldTransformParent = foundNPC.transform.parent;
        m_oldPosition = foundNPC.transform.position;

        TrainJourneyManager.GetInstance().m_OnTransition += TransitionOutside;
    }

    public void TransitionOutside()
    {
        //lool very hacky
        TrainJourneyManager.GetInstance().m_OnTransition -= TransitionOutside;

        foundNPC.transform.parent = transform.parent;
        foundNPC.transform.position = transform.position;

        //lol hacky
        Animator animator = foundNPC.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Stand");
        }
    }

    void TransitionInside()
    {
        TrainJourneyManager.GetInstance().m_OnTransition -= TransitionInside;

        foundNPC.transform.parent = m_oldTransformParent;
        foundNPC.transform.position = m_oldPosition;

        Animator animator = foundNPC.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }

    }


    void OnDestroy()
    {
        if (foundNPC != null)
        {
            TrainJourneyManager.GetInstance().m_OnTransition += TransitionInside;
        }
    }
}
