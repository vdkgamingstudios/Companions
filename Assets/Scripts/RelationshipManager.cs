using System.Collections.Generic;
using UnityEngine;

public class RelationshipManager : MonoBehaviour
{
    public static RelationshipManager Instance;

    [Header("NPC Relationships")]
    [SerializeField]
    private List<NPCRelationship> relationships = new List<NPCRelationship>();

    private void Awake()
    {
        //Create the singleton instance.
        if (Instance == null)
        {
            Instance = this;

            //Keep relationship data when changing scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Returns the affection value for a specific NPC.
    public int GetAffection(string npcID)
    {
        NPCRelationship relationship = relationships.Find(r => r.npcID == npcID);

        if (relationship == null)
        {
            Debug.LogWarning(
                "No relationship found for NPC: " + npcID
            );

            return 0;
        }

        return relationship.affection;
    }

    //Add or subtract affection from an NPC.
    public void ChangeAffection(string npcID, int amount)
    {
        NPCRelationship relationship = relationships.Find(r => r.npcID == npcID);

        if (relationship == null)
        {
            Debug.LogWarning(
                "No relationship found for NPC: " + npcID
            );

            return;
        }

        relationship.affection += amount;

        //Prevent affection going below 0 or above max.
        relationship.affection = Mathf.Clamp(relationship.affection,0,relationship.maxAffection);

        Debug.Log(relationship.displayName + " affection changed by " + amount + ". Current affection: " + relationship.affection);
    }

    public List<NPCRelationship> GetAllRelationships()
    {
        return relationships;
    }
}
