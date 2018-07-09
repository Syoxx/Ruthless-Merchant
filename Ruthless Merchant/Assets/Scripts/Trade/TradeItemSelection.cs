﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class TradeItemSelection : MonoBehaviour
    { 
        [SerializeField]
        Transform sellingItemParent;

        [SerializeField]
        GameObject sellingItemPrefab;

        [SerializeField]
        Text price;

        List<InventoryItem> listedItems;

        void Awake()
        {
            listedItems = new List<InventoryItem>();
        }

        public void AddItemToSellingList(InventoryItem inventoryItem)
        {
            if (!addToPresent(inventoryItem))
            {
                GameObject newObject = Instantiate(sellingItemPrefab, sellingItemParent);
                newObject.name = "SellingItem";

                InventoryItem newItem = newObject.GetComponent<InventoryItem>();
                newItem.itemQuantity.text = "1x";
                newItem.itemName.text = inventoryItem.itemName.text;
                newItem.itemPrice.text = inventoryItem.itemPrice.text;
                newItem.itemDescription.text = inventoryItem.itemDescription.text;

                listedItems.Add(newItem);
            }

            price.text = (int.Parse(price.text) + int.Parse(inventoryItem.itemPrice.text)).ToString();

            int inventoryItemQuantity = int.Parse(inventoryItem.itemQuantity.text.Replace("x", "")) - 1;

            if (inventoryItemQuantity < 1)
            {
                //Inventory.Singleton.Add()
                Destroy(inventoryItem.gameObject);
            }
            else
            {
                inventoryItem.itemQuantity.text = inventoryItemQuantity.ToString() + "x";
            }
        }

        bool addToPresent(InventoryItem data)
        {
            foreach (InventoryItem item in listedItems)
            {
                if (item.itemName.text == data.itemName.text && item.itemDescription.text == data.itemDescription.text)
                {
                    item.itemQuantity.text = (int.Parse(item.itemQuantity.text.Replace("x", "")) + 1).ToString() + "x";
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemToSellingList()
        {

        }
    }
}
