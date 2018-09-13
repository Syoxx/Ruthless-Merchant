using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RuthlessMerchant {
    public class Workbench : InteractiveObject {

        [SerializeField]
        Canvas workbenchCanvas;

        [SerializeField] private UnityEvent onSuccesfullBench;


        public override void Interact(GameObject caller)
        {
            Player player = caller.GetComponent<Player>();
            player.EnterWorkbench(this);           
        }

        /// <summary>
        ///Removes item from inventory and adds materials that they get broaken down into
        /// </summary>
        public void BreakdownItem(Item BreakableItem, Inventory inventory, Recipes recipes)
        {
            for (int i = 0; i < recipes.GetRecipes().Count; i++)
            {
                if (recipes.GetRecipes()[i].Result.ItemInfo.ItemName == BreakableItem.ItemInfo.ItemName)
                {
                    
                    for (int j = 0; j < recipes.GetRecipes()[i].ListOfMaterials.Count; j++)
                    {
                        if(recipes.GetRecipes()[i].ListOfMaterials[j].Count > 1)
                            inventory.Add(recipes.GetRecipes()[i].ListOfMaterials[j].Item, recipes.GetRecipes()[i].ListOfMaterials[j].Count - 1, false);
                        else
                            inventory.Add(recipes.GetRecipes()[i].ListOfMaterials[j].Item, recipes.GetRecipes()[i].ListOfMaterials[j].Count, false);
                        Debug.Log("Added " + recipes.GetRecipes()[i].ListOfMaterials[j].Item);
                    }
                    inventory.Remove(BreakableItem, 1, true);

                    onSuccesfullBench.Invoke();
                    Debug.Log("DestroyedBlub");
                    
                    //Sound - hammer
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Forging/hammer", GameObject.FindGameObjectWithTag("Player").transform.position);

                    break;
                }         
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
