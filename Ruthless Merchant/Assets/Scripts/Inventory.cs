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

        UnityEvent InventoryChanged;

        private void Awake()
        {
            inventorySlots = new InventorySlot[maxSlotCount];

            InventoryChanged = new UnityEvent();
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
            //inventorySlots = new InventorySlot[maxSlotCount];
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
            try
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
                    else if (inventorySlots[i].Item == item && inventorySlots[i].Count < item.MaxStackCount)
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
                    if(count <= 0)
                    {
                        if (sortAfterMethod)
                        {
                            SortInventory();
                        }
                        else
                        {
                            InventoryChanged.Invoke();
                        }
                        return count;
                    }
                }
                if(sortAfterMethod)
                {
                    SortInventory();
                }
                else
                {
                    InventoryChanged.Invoke();
                }
                return count;
            }
            catch
            {
                throw new Exception("Error when trying to Add Item to Inventory");
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
                if (inventorySlots[i].Item == item)
                    return i;
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
                if(inventorySlots[i].Item == item)
                {
                    amount += inventorySlots[i].Count;
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
            if(count > 0)
            {
                inventorySlots[slot].Count -= count;
                if (inventorySlots[slot].Count <= 0)
                {
                    inventorySlots[slot].Count = 0;
                    inventorySlots[slot].Item = null;
                }
            }
            else if(count < 0)
            {
                inventorySlots[slot].Count = 0;
                inventorySlots[slot].Item = null;
            }
            if (sortAfterMethod)
            {
                SortInventory();
            }
            else
            {
                InventoryChanged.Invoke();
            }
            return inventorySlots[slot].Item;
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
                    if (inventorySlots[i].Item == item)
                    {
                        foundItems += inventorySlots[i].Count;
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
                    if (inventorySlots[i].Item == item)
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
                            else
                            {
                                InventoryChanged.Invoke();
                            }
                            return true;
                        }
                    }
                }
                if (sortAfterMethod)
                {
                    SortInventory();
                }
                else
                {
                    InventoryChanged.Invoke();
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
                    if (inventorySlots[i].Item == null && inventorySlots[k].Item != null) //is the first object empty?
                    {
                        SwapItemPositions(i, k);
                    }
                    else if(inventorySlots[i].Item != null && inventorySlots[k].Item != null)
                    {
                        if (inventorySlots[i].Item.Faction < inventorySlots[k].Item.Faction) //Sorts by Faction
                        {
                            SwapItemPositions(i, k);
                        }
                        else if (inventorySlots[i].Item.Faction == inventorySlots[k].Item.Faction)
                        {
                            if (inventorySlots[i].Item.Type > inventorySlots[k].Item.Type) //Sorty by ItemType
                            {
                                SwapItemPositions(i, k);
                            }
                            else if (inventorySlots[i].Item.Type == inventorySlots[k].Item.Type)
                            {
                                if (inventorySlots[i].Item.Rarity < inventorySlots[k].Item.Rarity) //Sorty by Rarity
                                {
                                    SwapItemPositions(i, k);
                                }
                                else if (inventorySlots[i].Item.Rarity == inventorySlots[k].Item.Rarity)
                                {
                                    if (inventorySlots[i].Item.gameObject.name[0] > inventorySlots[k].Item.gameObject.name[0]) //Sorty by first Letter of Item
                                    {
                                        SwapItemPositions(i, k);
                                    }
                                    else if(inventorySlots[i].Item == inventorySlots[k].Item)
                                    {
                                        if(inventorySlots[i].Count > inventorySlots[k].Count)
                                        {
                                            SwapItemPositions(i, k);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            InventoryChanged.Invoke();
        }

        /// <summary>
        /// Swaps the Position of the Items at firstindex and secondIndex.
        /// </summary>
        void SwapItemPositions(int firstIndex, int SecondIndex)
        {
            InventorySlot buffer = new InventorySlot();
            buffer.Item = inventorySlots[firstIndex].Item;
            buffer.Count = inventorySlots[firstIndex].Count;

            inventorySlots[firstIndex] = inventorySlots[SecondIndex];
            inventorySlots[SecondIndex] = buffer;
        }

        public void AddTest(int number)
        {
            Add(startinventory[0], number, true);
        }

        public void RemoveTest(int number)
        {
            Remove(startinventory[0], number, true);
        }
    }
}