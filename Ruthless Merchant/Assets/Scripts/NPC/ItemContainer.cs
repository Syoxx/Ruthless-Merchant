//Author: Marcel Croonenbroeck

namespace RuthlessMerchant
{
    public class ItemContainer
    {
        public int Count;
        public Item Item;

        public ItemContainer(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}
