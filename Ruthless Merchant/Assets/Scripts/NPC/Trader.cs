using UnityEngine;

namespace RuthlessMerchant
{
    public class Trader : Civilian
    {
        public void Buy()
        {
            throw new System.NotImplementedException();
        }

        public void Sell()
        {
            throw new System.NotImplementedException();
        }

        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Trader");
        }
    }
}