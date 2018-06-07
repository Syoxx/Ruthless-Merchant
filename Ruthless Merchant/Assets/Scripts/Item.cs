using UnityEngine;

namespace RuthlessMerchant
{
    public class Item : InteractiveWorldObject
    {
        private int ownerId;
        public ItemType type;
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

        }

        public override void Update()
        {
        }

        public override void Interact(GameObject caller)
        {
            throw new System.NotImplementedException();
        }

        public void Pickup(out Item targetItem)
        {
            targetItem = this;
            Debug.Log("item pickup initiated");
        }
    }
}