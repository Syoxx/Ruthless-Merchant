// Authors: Daniil Masliy, Alberto Lladó

using UnityEngine;
using UnityEngine.UI;
using UnityScript.Steps;

namespace RuthlessMerchant
{
    [RequireComponent(typeof(UniqueIDGenerator))]
    public abstract class Item : InteractiveWorldObject
    {
        public ItemInfo ItemInfo;

        /// <summary>
        /// Creates deep copy of current item
        /// </summary>
        /// <returns></returns>
        internal Item DeepCopy()
        {
            Item otherItem = (Item)this.MemberwiseClone();
            return otherItem;
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