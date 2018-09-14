using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RuthlessMerchant
{
    public class Inventory : MonoBehaviour
    {
        #region Fields

        public static Inventory Singleton;

        private Item[] items;
        public InventorySlot[] inventorySlots;

        [SerializeField]
        private int maxSlotCount = 10;

        public List<Item> startinventory;
        private UnityEvent inventoryChanged;

        [System.NonSerialized]
        public PageLogic BookLogic = null;

        [System.NonSerialized]
        public GameObject ItemUIPrefab = null;

        #endregion

        #region Properties

        public Item[] Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        public InventorySlot[] InventorySlots
        {
            get
            {
                return inventorySlots;
            }
            set
            {
                inventorySlots = value;
            }
        }

        public UnityEvent InventoryChanged
        {
            get
            {
                return inventoryChanged;
            }
        }

        public int PlayerMoney
        {
            get
            {
                int slotIndex = FindMoneySlot();
                int amount = 0;
                if (slotIndex != -1)
                {
                    if (InventorySlots[slotIndex].Count >= 0)
                    {
                        amount = InventorySlots[slotIndex].Count;
                    }
                }
                
                return amount;
            }
            set
            {
                int slotIndex = FindMoneySlot();
                int amount = value;
                if (slotIndex != -1)
                {
                    inventorySlots[slotIndex].Count += amount;
                }
                else
                {
                    ItemInfo gold = new ItemInfo();

                    gold.ItemName = "Gold Coin";

                    Add(gold, amount, false);
                }
            }
        }

        #endregion

        #region Private Functions

        private void Awake()
        {
            Singleton = this;
            inventorySlots = new InventorySlot[maxSlotCount];
            inventoryChanged = new UnityEvent();
        }

        private void Start()
        {
            //Debugging
            foreach (Item item in startinventory)
            {
                Add(item, 1, true);
            }
        }

        /// <summary>
        /// Finds the index of a specific item in the inventory
        /// </summary>
        /// <param name="item">the type of Item</param>
        /// <returns>returns the index of the item in inventorySlots</returns>
        private int FindItem(Item item)
        {
            for (int i = 0; i < maxSlotCount; i++)
            {
                if (inventorySlots[i].Item)
                {
                    if (inventorySlots[i].ItemInfo.ItemName == item.ItemInfo.ItemName)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Identifies the inventory slot where player money is stored
        /// </summary>
        /// <returns>
        /// Returns the slot # if found, otherwise returns -1
        /// </returns>
        private int FindMoneySlot()
        {
            for (int i = 0; i < maxSlotCount; i++)
            {
                if (InventorySlots[i].ItemInfo.ItemName == "Gold Coin")
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// re-arranges the display panel to it's designated place in the book
        /// </summary>
        /// <param name="inventorySlot"></param>
        private void SortDisplayPanel(int inventorySlot)
        {
            int pageForItem = inventorySlot / Player.Singleton.MaxItemsPerPage;
            inventorySlots[inventorySlot].DisplayData.transform.SetParent(BookLogic.InventoryPageList[pageForItem].transform.GetChild(0), false);
            inventorySlots[inventorySlot].DisplayData.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Creates a Inventory-Display for a specific Inventoryslot. This Function is only used if UpdateDisplayData couldn't find a InventoryItem
        /// </summary>
        /// <param name="inventorySlot">inventoryslot the function will create</param>
        /// <returns>returns the created display</returns>
        private InventoryItem CreateDisplayData(InventorySlot inventorySlot)
        {
            if (ItemUIPrefab == null)
                throw new System.NullReferenceException("no ItemUIPrefab found");

            Debug.LogWarning("Create Display Data");

            Transform parent = BookLogic.InventoryPageList[BookLogic.PageForCurrentItemPlacement()].transform.Find("PNL_ZoneForItem");
            GameObject inventoryItemObject = Instantiate(ItemUIPrefab, parent) as GameObject;
            //inventoryItem.transform.SetParent(parent, false);

            InventoryItem inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();

            inventoryItem.Slot = inventorySlot;

            inventoryItem.ItemName.text = inventorySlot.ItemInfo.ItemName;
            inventoryItem.ItemQuantity.text = inventorySlot.Count + "x ";
            switch (inventorySlot.ItemInfo.ItemRarity)
            {
                case ItemRarity.Üblich:
                    inventoryItem.ItemName.color = new Color(0, 0, 0);
                    break;
                case ItemRarity.Ungewöhnlich:
                    inventoryItem.ItemName.color = new Color(0, 0.2f, 1);
                    break;
                case ItemRarity.Selten:
                    inventoryItem.ItemName.color = new Color(0.7f, 0, 1);
                    break;
            }
            inventoryItem.ItemDescription.text = inventorySlot.ItemInfo.ItemLore;
            inventoryItem.ItemPrice.text = inventorySlot.ItemInfo.ItemValue.ToString() + "G";

            if (inventorySlot.ItemInfo.ItemSprite != null)
            {
                inventoryItem.ItemImage.sprite = inventorySlot.ItemInfo.ItemSprite;
            }

            return inventoryItem;
        }

        /// <summary>
        /// Updates the Inventory-Display of a specific Inventoryslot
        /// </summary>
        /// <param name="inventorySlot">inventoryslot the function will update</param>
        /// <returns>returns the created display</returns>
        private InventoryItem UpdateDisplayData(InventorySlot inventorySlot)
        {
           // Debug.Log("UpdateDisplayData");

            if (ItemUIPrefab == null)
                throw new System.NullReferenceException("no ItemUIPrefab found");

            InventoryItem inventoryItem = inventorySlot.DisplayData;
            if (inventorySlot.ItemInfo.ItemName == null && inventoryItem != null)
            {
                Destroy(inventorySlot.DisplayData.gameObject);
                inventorySlot.DisplayData = null;
                inventorySlot.Count = 0;
                inventorySlot.ItemInfo = new ItemInfo();
                return null;
            }
            if (inventorySlot.ItemInfo.ItemName == null && inventoryItem == null)
            {
                return null;
            }
            if (inventoryItem == null)
            {
                return CreateDisplayData(inventorySlot);
            }

            inventoryItem.ItemName.text = inventorySlot.ItemInfo.ItemName;
            inventoryItem.ItemQuantity.text = inventorySlot.Count + "x ";
            inventoryItem.ItemDescription.text = inventorySlot.ItemInfo.ItemLore;


            inventoryItem.ItemPrice.text = inventorySlot.ItemInfo.ItemValue.ToString() + "G";

            if (inventorySlot.ItemInfo.ItemSprite != null)
            {
                inventoryItem.ItemImage.sprite = inventorySlot.ItemInfo.ItemSprite;
            }

            return inventoryItem;
        }

        /// <summary>
        /// Sorts the Inventory. Priorities Faction, then Type, then Rarity, then name.
        /// </summary>
        void SortInventory() //I know this Method looks huge, but it's just checking on the different things it can sort after.
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                for (int k = inventorySlots.Length - 1; k > i; k--)
                {
                    //try
                    {
                        if (inventorySlots[i].Item == null && inventorySlots[k].Item != null) //is the first object empty?
                        {
                            SwapItemPositions(i, k);
                        }
                        else if (inventorySlots[i].Item != null && inventorySlots[k].Item != null)
                        {
                            if (inventorySlots[i].Item.Faction < inventorySlots[k].Item.Faction) //Sorts by Faction
                            {
                                SwapItemPositions(i, k);
                            }
                            else if (inventorySlots[i].Item.Faction == inventorySlots[k].Item.Faction)
                            {
                                if (inventorySlots[i].ItemInfo.ItemType > inventorySlots[k].ItemInfo.ItemType) //Sorty by ItemType
                                {
                                    SwapItemPositions(i, k);
                                }
                                else if (inventorySlots[i].ItemInfo.ItemType == inventorySlots[k].ItemInfo.ItemType)
                                {
                                    if (inventorySlots[i].ItemInfo.ItemRarity > inventorySlots[k].ItemInfo.ItemRarity) //Sorty by Rarity
                                    {
                                        SwapItemPositions(i, k);
                                    }
                                    else if (inventorySlots[i].ItemInfo.ItemRarity == inventorySlots[k].ItemInfo.ItemRarity)
                                    {
                                        if (inventorySlots[i].ItemInfo.ItemName[0] > inventorySlots[k].ItemInfo.ItemName[0]) //Sorty by first Letter of Item
                                        {
                                            SwapItemPositions(i, k);
                                        }
                                        else if (inventorySlots[i].ItemInfo.ItemName == inventorySlots[k].ItemInfo.ItemName)
                                        {
                                            if (inventorySlots[i].Count > inventorySlots[k].Count)
                                            {
                                                SwapItemPositions(i, k);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //catch (Exception e)
                    //{
                    //    Debug.Log("Problem when comparing Items " + e.Message);
                    //}
                }
            }
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].DisplayData = UpdateDisplayData(inventorySlots[i]);
                if(inventorySlots[i].DisplayData)
                    SortDisplayPanel(i);
            }
            inventoryChanged.Invoke();
        }

        /// <summary>
        /// Swaps the Position of the Items at firstindex and secondIndex.
        /// </summary>
        void SwapItemPositions(int firstIndex, int SecondIndex)
        {
            /* InventorySlot buffer = new InventorySlot();
             buffer.Item = inventorySlots[firstIndex].Item;
             buffer.Count = inventorySlots[firstIndex].Count;*/

            InventorySlot buffer = inventorySlots[firstIndex];
            inventorySlots[firstIndex] = inventorySlots[SecondIndex];
            inventorySlots[SecondIndex] = buffer;
        }

        #endregion

        #region Public Functions ##################################################################################

        /// <summary>
        /// Primarily for Debugging. Outputs the Inventory in the Console.
        /// </summary>
        public void CallInventory()
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].Item != null)
                    Debug.Log(inventorySlots[i].ItemInfo.ItemName);
                else
                    Debug.Log("Empty");
            }
        }

        /// <summary>
        /// Adds one or multiple items to the inventory
        /// </summary>
        /// <param name="item">the item that will be added</param>
        /// <param name="count">the amount of items to add</param>
        /// <param name="sortAfterMethod">Set this to true, if Inventory should be sorted after adding the items</param>
        /// <returns>Returns the number of items that couldn't be stored in the inventory. returns 0 if all items were added successfully</returns>
        public int Add(Item item, int count, bool sortAfterMethod)
        {
            for (int i = 0; i < maxSlotCount; i++)
            {
                if (inventorySlots[i].Count <= 0)
                {
                    if (count > item.ItemInfo.MaxStackCount)
                    {
                        count -= item.ItemInfo.MaxStackCount;
                        inventorySlots[i].Count = item.ItemInfo.MaxStackCount;
                        inventorySlots[i].Item = item;
                        InventorySlots[i].ItemInfo = item.ItemInfo;
                    }
                    else
                    {
                        inventorySlots[i].Count += count;
                        inventorySlots[i].Item = item;
                        InventorySlots[i].ItemInfo = item.ItemInfo;
                        count = 0;
                    }
                }
                else if (inventorySlots[i].ItemInfo.ItemName != null)
                {
                    if (inventorySlots[i].ItemInfo.ItemName == item.ItemInfo.ItemName && inventorySlots[i].Count < item.ItemInfo.MaxStackCount)
                    {
                        if (inventorySlots[i].Count + count > item.ItemInfo.MaxStackCount)
                        {
                            count -= (item.ItemInfo.MaxStackCount - inventorySlots[i].Count);
                            inventorySlots[i].Count = item.ItemInfo.MaxStackCount;
                        }
                        else
                        {
                            inventorySlots[i].Count += count;
                            count = 0;
                        }
                    }
                }

                if (count <= 0)
                {
                    if (sortAfterMethod)
                    {
                        SortInventory();
                    }

                    return count;
                }
            }
            if (sortAfterMethod)
            {
                SortInventory();
            }
            return count;
        }

        /// <summary>
        /// Adds one or multiple items to the inventory
        /// </summary>
        /// <param name="item">the item that will be added</param>
        /// <param name="count">the amount of items to add</param>
        /// <param name="sortAfterMethod">Set this to true, if Inventory should be sorted after adding the items</param>
        /// <returns>Returns the number of items that couldn't be stored in the inventory. returns 0 if all items were added successfully</returns>
        public int Add(ItemInfo itemInfo, int count, bool sortAfterMethod)
        {
            for (int i = 0; i < maxSlotCount; i++)
            {
                if (inventorySlots[i].Count <= 0)
                {
                    if (count > itemInfo.MaxStackCount)
                    {
                        count -= itemInfo.MaxStackCount;
                        inventorySlots[i].Count = itemInfo.MaxStackCount;
                        InventorySlots[i].ItemInfo = itemInfo;
                    }
                    else
                    {
                        inventorySlots[i].Count += count;
                        InventorySlots[i].ItemInfo = itemInfo;
                        count = 0;
                    }
                }
                else
                {
                    if (inventorySlots[i].ItemInfo.ItemName == itemInfo.ItemName && inventorySlots[i].Count < itemInfo.MaxStackCount)
                    {
                        if (inventorySlots[i].Count + count > itemInfo.MaxStackCount)
                        {
                            count -= (itemInfo.MaxStackCount - inventorySlots[i].Count);
                            inventorySlots[i].Count = itemInfo.MaxStackCount;
                        }
                        else
                        {
                            inventorySlots[i].Count += count;
                            count = 0;
                        }
                    }
                }

                if (count <= 0)
                {
                    if (sortAfterMethod)
                    {
                        SortInventory();
                    }

                    return count;
                }
            }
            if (sortAfterMethod)
            {
                SortInventory();
            }
            return count;
        }

        /// <summary>
        /// Returns how many of a specific Item is within the inventory. Returns 0 if you don't have the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetNumberOfItems(Item item)
        {
            int amount = 0;
            if (item == null || inventorySlots == null)
                return 0;

            for (int i = 0; i < maxSlotCount; i++)
            {
                if (i < inventorySlots.Length && inventorySlots[i].Item)
                {
                    if (inventorySlots[i].ItemInfo.ItemName == item.ItemInfo.ItemName)
                    {
                        amount += inventorySlots[i].Count;
                    }
                }
            }

            return amount;
        }

        /// <summary>
        /// Removes all items of a specific item. returns the number of items that were in the inventory
        /// </summary>
        /// <param name="item">the item to be removed</param>
        /// <param name="sortAfterMethod">set on true if inventory should be sorted after removing the items</param>
        /// <returns>returns the number of items that were removed</returns>
        public int Remove(Item item, bool sortAfterMethod)
        {
            int count = 0;
            for(int i = 0; i < InventorySlots.Length; i++)
            {
                if (InventorySlots[i].Item != null)
                if (InventorySlots[i].Item.ItemInfo.ItemName == item.ItemInfo.ItemName)
                {
                    count += InventorySlots[i].Count;
                    InventorySlots[i].Count = 0;
                    InventorySlots[i].Item = null;
                    InventorySlots[i].ItemInfo = new ItemInfo();
                }
            }
            if (sortAfterMethod)
            {
                SortInventory();
            }
            return count;
        }

        /// <summary>
        /// Removes any Item from a specific Slot
        /// </summary>
        /// <param name="slot">the slot from which items will be removed</param>
        /// <param name="count">the number of items to be removed. Removes all items if negative</param>
        /// <param name="sortAfterMethod">Set this to true, if Inventory should be sorted after removing the items</param>
        /// <returns>Returns the item that was stored at the slot</returns>
        public Item Remove(int slot, int count, bool sortAfterMethod)
        {
            Item item = null;
            if (count > 0)
            {
                inventorySlots[slot].Count -= count;
                if (inventorySlots[slot].Count <= 0)
                {
                    item = inventorySlots[slot].Item;
                    inventorySlots[slot].Count = 0;
                    inventorySlots[slot].Item = null;
                    InventorySlots[slot].ItemInfo = new ItemInfo();
                }
            }
            else if (count < 0)
            {
                item = inventorySlots[slot].Item;
                inventorySlots[slot].Count = 0;
                inventorySlots[slot].Item = null;
                InventorySlots[slot].ItemInfo = new ItemInfo();
            }

            if (sortAfterMethod)
            {
                SortInventory();
            }

            return item;
        }

        /// <summary>
        /// Removes a specific Item from the inventory. doesn't remove the items if count is lower than amount of items
        /// </summary>
        /// <param name="item">the item which will be removed</param>
        /// <param name="count">how many of that item should be removed</param>
        /// <param name="sortAfterMethod">Set this to true, if Inventory should be sorted after removing the items</param>
        /// <returns>returns true if there are more items in the inventory, than count.</returns>
        public bool Remove(Item item, int count, bool sortAfterMethod)
        {
            //Searching for amount of Items
            try
            {
                int foundItems = 0;
                for (int i = 0; i < maxSlotCount; i++)
                {
                    if (inventorySlots[i].Item)
                    {
                        if (inventorySlots[i].ItemInfo.ItemName == item.ItemInfo.ItemName)
                        {
                            foundItems += inventorySlots[i].Count;
                        }
                    }
                }
                if (foundItems < count)
                {
                    return false;
                }
            }
            catch
            {
                throw new Exception("Error during counting of items");
            }

            //Removing Items
            try
            {
                for (int i = 0; i < maxSlotCount; i++)
                {
                    if (inventorySlots[i].Item)
                    {
                        if (inventorySlots[i].ItemInfo.ItemName == item.ItemInfo.ItemName)
                        {
                            inventorySlots[i].Count -= count;
                            if (inventorySlots[i].Count <= 0)
                            {
                                inventorySlots[i].Item = null;
                                InventorySlots[i].ItemInfo = new ItemInfo();
                                count = -inventorySlots[i].Count;
                                inventorySlots[i].Count = 0;
                            }
                            else
                            {
                                if (sortAfterMethod)
                                {
                                    SortInventory();
                                }

                                return true;
                            }
                        }
                    }
                }

                if (sortAfterMethod)
                {
                    SortInventory();
                }

                return true;

            }
            catch
            {
                throw new Exception("Error during removing of items from inventory");
            }
        }

        /// <summary>
        /// Removes a specific Item from the inventory. doesn't remove the items if count is lower than amount of items
        /// </summary>
        /// <param name="item">the item which will be removed</param>
        /// <param name="count">how many of that item should be removed</param>
        /// <param name="sortAfterMethod">Set this to true, if Inventory should be sorted after removing the items</param>
        /// <returns>returns true if there are more items in the inventory, than count.</returns>
        public bool Remove(InventoryItem item, int count, bool sortAfterMethod)
        {
            //Searching for amount of Items
            try
            {
                int foundItems = 0;
                for (int i = 0; i < maxSlotCount; i++)
                {
                    if (inventorySlots[i].ItemInfo.ItemName != null)
                    {
                        if (inventorySlots[i].ItemInfo.ItemName == item.ItemName.text)
                        {
                            foundItems += inventorySlots[i].Count;
                        }
                    }
                }
                if (foundItems < count)
                {
                    return false;
                }
            }
            catch
            {
                throw new Exception("Error during counting of items");
            }

            //Removing Items
            try
            {
                for (int i = 0; i < maxSlotCount; i++)
                {
                    if (inventorySlots[i].ItemInfo.ItemName != null)
                    {
                        if (inventorySlots[i].ItemInfo.ItemName == item.ItemName.text)
                        {
                            inventorySlots[i].Count -= count;
                            if (inventorySlots[i].Count <= 0)
                            {
                                inventorySlots[i].Item = null;
                                count = -inventorySlots[i].Count;
                                inventorySlots[i].Count = 0;
                                InventorySlots[i].ItemInfo = new ItemInfo();
                            }
                            else
                            {
                                if (sortAfterMethod)
                                {
                                    SortInventory();
                                }

                                return true;
                            }
                        }
                    }
                }

                if (sortAfterMethod)
                {
                    SortInventory();
                }

                return true;

            }
            catch
            {
                throw new Exception("Error during removing of items from inventory");
            }
        }

        /// <summary>
        /// Interacts with Item within a specific Inventoryslot
        /// </summary
        public void Interact(int slot, GameObject caller, bool sortAfterMethod)
        {
            if (inventorySlots[slot].Count > 0 && slot >= 0 && slot < maxSlotCount)
            {
                inventorySlots[slot].Item.Interact(caller);
            }
        }

        /// <summary>
        /// Finds Gold in inventory and removes the passed amount.
        /// </summary>
        /// <param name="cost">
        /// How much Gold should be removed.
        /// </param>
        /// <returns>
        /// Returns true if 'cost' was removed, otherwise returns false.
        /// </returns>
        public bool RemoveGold(int cost)
        {
            int goldslot = FindMoneySlot();

            if (goldslot != -1)
            {
                if (InventorySlots[goldslot].Count >= cost)
                {
                    Remove(goldslot, cost, true);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds a specific Item within the inventory and interacts with it
        /// </summary>
        /// <returns>returns false if item is not in the inventory</returns>
        public bool Interact(Item item, GameObject caller)
        {
            int slot = FindItem(item);
            if (slot >= 0)
            {
                inventorySlots[slot].Item.Interact(caller);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}