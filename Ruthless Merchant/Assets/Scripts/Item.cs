using UnityEngine;
using UnityEngine.UI;
using UnityScript.Steps;

namespace RuthlessMerchant
{
    [RequireComponent(typeof(UniqueIDGenerator))]
    public abstract class Item : InteractiveWorldObject
    {
        //Made by Daniil Masliy

        // SerializeField for Child classes
        [Header("Item Parameters")]
        [SerializeField] public int itemPrice;
        [SerializeField] public int itemWeight;
        [SerializeField] public string itemName;
        [TextArea] public string itemLore;
        [SerializeField] public Sprite itemSprite;
        [SerializeField] public ItemType itemType;
        [SerializeField] public ItemRarity itemRarity;



        // Creates deep copy of current item
        internal Item DeepCopy()
        {
            Item otherItem = (Item) this.MemberwiseClone();
            return otherItem;
        }

        //
        //
        // Not used things (Dunno if we need this)
        //
        //
        [SerializeField]
        private int maxStackCount = 1;
        private ItemValue[] itemValue;
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
    }
}