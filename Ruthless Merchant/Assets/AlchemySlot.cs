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

        public Ingredient Ingredient
        {
            get { return _ingredient; }
        }

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

        public void RemoveItem(Inventory inventory)
        {
            Item item = _ingredient.DeepCopy();
            inventory.Add(item, 1, true);
            _ingredient = null;
        }

        public void ClearItem()
        {
            _ingredient = null;
        }

        #endregion
    }
}
