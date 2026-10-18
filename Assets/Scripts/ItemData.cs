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

    [Header("Merchant Settings")]

    // Whether merchants are allowed to sell this item.
    public bool canBuy = true;

    // Whether the player can sell this item.
    public bool canSell = true;

    // Price when buying from a merchant.
    [Min(0)]
    public int buyPrice = 25;

    // Price when selling to a merchant.
    [Min(0)]
    public int sellPrice = 10;
}
