using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRelationship : MonoBehaviour
{
    //Internal ID used by code and Yarn.Keep this consistent even if the character's display name changes.
    public string npcID;

    //Name shown to the player.
    public string displayName;

    //Current relationship value.
    public int affection = 0;

    //Maximum relationship value.
    public int maxAffection = 100;
}
