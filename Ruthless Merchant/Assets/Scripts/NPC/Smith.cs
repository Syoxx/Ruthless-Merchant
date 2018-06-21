using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Smith : Civilian
    {
        [SerializeField]
        Recipes recipes;

        public override void Update()
        {
            
        }

        public override void Start()
        {
            if(!recipes)
            {
                recipes = GetComponent<Recipes>();
            }
        }

        public override void Interact(GameObject caller)
        {
            Inventory inventory = caller.GetComponent<Player>().Inventory;
            if(!recipes)
            {
                recipes = caller.GetComponent<Recipes>();
            }
            if(recipes)
            {
                TryCraft(inventory, 0);
            }
        }

        public void TryCraft(Inventory inventory, int index)
        {
            if (ContainMaterials(inventory, index))
            {
                for (int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
                {
                    inventory.Remove(recipes.GetRecipes()[index].ListOfMaterials[i].Item, recipes.GetRecipes()[index].ListOfMaterials[i].Count, false);
                }
                inventory.Add(recipes.GetRecipes()[index].Result, 1, true);
            }
        }

        public bool ContainMaterials(Inventory inventory, int index)
        {
            for (int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
            {
                if (recipes.GetRecipes()[index].ListOfMaterials[i].Count > inventory.GetNumberOfItems(recipes.GetRecipes()[index].ListOfMaterials[i].Item))
                {
                    return false;
                }
            }
            return true;
        }
    }

}