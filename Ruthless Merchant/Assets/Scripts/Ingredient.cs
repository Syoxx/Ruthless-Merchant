using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{

    public class Ingredient : Item
    {
        [SerializeField] private IngredientType ingredientType;
        [SerializeField] private float attackSpeedBuff = 0.0f;
        [SerializeField] private float healthBuff = 0.0f;
        [SerializeField] private int defenseBuff = 0;
        [SerializeField] private int regenerationBuff = 0;
        [SerializeField] private float movementBuff = 0.0f;

        public IngredientType IngredientType
        {
            get
            {
                return ingredientType;
            }
        }

        public float AttackSpeedBuff
        {
            get
            {
                return attackSpeedBuff;
            }
        }

        public float HealthBuff
        {
            get
            {
                return healthBuff;
            }
        }

        public int DefenseBuff
        {
            get
            {
                return defenseBuff;
            }
        }

        public int RegenerationBuff
        {
            get
            {
                return regenerationBuff;
            }
        }

        public float MovementBuff
        {
            get
            {
                return movementBuff;
            }
        }

        public override void Start()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
