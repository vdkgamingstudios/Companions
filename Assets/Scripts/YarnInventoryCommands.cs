using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnInventoryCommands : MonoBehaviour
{
    [Header("Quest Rewards")]
    //Drag the Special Gift ItemData asset here in the Inspector.
    [SerializeField]
    private ItemData specialGift;

    //Check if the player has an item. <<if has_item("gem")>> Because this is static, Yarn doesn't need a GameObject target.
    [YarnFunction("has_item")]
    public static bool HasItem(string itemID)
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found!");

            return false;
        }

        return InventoryManager.Instance.HasItem(itemID);
    }

    //<<remove_item GameManager "gem">> This is NOT static, so Yarn needs to know which GameObject contains this component.
    [YarnCommand("remove_item")]
    public void RemoveItem(string itemID)
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found!");

            return;
        }

        InventoryManager.Instance.RemoveItem(itemID);
    }

    //<<give_special_gift GameManager>>
    [YarnCommand("give_special_gift")]
    public void GiveSpecialGift()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found!");

            return;
        }

        if (specialGift == null)
        {
            Debug.LogError("Special Gift has not been assigned!");

            return;
        }

        InventoryManager.Instance.AddItem(
            specialGift
        );
    }
}
