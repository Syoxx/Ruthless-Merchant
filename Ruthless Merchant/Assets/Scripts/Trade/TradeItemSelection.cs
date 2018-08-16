using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class TradeItemSelection : MonoBehaviour
    {
        public static TradeItemSelection Singleton;

        [SerializeField]
        Transform sellingItemParent;

        [SerializeField]
        GameObject sellingItemPrefab;

        [SerializeField]
        Text price;

        [SerializeField]
        Button StartTradingButton;

        List<InventoryItem> listedItems;

        void Awake()
        {
            Singleton = this;
            listedItems = new List<InventoryItem>();
            InventoryItem.MoveItem += OnItemMoved;

            if (price == null)
                price = GameObject.Find("TotalPrice").GetComponent<Text>();
        }

        private void Start()
        {
            if (!Trader.CurrentTrader.WantsToStartTrading())
            {
                TradeAbstract.Singleton.TradeDialogue.text = "Nu-uh I'm not trading with ya.";
                TradeAbstract.Singleton.Exit = true;
                Player.RestrictCamera = false;
                Trader.CurrentTrader = null;
                GameObject.Find("UICanvas").SetActive(false);
            }
            else
            {
                Player.Singleton.EnterTrading();
            }
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

            UpdatePrice();
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

        public void RemoveItemFromSellingList(InventoryItem item)
        {
            Inventory.Singleton.Add(item.Slot.ItemInfo, 1, true);

            int newQuantity = int.Parse(item.ItemQuantity.text.Replace("x", "")) - 1;
            item.ItemQuantity.text = newQuantity.ToString() + "x";

            if (newQuantity <= 0)
            {
                listedItems.Remove(item);
                Destroy(item.gameObject);
            }

            UpdatePrice();
        }

        void UpdatePrice()
        {
            float totalPrice = 0;

            foreach(InventoryItem item in listedItems)
            {
                totalPrice += int.Parse(item.ItemPrice.text.Replace("G","")) * int.Parse(item.ItemQuantity.text.Replace("x", ""));
            }

            price.text = totalPrice.ToString() + "G";

            StartTradingButton.interactable = totalPrice > 0;
        }

        public void ConfirmTradeItems()
        {
            int totalPrice = int.Parse(price.text.Replace("G", ""));

            if (totalPrice > 0)
            {
                TradeAbstract.Singleton.Initialize(totalPrice);
                TradeAbstract.Singleton.ItemsToSell = listedItems;
                Player.RestrictCamera = false;
                GameObject.Find("UICanvas").SetActive(false);
                Player.Singleton.StartTrading();
                InventoryItem.MoveItem -= OnItemMoved;
            }
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
