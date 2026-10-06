using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Item")]
    [SerializeField]
    private ItemData item;

    public void Interact()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found!");

            return;
        }


        if (item == null)
        {
            Debug.LogError("No ItemData has been assigned to this pickup!");

            return;
        }

        //Try putting the item into the inventory.
        bool successfullyAdded = InventoryManager.Instance.AddItem(item);

        //Only remove the physical object if the item successfully entered the inventory. This means if the inventory is full,the Gem stays on the ground.
        if (successfullyAdded)
        {
            Destroy(gameObject);
        }
    }


    //Text displayed by your existing InteractionManager.
    public string GetInteractionText()
    {
        if (item == null)
        {
            return "Pick Up";
        }

        return "Pick Up " + item.displayName;
    }
}
