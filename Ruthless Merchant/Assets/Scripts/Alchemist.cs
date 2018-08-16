using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Alchemist : InteractiveWorldObject
    {
        #region Fields ##################################################################

        private AlchemySlot[] alchemySlots;
        #endregion


        #region Properties ##############################################################

        #endregion


        #region Private Functions #######################################################

        private void Awake()
        {
            alchemySlots = GetComponentsInChildren<AlchemySlot>();
        }

        #endregion


        #region Public Functions ########################################################

        public override void Interact(GameObject caller)
        {
            float atk = 0;
            int def = 0;
            float speed = 0;
            float hp = 0;
            int reg = 0;

            int atkCount = 0;
            int defCount = 0;
            int speedCount = 0;
            int hpCount = 0;

            bool hasItem = false;
            for(int i = 0; i < alchemySlots.Length; i++)
            {
                if(alchemySlots[i].Ingredient)
                {
                    atk += alchemySlots[i].Ingredient.AttackSpeedBuff;
                    def += alchemySlots[i].Ingredient.DefenseBuff;
                    hp += alchemySlots[i].Ingredient.HealthBuff;
                    speed += alchemySlots[i].Ingredient.MovementBuff;
                    reg += alchemySlots[i].Ingredient.RegenerationBuff;

                    if (alchemySlots[i].Ingredient.AttackSpeedBuff > 0)
                        atkCount++;
                    else if(alchemySlots[i].Ingredient.AttackSpeedBuff < 0)
                        atkCount--;

                    if (alchemySlots[i].Ingredient.HealthBuff > 0)
                        hpCount++;
                    else if (alchemySlots[i].Ingredient.HealthBuff < 0)
                        hpCount--;

                    if (alchemySlots[i].Ingredient.MovementBuff > 0)
                        speedCount++;
                    else if (alchemySlots[i].Ingredient.MovementBuff < 0)
                        speedCount--;

                    if (alchemySlots[i].Ingredient.DefenseBuff > 0)
                        defCount++;
                    else if (alchemySlots[i].Ingredient.DefenseBuff < 0)
                        defCount--;

                    hasItem = true;
                    alchemySlots[i].ClearItem();
                }
            }
            if(!hasItem)
            {
                return;
            }

            string itemName = PotionName(atkCount, defCount, speedCount, hpCount);

            Potion potion = new GameObject(itemName).AddComponent<Potion>();
            potion.CreatePotion(atk, def, speed, hp, reg);
            potion.ItemName = itemName;
            potion.ItemType = ItemType.ConsumAble;
            potion.ItemRarity = Rarity(atkCount, defCount, speedCount, hpCount);

            Player p = caller.GetComponent<Player>();
            p.Inventory.Add(potion.DeepCopy(), 1, true);
            Destroy(potion.gameObject);

            //Potion Pickup sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Pickup potion", GameObject.FindGameObjectWithTag("Player").transform.position);
        }

        /// <summary>
        /// Sets the rarity of the potion, depending on the total power
        /// </summary>
        private ItemRarity Rarity(int atk, int def, int speed, int hp)
        {
            int totalPower = Mathf.Abs(atk) + Mathf.Abs(def) + Mathf.Abs(speed) + Mathf.Abs(hp);

            if (totalPower >= 3)
            {
                return ItemRarity.Selten;
            }
            if (totalPower >= 2)
            {
                return ItemRarity.Ungewöhnlich;
            }
            return ItemRarity.Üblich;
        }

        /// <summary>
        /// Creates a Name for the Potion
        /// </summary>
        /// <param name="atk">ATK-Parameter of the potion</param>
        /// <param name="def">DEF-Parameter of the potion</param>
        /// <param name="speed">Speed-Parameter of the potion</param>
        /// <param name="hp">Health Regeneration-Parameter of the potion</param>
        /// <returns>final name as string</returns>
        string PotionName(int atk, int def, int speed, int hp)
        {
            string name = "Potion";

            if (atk != 0)
            {
                name += GetPowerAsString(atk);
                if (atk > 0)
                {
                    name += " STR";
                }
                else
                {
                    name += " WEAK";
                }
            }
            if (def != 0)
            {
                if (atk != 0)
                {
                    name += " &";
                }
                name += GetPowerAsString(def);
                if (def > 0)
                {
                    name += " DEF";
                }
                else
                {
                    name += " FRAG";
                }
            }
            if (speed != 0)
            {
                if (atk != 0 || def != 0)
                {
                    name += " &";
                }
                name += GetPowerAsString(speed);
                if (speed > 0)
                {
                    name += " SPED";
                }
                else
                {
                    name += " SLOW";
                }
            }
            if (hp != 0)
            {
                if (atk != 0 || def != 0 || speed != 0)
                {
                    name += " &";
                }
                name += GetPowerAsString(hp);
                if (hp > 0)
                {
                    name += " HEAL";
                }
                else
                {
                    name += " HURT";
                }
            }
            return name;
        }

        /// <summary>
        /// Creates a part-name that describes the strength of the potion
        /// </summary>
        /// <param name="power">strength value</param>
        /// <returns>returns the strength as string</returns>
        string GetPowerAsString(int power)
        {
            if(Mathf.Abs(power) >2)
            {
                return " 3x";
            }

            else if (Mathf.Abs(power) > 1)
            {
                return " 2x";
            }
            return "";
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }

        #endregion
    }
}