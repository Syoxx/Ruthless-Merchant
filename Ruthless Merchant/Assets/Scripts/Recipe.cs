using UnityEngine;

namespace RuthlessMerchant
{
    public class Recipe : Item
    {
        public ItemValue ItemResult;
        public ItemValue Ingridients;
        public int Duration;
        private float elapsedTime;

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Craft()
        {
            throw new System.NotImplementedException();
        }

        public override void Interact(GameObject caller)
        {
            throw new System.NotImplementedException();
        }
    }
}