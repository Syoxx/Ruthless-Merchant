using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class DamageAbleObject : MonoBehaviour
    {
        public class HealthArgs : EventArgs
        {
            public Character Sender;
            public int ChangedValue;

            public HealthArgs(Character sender, int changedValue)
            {
                Sender = sender;
                ChangedValue = changedValue;
            }
        }

        [SerializeField]
        [Range(0, 10000)]
        private int health = 100;

        [SerializeField]
        [Range(0, 10000)]
        private int maxHealth = 100;

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
            }
        }

        public event EventHandler<HealthArgs> OnDeath;
        public event EventHandler<HealthArgs> OnHealthChanged;

        public void ChangeHealth(int health, Character sender)
        {
            this.health += health;
            if (this.health > maxHealth)
                this.health = maxHealth;

            if (this.health <= 0)
            {
                this.health = 0;
                if(OnDeath != null)
                    OnDeath.Invoke(this, new HealthArgs(sender, health));
            }

            if (OnHealthChanged != null)
                OnHealthChanged.Invoke(this, new HealthArgs(sender, health));
        }
    }
}