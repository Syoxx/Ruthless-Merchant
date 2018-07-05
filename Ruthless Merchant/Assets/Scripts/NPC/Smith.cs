using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Smith : Civilian
    {
        public static Smith CurrentSmith;


        public class SmithArgs : EventArgs
        {
            public Character Sender;

            public SmithArgs(Character sender)
            {
                Sender = sender;
            }
        }

        [SerializeField]
        Recipes recipes;

        public static event EventHandler<SmithArgs> PlayerTriggerEnter;
        public static event EventHandler<SmithArgs> PlayerTriggerExit;

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

        private void OnTriggerEnter(Collider other)
        {
            Character collidingChar = other.gameObject.GetComponent<Character>();

            if (collidingChar != null)
            {
                if (collidingChar.IsPlayer)
                {
                    //enable smith crafting
                    PlayerTriggerEnter.Invoke(this, new SmithArgs(this));
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
                    //disable smith crafting
                    PlayerTriggerExit.Invoke(this, new SmithArgs(this));
                }
            }
        }
    }

}