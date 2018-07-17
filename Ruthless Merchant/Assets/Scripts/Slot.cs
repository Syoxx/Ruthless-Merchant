namespace RuthlessMerchant
{
    [System.Serializable]
    public class Slot
    {
        public enum SlotTypes
        {
            Inventory,
            External
        }

        public SlotTypes SlotType;

        public int Count;
        public int MaxStackCount;
        public Item Item;
        public SlotDisplay DisplayData;
    }
}