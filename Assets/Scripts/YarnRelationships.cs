using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnRelationships : MonoBehaviour
{
    [YarnCommand("change_affection")]
    public void ChangeAffection(string npcID,int amount)
    {
        if (RelationshipManager.Instance == null)
        {
            Debug.LogError("RelationshipManager missing!");

            return;
        }

        RelationshipManager.Instance.ChangeAffection(npcID, amount);
    }

    //Allows Yarn dialogue to read an NPC's affection value. Yarn usage: get_affection("characterone")
    [YarnFunction("get_affection")]
    public static int GetAffection(string npcID)
    {
        if (RelationshipManager.Instance == null)
        {
            Debug.LogWarning("RelationshipManager instance not found!");
            return 0;
        }

        return RelationshipManager.Instance.GetAffection(
            npcID
        );
    }
}
