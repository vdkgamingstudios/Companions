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
}
