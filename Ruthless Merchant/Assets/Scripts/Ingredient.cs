using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{

    public class Ingredient : Item
    {
        [SerializeField] private IngredientType ingredientType;

        public IngredientType IngredientType
        {
            get
            {
                return ingredientType;
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
