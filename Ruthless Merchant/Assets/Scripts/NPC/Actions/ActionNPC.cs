using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class ActionNPC
    {
        protected NPC parent;
        protected GameObject other;

        public virtual void StartAction(NPC parent, GameObject other)
        {
            this.parent = parent;
            this.other = other;
            Debug.Log(GetType().FullName);
        }

        public abstract void Update(float deltaTime);

        public virtual void EndAction()
        {

        }
    }
}
