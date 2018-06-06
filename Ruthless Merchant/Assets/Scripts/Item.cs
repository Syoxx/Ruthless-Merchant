namespace RuthlessMerchant
{
    public class Item : InteractiveWorldObject
    {
        private int ownerId;
        private ItemType type;
        private ItemValue[] itemValue;
        private int weight;
        private int maxStackCount;

        public ItemType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public ItemValue[] ItemValue
        {
            get
            {
                return itemValue;
            }
            set
            {
                itemValue = value;
            }
        }

        public int MaxStackCount
        {
            get
            {
                return maxStackCount;
            }
        }

        public override void Interact()
        {
            throw new System.NotImplementedException();
        }

        public void Pickup()
        {
            throw new System.NotImplementedException();
        }
    }
}