using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Smith : Civilian
    {
        #region Fields #############################################################################################



        #endregion


        #region Properties #########################################################################################



        #endregion


        #region Structs ############################################################################################



        #endregion


        #region Private Functions ##################################################################################



        #endregion


        #region Public Functions ##################################################################################

        public override void Update()
        {
        }

        public override void Start()
        {
        }

        public override void Interact(GameObject caller)
        {
            Player player = caller.GetComponent<Player>();
            player.EnterSmith(this);
        }

        /// <summary>
        /// This Function tries to craft a Item and add it to the inventory.
        /// </summary>
        /// <param name="inventory">The inventory with the materials and where the item will be placed</param>
        /// <param name="index">The index of the recipe in the recipes-List</param>
        /// <param name="recipes">the recipes class where all the recipes are defines</param>
        /// <returns>returns true if the craft was successfull and false if player didn't have enough material</returns>
        public bool TryCraft(Inventory inventory, int index, Recipes recipes)
        {
            if (ContainMaterials(inventory, index, recipes))
            {
                for (int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
                {
                    inventory.Remove(recipes.GetRecipes()[index].ListOfMaterials[i].Item, recipes.GetRecipes()[index].ListOfMaterials[i].Count, false);
                }
                inventory.Add(recipes.GetRecipes()[index].Result, 1, true);

                //Sound - OpenForge
                FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Forging/OpenForge", GameObject.FindGameObjectWithTag("Player").transform.position);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to find out whether there are enough materials to craft a specific recipe
        /// </summary>
        /// <param name="inventory">The inventory with the materials</param>
        /// <param name="index">The index of the recipe in the recipes-List</param>
        /// <param name="recipes">the recipes class where all the recipes are defines</param>
        /// <returns>returns true if there are enough materials in the inventory to craft the recipe</returns>
        public bool ContainMaterials(Inventory inventory, int index, Recipes recipes)
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

        #endregion
    }
}