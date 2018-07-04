using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Potion : Item
    {
        [Header("Potion Effects (Nur zum testen!")]
        [SerializeField] private int attackSpeedBuff;
        [SerializeField] private int defensiveBuff;
        [SerializeField] private int movementSpeedBuff;
        [SerializeField] private int healthBuff;

        public void CreatePotion(int attackSpeed, int defense, int movement, int health)
        {
            attackSpeedBuff = attackSpeed;
            defensiveBuff = defense;
            movementSpeedBuff = movement;
            healthBuff = health;
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }
    }

}

