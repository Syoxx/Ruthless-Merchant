using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class InteractiveObject : MonoBehaviour
    {
        protected Faction faction;
        protected int id;
        protected bool freeze;
        protected FactionStandings factionStandings;
        protected DamageAbleObject damageAbleObject;
        protected Inventory inventory;

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public DamageAbleObject DamageAbleObject
        {
            get
            {
                return damageAbleObject;
            }
            set
            {
                damageAbleObject = value;
            }
        }

        public Inventory Inventory
        {
            get
            {
                return inventory;
            }
            set
            {
                inventory = value;
            }
        }

        public FactionStandings FactionStandings
        {
            get
            {
                return factionStandings;
            }
            set
            {
                factionStandings = value;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void Interact(GameObject caller);
    }
}