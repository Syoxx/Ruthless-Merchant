using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class Workbench : Character {

        [SerializeField]
        Recipes recipe;
        [SerializeField]
        Item breakableItem;


        public override void Interact(GameObject caller)
        {
            Inventory inventory = caller.GetComponent<Player>().Inventory;
            BreakdownItem(breakableItem, caller, inventory);

        }
        public void BreakdownItem(Item BreakableItem, GameObject caller, Inventory inventory)
        {
            for (int i = 0; i < recipe.GetRecipes().Count; i++)
            {
                if (recipe.GetRecipes()[i].Result == BreakableItem)
                {

                    for (int j = 0; j < recipe.GetRecipes()[i].ListOfMaterials[j].Count; j++)
                    {
                        inventory.Add(recipe.GetRecipes()[i].ListOfMaterials[j].Item, recipe.GetRecipes()[i].ListOfMaterials[j].Count, false);
                    }
                    inventory.Remove(BreakableItem, 1, false);
                    break;
                }
            }
        }

    }
}
