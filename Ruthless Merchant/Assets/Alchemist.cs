using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Alchemist : InteractiveWorldObject
    {
        #region Fields ##################################################################

        [SerializeField]
        AlchemySlot[] alchemySlots;

        #endregion


        #region Properties ##############################################################

        #endregion


        #region Private Functions #######################################################

        #endregion


        #region Public Functions ########################################################

        public override void Interact(GameObject caller)
        {
            throw new System.NotImplementedException();
        }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}