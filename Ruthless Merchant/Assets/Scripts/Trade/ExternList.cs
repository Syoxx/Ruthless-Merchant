using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class ExternList : MonoBehaviour
    {
        public static ExternList Singleton;

        public List<Slot> Slots;

        [SerializeField]
        Transform sellingItemParent;

        [SerializeField]
        GameObject sellingItemPrefab;

        [SerializeField]
        Text price;

        void Awake()
        {
            Singleton = this;
            Slots = new List<Slot>();
        }

        private void Start()
        {
            Player.Singleton.OpenBook(KeyCode.I);

            for (int x = 0; x < Inventory.Singleton.inventorySlots.Length; x++)
            {
                SlotDisplay display = Inventory.Singleton.inventorySlots[x].DisplayData;
            }
        }

        public void AddItemToSellingList(SlotDisplay inventorySlotDisplay)
        {
            if (!AddToPresent(inventorySlotDisplay))
            {
                GameObject newObject = Instantiate(sellingItemPrefab, sellingItemParent);
                newObject.name = "SellingItem";

                SlotDisplay newItem = newObject.GetComponent<SlotDisplay>();
                newItem.ItemQuantity.text = "1x";
                newItem.ItemName.text = inventorySlotDisplay.ItemName.text;
                newItem.ItemPrice.text = inventorySlotDisplay.ItemPrice.text;
                newItem.ItemDescription.text = inventorySlotDisplay.ItemDescription.text;
                newItem.ItemRarity.text = inventorySlotDisplay.ItemRarity.text;
                newItem.ItemSprite.sprite = inventorySlotDisplay.ItemSprite.sprite;

                Slot newSlot = new Slot();
                newSlot.Count = 1;
                newSlot.Item = inventorySlotDisplay.Slot.Item;
                newSlot.SlotType = Slot.SlotTypes.External;
                newSlot.DisplayData = newItem;
                newSlot.MaxStackCount = inventorySlotDisplay.Slot.MaxStackCount;

                newItem.Slot = newSlot;

                Slots.Add(newSlot);
            }

            price.text = (int.Parse(price.text) + int.Parse(inventorySlotDisplay.ItemPrice.text.Replace("G",""))).ToString();

            Debug.Log(Inventory.Singleton.Remove(inventorySlotDisplay.Slot.Item, 1, false));
        }

        bool AddToPresent(SlotDisplay data)
        {
            for(int x = 0; x < Slots.Count; x++)
            {
                if (Slots[x].Item.ItemName == data.ItemName.text && Slots[x].DisplayData.ItemDescription.text == data.ItemDescription.text)
                {
                    Slots[x].Count += 1;
                    Slots[x].DisplayData.ItemQuantity.text = Slots[x].Count.ToString() + "x";
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemToSellingList(SlotDisplay slotDisplay)
        {
            Debug.Log(Inventory.Singleton.Add(slotDisplay.Slot, false).ToString());
            slotDisplay.Slot.Count -= 1;

            price.text = (int.Parse(price.text.Replace("G","")) - int.Parse(slotDisplay.ItemPrice.text.Replace("G", ""))).ToString();

            if (slotDisplay.Slot.Count > 0)
            {
                slotDisplay.Slot.DisplayData.ItemQuantity.text = slotDisplay.Slot.Count.ToString() + "x";
            }
            else
            {
                Slots.Remove(slotDisplay.Slot);
                Destroy(slotDisplay.gameObject);
            }
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
