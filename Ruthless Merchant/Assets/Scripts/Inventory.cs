using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Inventory
    {
        private Item[] items;
        private InventorySlot[] inventorySlots;
        private int maxSlotCount;

        public Inventory()
        {
            inventorySlots = new InventorySlot[maxSlotCount];
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

        public int Add(Item item, int count)
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
                }
                else if(inventorySlots[i].Item == item && inventorySlots[i].Count < item.MaxStackCount)
                {
                    if (inventorySlots[i].Count + count > item.MaxStackCount)
                    {
                        count -= (item.MaxStackCount - inventorySlots[i].Count);
                        inventorySlots[i].Count = item.MaxStackCount;
                        inventorySlots[i].Item = item;
                    }
                }
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
        /// Removes any Item from a specific Slot
        /// </summary>
        /// <param name="slot">the slot from which items will be removed</param>
        /// <param name="count">the number of items to be removed. Removes all items if negative</param>
        public void Remove(int slot, int count)
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
        }

        /// <summary>
        /// Removes a specific Item from the inventory. doesn't remove the items if count is lower than amount of items
        /// </summary>
        /// <param name="item">the item which will be removed</param>
        /// <param name="count">how many of that item should be removed</param>
        /// <returns>returns true if there are more items in the inventory, than count.</returns>
        public bool Remove(Item item, int count)
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
                            return true;
                        }
                    }
                }
                return true;

            }
            catch
            {
                throw new Exception("Error during removing of items from inventory");
            }
        }
    }
}