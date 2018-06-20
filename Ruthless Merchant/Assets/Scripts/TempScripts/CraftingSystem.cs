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

        Recipes.Recipe GetRecipe(int index)
        {
            if(!recipes)
            {
                return recipes.GetRecipes()[index];
            }
            else
            {
                return new Recipes.Recipe(new Item(), new List<Recipes.Recipe.Materials> { new Recipes.Recipe.Materials(new Item(), 1) }, false);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        ContainMaterials(int index)
        {
            for(int i = 0; i < Inventory)
        }
    }
}
