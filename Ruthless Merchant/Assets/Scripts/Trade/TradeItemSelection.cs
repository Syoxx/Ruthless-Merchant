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

        List<SlotDisplay> listedItems;

        void Awake()
        {
            listedItems = new List<SlotDisplay>();
            //FindObjectOfType<BookPro>().UpdatePages();
        }

        private void Start()
        {
            for(int x = 0; x < Inventory.Singleton.inventorySlots.Length; x++)
            {

            }
        }

        public void AddItemToSellingList(SlotDisplay inventoryItem)
        {
            if (!addToPresent(inventoryItem))
            {
                GameObject newObject = Instantiate(sellingItemPrefab, sellingItemParent);
                newObject.name = "SellingItem";

                SlotDisplay newItem = newObject.GetComponent<SlotDisplay>();
                newItem.ItemQuantity.text = "1x";
                newItem.ItemName.text = inventoryItem.ItemName.text;
                newItem.ItemPrice.text = inventoryItem.ItemPrice.text;
                newItem.ItemDescription.text = inventoryItem.ItemDescription.text;

                listedItems.Add(newItem);
            }

            price.text = (int.Parse(price.text) + int.Parse(inventoryItem.ItemPrice.text)).ToString();

            int inventoryItemQuantity = int.Parse(inventoryItem.ItemQuantity.text.Replace("x", "")) - 1;

            if (inventoryItemQuantity < 1)
            {
                //Inventory.Singleton.Add()
                Destroy(inventoryItem.gameObject);
            }
            else
            {
                inventoryItem.ItemQuantity.text = inventoryItemQuantity.ToString() + "x";
            }
        }

        bool addToPresent(SlotDisplay data)
        {
            foreach (SlotDisplay item in listedItems)
            {
                if (item.ItemName.text == data.ItemName.text && item.ItemDescription.text == data.ItemDescription.text)
                {
                    item.ItemQuantity.text = (int.Parse(item.ItemQuantity.text.Replace("x", "")) + 1).ToString() + "x";
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemToSellingList()
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
