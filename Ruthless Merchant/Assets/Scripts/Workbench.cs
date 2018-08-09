using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class Workbench : InteractiveObject {

        [SerializeField]
        Canvas workbenchCanvas;


        public override void Interact(GameObject caller)
        {
            Player player = caller.GetComponent<Player>();
            player.EnterWorkbench(this);
        }

        public void BreakdownItem(Item BreakableItem, Inventory inventory, Recipes recipes)
        {
            for (int i = 0; i < recipes.GetRecipes().Count; i++)
            {
                if (recipes.GetRecipes()[i].Result.ItemName == BreakableItem.ItemName)
                {
                    
                    for (int j = 0; j < recipes.GetRecipes()[i].ListOfMaterials.Count; j++)
                    {
                        if(recipes.GetRecipes()[i].ListOfMaterials[j].Count > 1)
                            inventory.Add(recipes.GetRecipes()[i].ListOfMaterials[j].Item, recipes.GetRecipes()[i].ListOfMaterials[j].Count - 1, true);
                        else
                            inventory.Add(recipes.GetRecipes()[i].ListOfMaterials[j].Item, recipes.GetRecipes()[i].ListOfMaterials[j].Count, true);
                        Debug.Log("Added " + recipes.GetRecipes()[i].ListOfMaterials[j].Item);
                    }
                    inventory.Remove(BreakableItem, 1, true);
                    break;
                }
                else continue;               
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
