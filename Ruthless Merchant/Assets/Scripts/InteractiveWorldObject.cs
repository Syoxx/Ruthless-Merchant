using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class InteractiveWorldObject : InteractiveObject
    {
        private QuestItem questItem;

        public QuestItem QuestItem
        {
            get
            {
                return questItem;
            }
            set
            {
                questItem = value;
            }
        }

        public abstract override void Interact(GameObject caller);
    }
}