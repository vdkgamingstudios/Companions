using System.Collections.Generic;
using UnityEngine;

public class RelationshipManager : MonoBehaviour
{
    public static RelationshipManager Instance;

    [Header("NPC Relationships")]
    [SerializeField]
    private List<NPCRelationship> relationships =
        new List<NPCRelationship>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Return an NPC relationship using their ID.
    public NPCRelationship GetRelationship(string npcID)
    {
        return relationships.Find(r => r.npcID == npcID);
    }

    // Return only the affection amount.
    public int GetAffection(string npcID)
    {
        NPCRelationship relationship = GetRelationship(npcID);

        if (relationship == null)
        {
            Debug.LogWarning("NPC not found: " + npcID);
            return 0;
        }

        return relationship.affection;
    }

    // Increase or decrease affection.
    public void ChangeAffection(string npcID, int amount)
    {
        NPCRelationship relationship = GetRelationship(npcID);

        if (relationship == null)
        {
            Debug.LogWarning("NPC not found: " + npcID);
            return;
        }

        relationship.affection += amount;

        relationship.affection = Mathf.Clamp(relationship.affection,0, relationship.maxAffection);

        Debug.Log(relationship.displayName +" affection is now " +relationship.affection);
    }

    // Used later by the relationship menu.
    public List<NPCRelationship> GetAllRelationships()
    {
        return relationships;
    }
}
