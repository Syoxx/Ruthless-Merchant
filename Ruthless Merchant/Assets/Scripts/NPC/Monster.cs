using UnityEngine;

namespace RuthlessMerchant
{
    public class Monster : Fighter
    {
        public override void Interact()
        {
            Debug.Log("Interaction with Monster!");
        }
    }
}