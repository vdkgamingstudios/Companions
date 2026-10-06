using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image iconImage;

    //The item currently displayed in this slot.
    private ItemData currentItem;
    public ItemData CurrentItem => currentItem;


    //Put an item into this slot.
    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (iconImage != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
    }


    //Make this slot empty.
    public void ClearSlot()
    {
        currentItem = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }
}
