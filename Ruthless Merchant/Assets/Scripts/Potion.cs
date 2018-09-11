using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Potion : Item
    {
        private float attackSpeed;
        private int defenseValue;
        private float movementSpeed;
        private float healthValue;
        private int regenerationValue;

        public float AttackSpeed
        {
            get
            {
                return attackSpeed;
            }
        }

        public int DefenseValue
        {
            get
            {
                return Convert.ToInt32(defenseValue);
            }
        }

        public int MovementSpeed
        {
            get
            {
                return Convert.ToInt32(movementSpeed);
            }
        }

        public float Health
        {
            get
            {
                return healthValue;
            }
        }

        public float Regeneration
        {
            get
            {
                return regenerationValue;
            }
        }

        /// <summary>
        /// Sets the potion-buffs to the funcion-input
        /// </summary>
        /// <param name="attackSpeed"></param>
        /// <param name="defense"></param>
        /// <param name="movement"></param>
        /// <param name="health"></param>
        /// <param name="reg"></param>
        public void CreatePotion(float attackSpeed, int defense, float movement, float health, int reg)
        {
            this.attackSpeed = attackSpeed;
            this.defenseValue = defense;
            this.movementSpeed = movement;
            this.healthValue = health;
            this.regenerationValue = reg;
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }
    }

}

