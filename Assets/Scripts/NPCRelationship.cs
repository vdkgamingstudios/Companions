using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCRelationship
{
    // Internal ID used by scripts and Yarn.
    public string npcID;

    // Name displayed to the player.
    public string displayName;

    // Current affection value.
    public int affection = 0;

    // Maximum affection this NPC can have.
    public int maxAffection = 100;
}
