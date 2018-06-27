using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class AlchemySlot : InteractiveWorldObject {

        #region Fields ##################################################################

        Ingredient _ingredient;

        #endregion


        #region Properties ##############################################################

        #endregion


        #region Private Functions #######################################################

        #endregion


        #region Public Functions ########################################################

        public override void Interact(GameObject caller)
        {
            if(!_ingredient)
            {
                caller.GetComponent<Player>().EnterAlchemySlot(this);
            }
            else
            {
                RemoveItem(caller.GetComponent<Player>().Inventory);
            }
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }

        public void AddItem(Ingredient ingredient)
        {
            _ingredient = ingredient;
        }

        public Item RemoveItem(Inventory inventory)
        {
            Item item = (Item)_ingredient.DeepCopy();
            _ingredient = null;
            return item;
        }

        #endregion
    }
}
