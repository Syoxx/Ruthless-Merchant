using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RuthlessMerchant
{
    public class Inventory : MonoBehaviour
    {
        #region Fields #############################################################################################

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


        #region Properties #########################################################################################

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

        #endregion


        #region Structs ############################################################################################



        #endregion


        #region Private Functions ##################################################################################

        private void Awake()
        {
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
                    if (inventorySlots[i].Item.ItemName == item.ItemName)
                        return i;
                }
            }
            return -1;
        }

        private void SortDisplayPanel(int inventorySlot)
        {
            int pageForItem = inventorySlot / Player.Singleton.MaxItemsPerPage;
            inventorySlots[inventorySlot].DisplayData.transform.parent = BookLogic.InventoryPageList[pageForItem].transform.GetChild(0);
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
            GameObject inventoryItem = Instantiate(ItemUIPrefab, parent) as GameObject;
            //inventoryItem.transform.SetParent(parent, false);

            InventoryItem itemInfos = inventoryItem.GetComponent<InventoryItem>();

            itemInfos.itemName.text = inventorySlot.Count + "x " + inventorySlot.Item.ItemName + " (" + inventorySlot.Item.ItemRarity + ")";
            itemInfos.itemDescription.text = inventorySlot.Item.ItemLore;
            if (inventorySlot.Item.ItemValue != null)
                if (inventorySlot.Item.ItemValue.Length > 0)
                    itemInfos.itemPrice.text = inventorySlot.Item.ItemValue[0].Count.ToString();
                else
                    itemInfos.itemPrice.text = "0G";

            if (inventorySlot.Item.ItemSprite != null)
            {
                itemInfos.ItemImage.sprite = inventorySlot.Item.ItemSprite;
            }

            return itemInfos;
        }

        /// <summary>
        /// Updates the Inventory-Display of a specific Inventoryslot
        /// </summary>
        /// <param name="inventorySlot">inventoryslot the function will update</param>
        /// <returns>returns the created display</returns>
        private InventoryItem UpdateDisplayData(InventorySlot inventorySlot)
        {
            if (ItemUIPrefab == null)
                throw new System.NullReferenceException("no ItemUIPrefab found");

            InventoryItem itemInfos = inventorySlot.DisplayData;
            if (inventorySlot.Item == null && itemInfos != null)
            {
                Destroy(inventorySlot.DisplayData.gameObject);
                inventorySlot.DisplayData = null;
                return null;
            }
            if (inventorySlot.Item == null && itemInfos == null)
            {
                return null;
            }
            if (itemInfos == null)
            {
                return CreateDisplayData(inventorySlot);
            }

            itemInfos.itemName.text = inventorySlot.Count + "x " + inventorySlot.Item.ItemName + " (" + inventorySlot.Item.ItemRarity + ")";
            itemInfos.itemDescription.text = inventorySlot.Item.ItemLore;
            if (inventorySlot.Item.ItemValue != null)
                if (inventorySlot.Item.ItemValue.Length > 0)
                    itemInfos.itemPrice.text = inventorySlot.Item.ItemValue[0].Count + "G";
                else
                    itemInfos.itemPrice.text = "0G";

            if (inventorySlot.Item.ItemSprite != null)
            {
                itemInfos.ItemImage.sprite = inventorySlot.Item.ItemSprite;
            }

            return itemInfos;
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
                    try
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
                                if (inventorySlots[i].Item.ItemType > inventorySlots[k].Item.ItemType) //Sorty by ItemType
                                {
                                    SwapItemPositions(i, k);
                                }
                                else if (inventorySlots[i].Item.ItemType == inventorySlots[k].Item.ItemType)
                                {
                                    if (inventorySlots[i].Item.ItemRarity > inventorySlots[k].Item.ItemRarity) //Sorty by Rarity
                                    {
                                        SwapItemPositions(i, k);
                                    }
                                    else if (inventorySlots[i].Item.ItemRarity == inventorySlots[k].Item.ItemRarity)
                                    {
                                        if (inventorySlots[i].Item.ItemName[0] > inventorySlots[k].Item.ItemName[0]) //Sorty by first Letter of Item
                                        {
                                            SwapItemPositions(i, k);
                                        }
                                        else if (inventorySlots[i].Item.ItemName == inventorySlots[k].Item.ItemName)
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
                    catch
                    {
                        Debug.Log("Problem when comparing Items");
                    }
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
                    Debug.Log(inventorySlots[i].Item.gameObject.name);
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
                    if (count > item.MaxStackCount)
                    {
                        count -= item.MaxStackCount;
                        inventorySlots[i].Count = item.MaxStackCount;
                        inventorySlots[i].Item = item;
                    }
                    else
                    {
                        inventorySlots[i].Count += count;
                        inventorySlots[i].Item = item;
                        count = 0;
                    }
                }
                else if (inventorySlots[i].Item)
                {
                    if (inventorySlots[i].Item.ItemName == item.ItemName && inventorySlots[i].Count < item.MaxStackCount)
                    {
                        if (inventorySlots[i].Count + count > item.MaxStackCount)
                        {
                            count -= (item.MaxStackCount - inventorySlots[i].Count);
                            inventorySlots[i].Count = item.MaxStackCount;
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
                    if (inventorySlots[i].Item.ItemName == item.ItemName)
                    {
                        amount += inventorySlots[i].Count;
                    }
                }
            }

            return amount;
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
                }
            }
            else if (count < 0)
            {
                item = inventorySlots[slot].Item;
                inventorySlots[slot].Count = 0;
                inventorySlots[slot].Item = null;
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
                        if (inventorySlots[i].Item.ItemName == item.ItemName)
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
                        if (inventorySlots[i].Item.ItemName == item.ItemName)
                        {
                            inventorySlots[i].Count -= count;
                            if (inventorySlots[i].Count <= 0)
                            {
                                inventorySlots[i].Item = null;
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