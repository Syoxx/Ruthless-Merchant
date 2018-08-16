using UnityEngine;

namespace RuthlessMerchant
{
    public struct InventorySlot
    {
        public int Count;
        public Item Item;
        public ItemInfo ItemInfo;
        public InventoryItem DisplayData;
    }

    [System.Serializable]
    public struct ItemInfo
    {
        public string ItemName;

        public ItemValue[] ItemValue;

        public int ItemWeight;

        public string ItemLore;

        public Sprite ItemSprite;

        public ItemType ItemType;

        public ItemRarity ItemRarity;

        [SerializeField]
        int maxStackCount;

        public int MaxStackCount
        {
            get
            {
                if (maxStackCount != 0)
                {
                    return maxStackCount;
                }
                else
                {
                    return MaxStackCountDefault;
                }
            }
            set
            {
                maxStackCount = value;
            }
        }

        public const int MaxStackCountDefault = 20;
    }
}