using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Item",
    menuName = "COTV/Item"
)]

public class ItemData : ScriptableObject
{
    [Header("Item Information")]

    //Internal ID used by Yarn and other scripts.Example: flower
    public string itemID;

    //Name shown to the player.Example: Flower
    public string displayName;

    //Image displayed in the inventory.
    public Sprite icon;

    [TextArea]
    public string description;
}
