using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Alchemist : InteractiveWorldObject
    {
        #region Fields ##################################################################

        AlchemySlot[] alchemySlots;

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
            int atk = 0;
            int def = 0;
            int speed = 0;
            int hp = 0;

            bool hasItem = false;
            for(int i = 0; i < alchemySlots.Length; i++)
            {
                if(alchemySlots[i].Ingredient)
                {
                    switch(alchemySlots[i].Ingredient.IngredientType)
                    {
                        case IngredientType.Schwertgras:
                            atk++;
                            break;
                        case IngredientType.Stichelpilz:
                            atk--;
                            break;
                        case IngredientType.SteinernderRuestling:
                            def++;
                            break;
                        case IngredientType.GemeinerDornling:
                            def--;
                            break;
                        case IngredientType.Windfarn:
                            speed++;
                            break;
                        case IngredientType.KnarzigeKnolle:
                            speed--;
                            break;
                        case IngredientType.Segensblüte:
                            hp++;
                            break;
                        case IngredientType.Grabesmoos:
                            hp--;
                            break;
                    }
                    hasItem = true;
                    alchemySlots[i].ClearItem();
                }
            }
            if(!hasItem)
            {
                return;
            }

            string itemName = PotionName(atk, def, speed, hp);

            Potion potion = new GameObject(itemName).AddComponent<Potion>();
            potion.CreatePotion(atk, def, speed, hp);
            potion.itemName = itemName;
            potion.itemType = ItemType.ConsumAble;

            Player p = caller.GetComponent<Player>();
            p.Inventory.Add(potion.DeepCopy(), 1, true);
            Destroy(potion.gameObject);
        }

        string PotionName(int atk, int def, int speed, int hp)
        {
            string name = "potion of";

            if (atk != 0)
            {
                name += GetPowerAsString(atk);
                if (atk > 0)
                {
                    name += " strength";
                }
                else
                {
                    name += " weakness";
                }
            }
            if (def != 0)
            {
                if (atk != 0)
                {
                    name += " and";
                }
                name += GetPowerAsString(def);
                if (def > 0)
                {
                    name += " defense";
                }
                else
                {
                    name += " fragility";
                }
            }
            if (speed != 0)
            {
                if (atk != 0 || def != 0)
                {
                    name += " and";
                }
                name += GetPowerAsString(speed);
                if (speed > 0)
                {
                    name += " speed";
                }
                else
                {
                    name += " sloth";
                }
            }
            if (hp != 0)
            {
                if (atk != 0 || def != 0 || speed != 0)
                {
                    name += " and";
                }
                name += GetPowerAsString(hp);
                if (hp > 0)
                {
                    name += " health";
                }
                else
                {
                    name += " poison";
                }
            }
            return name;
        }

        string GetPowerAsString(int power)
        {
            if(Mathf.Abs(power) >2)
            {
                return " supreme";
            }

            else if (Mathf.Abs(power) > 1)
            {
                return " great";
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