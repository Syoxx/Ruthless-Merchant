using UnityEngine;

namespace RuthlessMerchant
{
    public class Item : InteractiveWorldObject
    {
        private int ownerId;
        private ItemType type;
        private ItemValue[] itemValue;
        private int weight;
        private int MaxStackCount;

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

        public override void Start()
        {
            //throw new System.NotImplementedException();
        }

        public override void Update()
        {
            //throw new System.NotImplementedException();
        }

        public override void Interact(GameObject caller)
        {
            throw new System.NotImplementedException();
        }

        public void Pickup()
        {
            throw new System.NotImplementedException();
        }
    }
}