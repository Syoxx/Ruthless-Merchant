using UnityEngine;

namespace RuthlessMerchant
{
    public class Civilian : NPC
    {
        [SerializeField]
        [Range(0, 1000)]
        private float fleeDistance = 5.0f;

        public override void Update()
        {
            base.Update();
        }

        public override void Flee()
        {
            throw new System.NotImplementedException();
        }

        public override void Interact()
        {
            Debug.Log("Interaction with Civilian");
        }
    }
}