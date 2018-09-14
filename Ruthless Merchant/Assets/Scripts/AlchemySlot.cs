using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class AlchemySlot : InteractiveWorldObject {

        #region Fields ##################################################################

        /// <summary>
        /// The Ingredient stored for this Alchemyslot
        /// </summary>
        Ingredient _ingredient;

        /// <summary>
        /// The Brewing particle emitter attached to the alchemyslot
        /// </summary>
        [SerializeField]
        ParticleSystem _particlesPerma;

        /// <summary>
        /// The Explosion particle emitter attached to the alchemyslot
        /// </summary>
        [SerializeField]
        ParticleSystem _particlesExp;

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

        /// <summary>
        /// Gets called when interacting with the Alchemyslot. triggers the EnterAlchemySlot function for the player or removes the stored item
        /// </summary>
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

        /// <summary>
        /// Adds an Item to the Alchemyslot
        /// </summary>
        /// <param name="ingredient">ingredient to add</param>
        public void AddItem(Ingredient ingredient)
        {
            _ingredient = ingredient;
            if(_particlesPerma)
                _particlesPerma.Play();
                
            //Sound - AlchemyItems
            FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Alchemy Items", GameObject.FindGameObjectWithTag("Player").transform.position);
        }

        /// <summary>
        /// Removes the Item from the Slot and adds it to the inventory
        /// </summary>
        /// <param name="inventory">Inventory to add the item to</param>
        public void RemoveItem(Inventory inventory)
        {
            Item item = _ingredient.DeepCopy();
            inventory.Add(item, 1, true);
            _ingredient = null;
            if (_particlesPerma)
                _particlesPerma.Stop();
                
            //Sound - AlchemyItems
            FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Alchemy Items", GameObject.FindGameObjectWithTag("Player").transform.position);
        }

        public void ClearItem()
        {
            _ingredient = null;
            if (_particlesPerma)
                _particlesPerma.Stop();
        }

        public void DoExplosive()
        {
            if (_particlesExp)
                _particlesExp.Play();
        }

        #endregion
    }
}
