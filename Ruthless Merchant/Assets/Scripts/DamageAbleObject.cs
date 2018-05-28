using System;

namespace RuthlessMerchant
{
    public class DamageAbleObject
    {
        private int health;
        private int maxHealth;

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

        public event EventHandler OnDeath;
        public event EventHandler OnHealthChanged;

        public void ChangeHealth()
        {
            throw new System.NotImplementedException();
        }
    }
}