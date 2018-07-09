// Authors: Daniil Masliy, Alberto Lladó

using UnityEngine;
using UnityEngine.UI;
using UnityScript.Steps;

namespace RuthlessMerchant
{
    [RequireComponent(typeof(UniqueIDGenerator))]
    public abstract class Item : InteractiveWorldObject
    {
        public string ItemName;

        public ItemValue[] ItemValue;

        public int ItemWeight;

        [TextArea]
        public string ItemLore;

        public Sprite ItemSprite;

        public ItemType ItemType;

        public ItemRarity ItemRarity;

        /// <summary>
        /// Creates deep copy of current item
        /// </summary>
        /// <returns></returns>
        internal Item DeepCopy()
        {
            Item otherItem = (Item)this.MemberwiseClone();
            return otherItem;
        }

        [SerializeField]
        public int MaxStackCount { get; private set; }

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