using UnityEngine;

namespace RuthlessMerchant
{
    public class Warrior : Fighter
    {
        public override void Start()
        {
            base.Start();
        }

        public override void Interact()
        {
            Debug.Log("Interaction with Warrior!");
        }
    }
}
