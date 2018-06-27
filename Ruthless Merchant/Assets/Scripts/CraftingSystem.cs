using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CraftingSystem : MonoBehaviour
    {
        [SerializeField]
        Recipes recipes;

        [SerializeField]
        Inventory inventory;

        private void Awake()
        {
            if(recipes == null)
            {
                throw new System.Exception("No Recipes Added");
            }
            if(inventory == null)
            {
                throw new System.Exception("No Inventory Added");
            }
        }

        /// <summary>
        /// Tries to craft and item. returns true if item was crafted successfully.
        /// </summary>
        /// <param name="index">index of the chosen recipe</param>
        public void TryCraft(int index)
        {
            if(ContainMaterials(index))
            {
                for(int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
                {
                    inventory.Remove(recipes.GetRecipes()[index].ListOfMaterials[i].Item, recipes.GetRecipes()[index].ListOfMaterials[i].Count, false);
                }
                inventory.Add(recipes.GetRecipes()[index].Result, 1, true);
            }
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
