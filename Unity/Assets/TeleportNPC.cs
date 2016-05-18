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

        if (Inventory.GetInstance().HasItem(m_RequiredObjective) == false)
            return;

        foundNPC = GameObject.Find(NameOfNPCToTeleport);
        if(foundNPC == null)
        {
            Debug.LogError("COULD NOT FIND NPC " + NameOfNPCToTeleport + " TO TELEPORT");
        }
        m_oldTransformParent = foundNPC.transform.parent;
        m_oldPosition = foundNPC.transform.position;



        foundNPC.transform.parent = transform.parent;
        foundNPC.transform.position = transform.position;
    }



    void OnDestroy()
    {
        if (foundNPC != null)
        {
            foundNPC.transform.parent = m_oldTransformParent;
            foundNPC.transform.position = m_oldPosition;
        }
    }
}
