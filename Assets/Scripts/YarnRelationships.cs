using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnRelationships : MonoBehaviour
{
    [SerializeField]
    private PlayerUIManager playerUIManager;

    //Yarn usage - <<change_affection GameManager "villager" 5>> or <<change_affection GameManager "villager" -5>>
    [YarnCommand("change_affection")]
    public void ChangeAffection(string npcID,int amount)
    {
        if (RelationshipManager.Instance == null)
        {
            Debug.LogError("RelationshipManager instance not found!");

            return;
        }

        //Change the NPC's relationship value.
        RelationshipManager.Instance.ChangeAffection(npcID,amount);

        //Update the HUD immediately.
        if (playerUIManager != null)
        {
            playerUIManager.UpdatePlayerUI();
        }
    }
}
