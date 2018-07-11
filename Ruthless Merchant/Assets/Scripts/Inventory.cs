using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RuthlessMerchant
{
    public class Inventory : MonoBehaviour
    {
        private Item[] items;
        public InventorySlot[] inventorySlots;
        [SerializeField]
        private int maxSlotCount = 10;

        public List<Item> startinventory;
        private UnityEvent inventoryChanged;

        public JumpToPaper BookLogic = null;
        public GameObject ItemUIPrefab = null;

        public UnityEvent InventoryChanged
        {
            get
            {
                return inventoryChanged;
            }
        }

        private void Awake()
        {
            inventorySlots = new InventorySlot[maxSlotCount];

            inventoryChanged = new UnityEvent();
        }

        private void Start()
        {
            //Debugging
            foreach(Item item in startinventory)
            {
                Add(item, 1, true);
            }
        }

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

        public Inventory()
        {
            inventorySlots = new InventorySlot[maxSlotCount];

            inventoryChanged = new UnityEvent();
        }

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
                    if (inventorySlots[i].Item.itemName == item.itemName && inventorySlots[i].Count < item.MaxStackCount)
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
                    break;
                }
            }
            if (sortAfterMethod)
            {
                SortInventory();
            }
            return count;
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
                    if (inventorySlots[i].Item.itemName == item.itemName)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns how many of a specific Item is within the inventory. Returns 0 if you don't have the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetNumberOfItems(Item item)
        {
            int amount = 0;

            for(int i = 0; i < maxSlotCount; i++)
            {
                if(inventorySlots[i].Item)
                {
                    if (inventorySlots[i].Item.itemName == item.itemName)
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
            if(count > 0)
            {
                inventorySlots[slot].Count -= count;
                if (inventorySlots[slot].Count <= 0)
                {
                    item = inventorySlots[slot].Item;
                    inventorySlots[slot].Count = 0;
                    inventorySlots[slot].Item = null;
                }
            }
            else if(count < 0)
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
                    if(inventorySlots[i].Item)
                    {
                        if (inventorySlots[i].Item.itemName == item.itemName)
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
                        if (inventorySlots[i].Item.itemName == item.itemName)
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

        private InventoryDisplayedData CreateDisplayData(InventorySlot inventorySlot)
        {
            if (ItemUIPrefab == null)
                return null;

            Transform parent = BookLogic.InventoryPageList[BookLogic.PageForCurrentWeaponPlacement()].transform.Find("PNL_ZoneForItem");
            GameObject inventoryItem = Instantiate(ItemUIPrefab, parent) as GameObject;
            //inventoryItem.transform.SetParent(parent, false);

            InventoryDisplayedData itemInfos = inventoryItem.GetComponent<InventoryDisplayedData>();
            itemInfos.itemName.text = inventorySlot.Count + "x " + inventorySlot.Item.itemName + " (" + inventorySlot.Item.itemRarity + ")";
            itemInfos.itemDescription.text = inventorySlot.Item.itemLore;
            itemInfos.itemPrice.text = inventorySlot.Item.itemPrice + "G";

            if (inventorySlot.Item.itemSprite != null)
            {
                itemInfos.ItemImage.sprite = inventorySlot.Item.itemSprite;
            }

            return itemInfos;
        }

        private InventoryDisplayedData UpdateDisplayData(InventorySlot inventorySlot)
        {
            if (ItemUIPrefab == null)
                return null;

            InventoryDisplayedData itemInfos = inventorySlot.DisplayData;
            if (itemInfos == null && inventorySlot.Item == null)
            {
                return null;
            }
            else if(itemInfos != null && inventorySlot.Item == null)
            {
                Destroy(itemInfos.gameObject);
                inventorySlot.DisplayData = null;
                return null;
            }
            else if (itemInfos == null && inventorySlot.Item != null)
                return CreateDisplayData(inventorySlot);

            itemInfos.itemName.text = inventorySlot.Count + "x " + inventorySlot.Item.itemName + " (" + inventorySlot.Item.itemRarity + ")";
            itemInfos.itemDescription.text = inventorySlot.Item.itemLore;
            itemInfos.itemPrice.text = inventorySlot.Item.itemPrice + "G";

            if (inventorySlot.Item.itemSprite != null)
            {
                itemInfos.ItemImage.sprite = inventorySlot.Item.itemSprite;
            }

            return itemInfos;
        }

        void UpdateDisplay()
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].DisplayData = UpdateDisplayData(inventorySlots[i]);
            }
        }

        /// <summary>
        /// Interacts with Item within a specific Inventoryslot
        /// </summary
        public void Interact(int slot, GameObject caller, bool sortAfterMethod)
        {
            if(inventorySlots[slot].Count > 0 && slot >= 0 && slot < maxSlotCount)
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
            if(slot >= 0)
            {
                inventorySlots[slot].Item.Interact(caller);
                return true;
            }
            else
            {
                return false;
            }
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
                                if (inventorySlots[i].Item.itemType > inventorySlots[k].Item.itemType) //Sorty by ItemType
                                {
                                    SwapItemPositions(i, k);
                                }
                                else if (inventorySlots[i].Item.itemType == inventorySlots[k].Item.itemType)
                                {
                                    if (inventorySlots[i].Item.itemRarity > inventorySlots[k].Item.itemRarity) //Sorty by Rarity
                                    {
                                        SwapItemPositions(i, k);
                                    }
                                    else if (inventorySlots[i].Item.itemRarity == inventorySlots[k].Item.itemRarity)
                                    {
                                        if (inventorySlots[i].Item.itemName[0] > inventorySlots[k].Item.itemName[0]) //Sorty by first Letter of Item
                                        {
                                            SwapItemPositions(i, k);
                                        }
                                        else if (inventorySlots[i].Item.itemName == inventorySlots[k].Item.itemName)
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
            UpdateDisplay();
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
    }
}