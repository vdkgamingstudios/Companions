using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Allows other scripts to easily access the inventory.
    public static InventoryManager Instance;

    [Header("Inventory")]

    //All items currently owned by the player.
    [SerializeField]
    private List<ItemData> items = new List<ItemData>();

    [SerializeField]
    private int maxInventorySize = 16;

    //Allows the Inventory UI to read the inventory,but prevents it from directly replacing the list.
    public List<ItemData> Items => items;


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
        if (items.Count >= maxInventorySize)
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

    public bool HasItem(string itemID)
    {
        return items.Exists(
            item => item.itemID == itemID
        );
    }

    public bool RemoveItem(string itemID)
    {
        ItemData item =items.Find(inventoryItem =>inventoryItem.itemID == itemID);

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
        return items.Find(item => item.itemID == itemID);
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
