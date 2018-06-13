using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Item : InteractiveWorldObject
    {
        private int ownerId;
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private ItemType type;
        [SerializeField] private ItemRarity rarity;
        [SerializeField] private int price;
        [SerializeField] private float weight;
        [SerializeField] private string itemLore = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
        private string itemName = "object name";
        private int maxStackCount = 1;
        private ItemValue[] itemValue;

        public float ItemWeight
        {
            get { return weight; }
        }

        public int Price
        {
            get { return price; }
        }

        public Sprite ItemSprite
        {
            get { return itemSprite; }
        }

        public string Name
        {
            get { return itemName; }
        }

        public string Description
        {
            get { return itemLore; }
        }

        public ItemRarity Rarity
        {
            get { return rarity; }
        }

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
<<<<<<< HEAD
            //throw new System.NotImplementedException();
=======
            itemName = this.name;
>>>>>>> 1b80879c31900804716bfbc5ecabe760b947ea67
        }

        public override void Update()
        {
<<<<<<< HEAD
            //throw new System.NotImplementedException();
=======
>>>>>>> 1b80879c31900804716bfbc5ecabe760b947ea67
        }

        public override void Interact(GameObject caller)
        {
            throw new System.NotImplementedException();
        }

        public void Pickup()
        {
            throw new System.NotImplementedException();
        }

        public int MaxStackCount
        {
            get
            {
                return maxStackCount;
            }
        }

        // Creates deep copy of current item
        internal Item DeepCopy()
        {
            Item otherItem = (Item) this.MemberwiseClone();
            return otherItem;
        }
    }
}