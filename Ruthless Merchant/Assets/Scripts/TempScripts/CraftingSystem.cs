using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CraftingSystem : MonoBehaviour
    {

        Recipes recipes;

        [SerializeField]
        Inventory inventory;

        private void Awake()
        {
            recipes = GetComponent<Recipes>();
            if(!recipes)
            {
                //If A Crafting-Station has no local Recipes, It will search for global recipes
                recipes = FindObjectOfType<Recipes>();
                if (!recipes)
                {
                    throw new System.Exception("No Recipes were found in this Scene");
                }
            }
            if(!inventory)
            {
                inventory = FindObjectOfType<Inventory>();
                if(!inventory)
                {
                    throw new System.Exception("No Inventory was found in this Scene");
                }
            }
        }

        /// <summary>
        /// Tries to craft and item. returns true if item was crafted successfully.
        /// </summary>
        /// <param name="index">index of the chosen recipe</param>
        public bool TryCraft(int index)
        {
            if(ContainMaterials(index))
            {
                for(int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
                {
                    inventory.Remove(recipes.GetRecipes()[index].ListOfMaterials[i].Item, recipes.GetRecipes()[index].ListOfMaterials[i].Count, false);
                }
                inventory.Add(recipes.GetRecipes()[index].Result, 1, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if required Items are in the inventory.
        /// </summary>
        /// <param name="index">index of the chosen recipe</param>
        /// <returns>return true if item can be crafted</returns>
        public bool ContainMaterials(int index)
        {
            for(int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
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
