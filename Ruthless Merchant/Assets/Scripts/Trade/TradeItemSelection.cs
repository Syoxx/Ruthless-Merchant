using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class TradeItemSelection : MonoBehaviour
    {
        public static TradeItemSelection Singleton;
        private bool abort = false;

        [SerializeField]
        Transform sellingItemParent;

        [SerializeField]
        GameObject sellingItemPrefab;

        [SerializeField]
        Text price;

        [SerializeField]
        Button abortTradingButton;

        [SerializeField]
        Button startTradingButton;

        [SerializeField]
        GameObject sellingItemInfoPrefab;

        [SerializeField]
        GameObject sellingItemInfoList;

        [SerializeField]
        GameObject sellingItemInfoParent;

        [SerializeField]
        Text sellingItemInfoPrice;

        List<InventoryItem> listedItems;

        void Awake()
        {
            Singleton = this;
            listedItems = new List<InventoryItem>();
            sellingItemInfoList.SetActive(false);
            InventoryItem.ResetEvent();
            InventoryItem.MoveItem += OnItemMoved;

            if (price == null)
                price = GameObject.Find("TotalPrice").GetComponent<Text>();

            Trader.TradeHasLoaded = true;
        }

        public void OnItemMoved(InventoryItem item)
        {
            Debug.Log("Moving " + item.ItemName.text + " !");
            MoveItemToSellingList(item);
        }

        /// <summary>
        /// Moves an InventoryItem in the Inventory to the selling list.
        /// </summary>
        /// <param name="inventoryItem">The InventoryItem to be added</param>
        public void MoveItemToSellingList(InventoryItem inventoryItem)
        {
            if (int.Parse(inventoryItem.ItemQuantity.text.Replace("x", "")) > 0)
            {
                if (!MoveToExisting(inventoryItem))
                {
                    GameObject newObject = Instantiate(sellingItemPrefab, sellingItemParent);
                    newObject.name = "SellingItem";

                    InventoryItem newItem = newObject.GetComponent<InventoryItem>();
                    newItem.ItemQuantity.text = "1x";
                    newItem.ItemName.text = inventoryItem.ItemName.text;
                    newItem.ItemPrice.text = inventoryItem.ItemPrice.text;
                    newItem.ItemDescription.text = inventoryItem.ItemDescription.text;
                    newItem.ItemImage.sprite = inventoryItem.ItemImage.sprite;
                    newItem.Location = InventoryItem.UILocation.ExternList;

                    newItem.Slot.ItemInfo = inventoryItem.Slot.ItemInfo;
                    newItem.Slot.Item = inventoryItem.Slot.Item;
                    listedItems.Add(newItem);
                }

                UpdatePrice();
            }
        }

        /// <summary>
        /// Adds the InventoryItem to its existing stack in the selling list.
        /// </summary>
        /// <param name="inventoryItem">The InventoryItem to be added</param>
        /// <returns>False if no stack of the inventoryItem's type exists.</returns>
        bool MoveToExisting(InventoryItem inventoryItem)
        {
            foreach (InventoryItem item in listedItems)
            {
                if (item.ItemName.text == inventoryItem.ItemName.text && item.ItemDescription.text == inventoryItem.ItemDescription.text)
                {
                    item.ItemQuantity.text = (int.Parse(item.ItemQuantity.text.Replace("x", "")) + 1).ToString() + "x";
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Moves an InventoryItem from the selling list to the Inventory.
        /// </summary>
        /// <param name="item"></param>
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

        /// <summary>
        /// Updates the total price of all items to be sold.
        /// </summary>
        void UpdatePrice()
        {
            float totalPrice = 0;

            foreach(InventoryItem item in listedItems)
            {
                totalPrice += int.Parse(item.ItemPrice.text.Replace("G","")) * int.Parse(item.ItemQuantity.text.Replace("x", ""));
            }

            price.text = totalPrice.ToString() + "G";

            if (totalPrice > 0 && (!Trader.CurrentTrader.startTradeImmediately || Tutorial.Singleton != null && Tutorial.Singleton.isTutorial && int.Parse(listedItems[0].ItemQuantity.text.Replace("x","")) == 5))
            {
                startTradingButton.interactable = true;
            }
            else
            {
                startTradingButton.interactable = false;
            }
        }

        /// <summary>
        /// Called when the Player confirms the selling list. Starts the actual trading.
        /// </summary>
        public void ConfirmTradeItems()
        {
            int totalPrice = int.Parse(price.text.Replace("G", ""));

            TradeAbstract.Singleton.Initialize(totalPrice);
            TradeAbstract.Singleton.ItemsToSell = listedItems;
            Player.Singleton.AllowTradingMovement();
            InventoryItem.MoveItem -= OnItemMoved;
            gameObject.SetActive(false);

            sellingItemInfoList.SetActive(true);
            sellingItemInfoPrice.text = price.text;

            foreach (InventoryItem inventoryItem in listedItems)
            {
                Text itemInfoText = Instantiate(sellingItemInfoPrefab, sellingItemInfoParent.transform).GetComponentsInChildren<Text>()[0];
                itemInfoText.text = inventoryItem.ItemQuantity.text + " " + inventoryItem.ItemName.text + " " + inventoryItem.ItemRarity.text;
            }

            sellingItemInfoParent.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            Trader.CurrentTrader.SpawnMoodIcon();

            Tutorial.Singleton.TraderItemSelectionMonolog2();
        }

        /// <summary>
        /// Called when the Player aborts the trading clicking on the abort button in the selling list.
        /// </summary>
        public void AbortTrade()
        {
            abort = true;

            for (int x = listedItems.Count - 1; x >= 0; x--)
            {
                Inventory.Singleton.Add(listedItems[x].Slot.ItemInfo, int.Parse(listedItems[x].ItemQuantity.text.Replace("x","")), true);
                InventoryItem temp = listedItems[x];
                listedItems.Remove(listedItems[x]);
                Destroy(temp.gameObject);
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            InventoryItem.MoveItem -= OnItemMoved;

            Player player = Player.Singleton;
            player.RestrictBookUsage = false;
            player.AllowTradingMovement();
            player.NavMeshObstacle.enabled = false;

            if(Trader.CurrentTrader.Scale)
                Trader.CurrentTrader.Scale.SetActive(true);

            Main_SceneManager.UnLoadScene("TradeScene");
        }
    }
}
