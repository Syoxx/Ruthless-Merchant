using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class InteractiveWorldObject : InteractiveObject
    {
        public abstract override void Interact(GameObject caller);
    }
}