using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Smith : Civilian
    {
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

        public void TryCraft(Inventory inventory, int index, Recipes recipes)
        {
            if (ContainMaterials(inventory, index, recipes))
            {
                for (int i = 0; i < recipes.GetRecipes()[index].ListOfMaterials.Count; i++)
                {
                    inventory.Remove(recipes.GetRecipes()[index].ListOfMaterials[i].Item, recipes.GetRecipes()[index].ListOfMaterials[i].Count, false);
                }
                inventory.Add(recipes.GetRecipes()[index].Result, 1, true);
            }
        }

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
    }

}