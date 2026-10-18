using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MerchantCommands : MonoBehaviour
{
    [YarnFunction("can_buy_item")]
    public static bool CanBuyItem(string itemID)
    {
        if (InventoryManager.Instance == null ||
            GameManager.Instance == null)
        {
            return false;
        }

        ItemData item =
            InventoryManager.Instance.GetItemData(itemID);

        if (item == null || !item.canBuy)
        {
            return false;
        }

        return InventoryManager.Instance.HasSpace() &&
               GameManager.Instance.HasEnoughMoney(item.buyPrice);
    }

    [YarnFunction("can_sell_item")]
    public static bool CanSellItem(string itemID)
    {
        if (InventoryManager.Instance == null ||
            GameManager.Instance == null)
        {
            return false;
        }

        ItemData item =
            InventoryManager.Instance.GetItemData(itemID);

        if (item == null || !item.canSell)
        {
            return false;
        }

        return InventoryManager.Instance.HasItem(itemID);
    }

    [YarnCommand("buy_item")]
    public void BuyItem(string itemID)
    {
        if (!CanBuyItem(itemID))
        {
            Debug.Log("Cannot buy item: " + itemID);
            return;
        }

        ItemData item =
            InventoryManager.Instance.GetItemData(itemID);

        // Add the item first.
        bool added =
            InventoryManager.Instance.AddItem(item);

        // Only deduct money if the item was added.
        if (added)
        {
            GameManager.Instance.SpendMoney(item.buyPrice);

            RefreshMoneyUI();

            Debug.Log("Bought " + item.displayName);
        }
    }

    [YarnCommand("sell_item")]
    public void SellItem(string itemID)
    {
        if (!CanSellItem(itemID))
        {
            Debug.Log("Cannot sell item: " + itemID);
            return;
        }

        ItemData item =
            InventoryManager.Instance.GetItemData(itemID);

        // Remove one item from inventory.
        bool removed =
            InventoryManager.Instance.RemoveItem(itemID);

        // Only pay the player if removal succeeds.
        if (removed)
        {
            GameManager.Instance.AddMoney(item.sellPrice);

            RefreshMoneyUI();

            Debug.Log("Sold " + item.displayName);
        }
    }

    private void RefreshMoneyUI()
    {
        PlayerUIManager playerUI =
            FindObjectOfType<PlayerUIManager>(true);

        if (playerUI != null)
        {
            playerUI.UpdatePlayerUI();
        }
    }
}
