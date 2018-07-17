using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    /// <summary>
    /// This script permits setting information of an inventory UI object
    /// </summary>
    public class SlotDisplay : MonoBehaviour
    {
        public Slot Slot;

        public Image ItemSprite;

        public TextMeshProUGUI ItemQuantity;
        public TextMeshProUGUI ItemName;
        public TextMeshProUGUI ItemRarity;
        public TextMeshProUGUI ItemDescription;
        public TextMeshProUGUI ItemPrice;
        public TextMeshProUGUI ItemWeight;

        public Button ItemButton;

        public void OnButtonClick()
        {
            if (Slot.SlotType == Slot.SlotTypes.Inventory)
            {
                if (ExternList.Singleton != null)
                {
                    ExternList.Singleton.AddItemToSellingList(this);
                }
            }
            else
            {
                if (ExternList.Singleton != null)
                {
                    ExternList.Singleton.RemoveItemToSellingList(this);
                }
            }
        }
    }
}