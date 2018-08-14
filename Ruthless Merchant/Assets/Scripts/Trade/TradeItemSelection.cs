using System.Collections;
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
            InventoryItem.MoveItem += OnItemMoved;
        }

        public void OnItemMoved(InventoryItem item)
        {
            Debug.Log("Moving " + item.ItemName.text + " !");
            AddItemToSellingList(item);
        }

        public void AddItemToSellingList(InventoryItem inventoryItem)
        {
            if (!addToPresent(inventoryItem))
            {
                GameObject newObject = Instantiate(sellingItemPrefab, sellingItemParent);
                newObject.name = "SellingItem";

                InventoryItem newItem = newObject.GetComponent<InventoryItem>();
                newItem.ItemQuantity.text = "1x";
                newItem.ItemName.text = inventoryItem.ItemName.text;
                newItem.ItemPrice.text = inventoryItem.ItemPrice.text;
                newItem.ItemDescription.text = inventoryItem.ItemDescription.text;
                newItem.Location = InventoryItem.UILocation.ExternList;

                newItem.Slot.ItemInfo = inventoryItem.Slot.ItemInfo;

                listedItems.Add(newItem);
            }

            price.text = (int.Parse(price.text.Replace("G", "")) + int.Parse(inventoryItem.ItemPrice.text.Replace("G", ""))).ToString();

            //int inventoryItemQuantity = int.Parse(inventoryItem.ItemQuantity.text.Replace("x", "")) - 1;

            //if (inventoryItemQuantity < 1)
            //{
            //    Destroy(inventoryItem.gameObject);
            //}
            //else
            //{
            //    inventoryItem.ItemQuantity.text = inventoryItemQuantity.ToString() + "x";
            //}
        }

        bool addToPresent(InventoryItem data)
        {
            foreach (InventoryItem item in listedItems)
            {
                if (item.ItemName.text == data.ItemName.text && item.ItemDescription.text == data.ItemDescription.text)
                {
                    item.ItemQuantity.text = (int.Parse(item.ItemQuantity.text.Replace("x", "")) + 1).ToString() + "x";
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemFromSellingList()
        {

        }

        public void ConfirmTradeItems()
        {
            Trade.Singleton.Initialize(int.Parse(price.text));
            GameObject.Find("UICanvas").SetActive(false);
            Player.RestrictCamera = false;
        }

        private void LateUpdate()
        {
            //TODO: Clean this.

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Player.RestrictCamera = true;
        }
    }
}
