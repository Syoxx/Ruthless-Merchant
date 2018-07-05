using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class Workbench : InteractiveObject {

        public class WorkbenchArgs : EventArgs
        {
            public Workbench Sender;

            public WorkbenchArgs(Workbench sender)
            {
                Sender = sender;
            }
        }

        [SerializeField]
        static Recipes recipes;
        [SerializeField]
        Canvas workbenchCanvas;

        public static event EventHandler<WorkbenchArgs> PlayerTriggerEnter;
        public static event EventHandler<WorkbenchArgs> PlayerTriggerExit;

        public override void Interact(GameObject caller)
        {
            Player player = caller.GetComponent<Player>();
            player.EnterWorkbench(this);
        }
        public static void BreakdownItem(Item BreakableItem, Inventory inventory)
        {
            for (int i = 0; i < recipes.GetRecipes().Count; i++)
            {
                if (recipes.GetRecipes()[i].Result == BreakableItem)
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

        private void OnTriggerEnter(Collider other)
        {
            Character collidingChar = other.gameObject.GetComponent<Character>();

            if (collidingChar != null)
            {
                if (collidingChar.IsPlayer)
                {
                    //enable workbench crafting
                    PlayerTriggerEnter.Invoke(this, new WorkbenchArgs(this));
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Character collidingChar = other.gameObject.GetComponent<Character>();

            if (collidingChar != null)
            {
                if (collidingChar.IsPlayer)
                {
                    //disable workbench crafting
                    PlayerTriggerExit.Invoke(this, new WorkbenchArgs(this));
                }
            }
        }
    }
}
