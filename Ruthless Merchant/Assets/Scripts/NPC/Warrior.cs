using UnityEngine;

namespace RuthlessMerchant
{
    public class Warrior : Fighter
    {
        public override void Start()
        {
            base.Start();
        }

        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Warrior!");
        }
    }
}
