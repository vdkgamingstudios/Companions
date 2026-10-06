using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Slots")]

    [SerializeField]
    private InventorySlot[] inventorySlots;

    //Every time the Inventory screen becomes active,update what is displayed.
    private void OnEnable()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        if (InventoryManager.Instance == null)
        {
            return;
        }

        //Clear old display
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.ClearSlot();
            }
        }

        //Display current items
        for (int i = 0;
             i < InventoryManager.Instance.Items.Count;
             i++)
        {
            // Stop if we've run out of UI slots.
            if (i >= inventorySlots.Length)
            {
                break;
            }

            inventorySlots[i].SetItem(
                InventoryManager.Instance.Items[i]
            );
        }
    }
}
