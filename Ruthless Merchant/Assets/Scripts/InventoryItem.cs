using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RuthlessMerchant
{
    /// <summary>
    /// This script permits setting information of an inventory UI object
    /// </summary>
    public class InventoryItem : MonoBehaviour
    {
        public delegate void MoveItemEventHandler(InventoryItem item);
        public static event MoveItemEventHandler MoveItem;

        public enum ItemBehaviour
        {
            None,
            Move
        }

        public enum UILocation
        {
            Inventory,
            ExternList
        }

        public UILocation Location;

        public static ItemBehaviour Behaviour;

        public Image ItemImage;

        public TextMeshProUGUI ItemQuantity;

        public TextMeshProUGUI ItemName;

        public TextMeshProUGUI ItemRarity;

        public TextMeshProUGUI ItemDescription;

        public TextMeshProUGUI ItemPrice;

        public TextMeshProUGUI ItemWeight;

        public Button ItemButton;

        public InventorySlot Slot;

        public void OnButtonClick()
        {
            Debug.Log("Inventory Item Button Pressed!");

            switch (Behaviour)
            {
                case ItemBehaviour.Move:

                    if (Location == UILocation.Inventory)
                    {
                        MoveItem(this);
                        Inventory.Singleton.Remove(this, 1, true);
                    }
                    else
                    {
                        TradeItemSelection.Singleton.RemoveItemFromSellingList(this);
                    }

                    break;
            }
        }
    }
}
