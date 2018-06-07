using System;

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

        /// <summary>
        /// Adds one or multiple items to the inventory
        /// </summary>
        /// <param name="item">the item that will be added</param>
        /// <param name="count">the amount of items to add</param>
        /// <returns>Returns the number of items that couldn't be stored in the inventory. returns 0 if all items were added successfully</returns>
        public int Add(Item item, int count)
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
        /// Removes any Item from a specific Slot
        /// </summary>
        /// <param name="slot">the slot from which items will be removed</param>
        /// <param name="count">the number of items to be removed. Removes all items if negative</param>
        /// <returns>Returns the item that was stored at the slot</returns>
        public Item Remove(int slot, int count)
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
            return inventorySlots[slot].Item;
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

        /// <summary>
        /// Interacts with Item within a specific Inventoryslot
        /// </summary
        public void Interact(int slot)
        {
            if(inventorySlots[slot].Count > 0 && slot >= 0 && slot < maxSlotCount)
            {
                inventorySlots[slot].Item.Interact();
            }
        }

        /// <summary>
        /// Finds a specific Item within the inventory and interacts with it
        /// </summary>
        /// <returns>returns false if item is not in the inventory</returns>
        public bool Interact(Item item)
        {
            int slot = FindItem(item);
            if(slot >= 0)
            {
                inventorySlots[slot].Item.Interact();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}