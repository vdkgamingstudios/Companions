using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Allows other scripts to access the inventory.
    public static InventoryManager Instance;

    [Header("Player Inventory")]

    //Items currently owned by the player.
    [SerializeField]
    private List<ItemData> items = new List<ItemData>();

    [SerializeField]
    private int maxInventorySize = 16;

    //Allows the Inventory UI to read the items.
    public List<ItemData> Items => items;

    [Header("Available Items")]

    //All ItemData assets that can be added through merchants, quests or other systems.
    [SerializeField]
    private List<ItemData> allItems = new List<ItemData>();


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

    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tried to add an empty item to the inventory.");

            return false;
        }

        //Don't allow more than 16 items.
        if (!HasSpace())
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        //Add the item.
        items.Add(item);

        Debug.Log("Added " +item.displayName +" to inventory.");

        //Update the inventory screen.
        RefreshInventoryUI();

        return true;
    }

    public bool HasSpace()
    {
        return items.Count < maxInventorySize;
    }

    public ItemData GetItemData(string itemID)
    {
        return allItems.Find(
            item => item != null && item.itemID == itemID
        );
    }

    public bool AddItemByID(string itemID)
    {
        ItemData item = GetItemData(itemID);

        if (item == null)
        {
            Debug.LogWarning(
                "Item not found in database: " + itemID
            );

            return false;
        }

        return AddItem(item);
    }

    public bool HasItem(string itemID)
    {
        return items.Exists(
            item => item != null && item.itemID == itemID
        );
    }

    public int GetItemCount(string itemID)
    {
        return items.FindAll(
            item => item != null && item.itemID == itemID
        ).Count;
    }

    public bool RemoveItem(string itemID)
    {
        ItemData item = items.Find(inventoryItem =>inventoryItem != null &&inventoryItem.itemID == itemID);

        //Player doesn't own this item.
        if (item == null)
        {
            Debug.Log("Player does not have item: " + itemID);

            return false;
        }

        //Remove it from the inventory.
        items.Remove(item);

        Debug.Log("Removed " +item.displayName +" from inventory.");

        //Update the inventory screen.
        RefreshInventoryUI();

        return true;
    }

    public ItemData GetItem(string itemID)
    {
        return items.Find(item => item != null && item.itemID == itemID);
    }

    private void RefreshInventoryUI()
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>(true);

        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory();
        }
    }
}
