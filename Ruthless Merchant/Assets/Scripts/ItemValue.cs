namespace RuthlessMerchant
{
    public struct ItemValue
    {
        public int Count;
        private Item item;

        public Item Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }
    }
}