using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RuthlessMerchant {
    /// <summary>
    /// Includes the logic of the Achievement System
    /// </summary>
    public class Achievements : MonoBehaviour {

        public TextMeshProUGUI textMesh;
        [HideInInspector]
        public int switchIndex;
        public static Achievements Singleton;
        private int counter;
        private int counter2;
        private int RequiredAmount;
        private int RequiredAmount2;

        [SerializeField]
        [Tooltip("Drage a StealSpecianWeapon from Achievements/EasternMountainTrail there")]
        private Item StealSpecialWeapon;

        [SerializeField]
        [Tooltip("Drage a Iron Mace there")]
        private Item IronMaceWeapon;

        private bool ironMaceCrafted;

        [HideInInspector]
        public bool Completed;

        private string[] Monolog =
        {
        "Enough fooling around, it is time to make some money. From the looks of it, my storage seems to be empty once again. Maybe I’ll be able to find something of value near the Ancient Trade Route.\n" ,
        "Well, this place seems to meet my needs. Maybe I should build an outpost here, that I can use for fast travelling by opening the map.\n" ,
        "Now I have gathered everything I need to make some new friends. I should travel to the Open-Minded’s capital to finally make some money.\n" ,
        "They say the Open-Minded are a very strange culture. They are living in the woods, ignoring their origins. I should help these poor people by selling them some weapons. This has the side benefit of weakening those Imperialists and raising their need for better weapons.\n" ,
        "A hero of the Imperialists wants me to steal a special weapon at “Eastern Mountain Trail” and deliver it to the capital. The weapon is probably stored in a military tent…\n",
        "Because of the rather long way to the Imperialist’s capital, I guess there is time to make some money on the way by brewing and selling some shady potions. Someone once told me that many poisonous Vipershrooms can be found near “Uphill Path”.\n",
        "The place across the bridge near “Green Leaf Path” is well-known for other ingredients like the Creaky Tuber. I certainly could make use of those.\n",
        "Now I can brew some potions and sell them for profit.\n",
        "I should arrive at the “Imperialist capital” any second to finally be able to sell this special weapon. The Imperialists think that they are the superior culture and that they are blessed by the Ancestors. Let’s see if they can handle a weapon like this.\n",
        "Before I arrive at the “Imperialist capital” I should talk a break at “Imperial Roadway” and talk to a so-called hero about a mission.\n",
        "A rather large imbalance has arisen between both factions. Everything is working in my favor! I should sell lots of good weapons to the Imperalists now, so they can stand a chance. I should try to keep both armies from gaining too much control…\n"
    };

        private string[] Goal =
        {
        "\nCollect Wood  ",
        "\nCollect Iron  ",

        "\nReach “Ancient Trade Route” ",
        "\nClick E on the lot next to the tent ",

        "\nReach “Open-Minded Capital” ",

        "\nCraft Weapons (Iron Mace) at smith ",
        "\nSell Weapons at trader ",

        "Reach “Eastern Mountain Trail” ",
        "Steal weapon ",

        "Reach Uphill Path ",
        "Collect Vipershrooms ",

        "Reach “Green Leaf Path” ",
        "Collect Creaky Tuber ",

        "Brew potions ",
        "Sell potions ",

        "Reach “Imperial Roadway”",
        "Order a collection mission from a hero",

        "Reach “Imperial Capital” ",
        "Sell special Weapon",
    };

        private void Awake()
        {
            Singleton = this;
            UpdateCanvas(0);
            Trade.ItemsSold += OnItemSold;
            
        }

        void Update()
        {
            if (Singleton != null)
            {
                if (Singleton.switchIndex == 3 && !ironMaceCrafted)
                {
                    Singleton.counter = Player.Singleton.Inventory.GetNumberOfItems(IronMaceWeapon);
                    if (Singleton.counter == 1)
                    {
                        ironMaceCrafted = true;
                    }
                }
            }

        }

        /// <summary>
        /// Signal function to check if the Potion was brewed
        /// </summary>
        public void BrewingPotion()
        {
            if (Singleton.switchIndex == 7)
            {
                Singleton.counter++;
                EvaluateGoal();
                UpdateCanvas(Singleton.switchIndex);
            }
        }
        /// <summary>
        /// Singal function to check if the Item was sold
        /// </summary>
        void OnItemSold(object sender, EventArgs args)
        {
            if (Singleton.switchIndex == 7)
            {
                Singleton.counter2++;
                EvaluateGoal();
                UpdateCanvas(Singleton.switchIndex);
            }

            if (Singleton.switchIndex == 8)
            {
                Singleton.counter2++;
                EvaluateGoal();
                UpdateCanvas(Singleton.switchIndex);
            }
            if (Singleton.switchIndex == 3 && ironMaceCrafted)
            {
                Singleton.counter2++;
                EvaluateGoal();
                UpdateCanvas(Singleton.switchIndex);
            }
        }

        /// <summary>
        /// This method is used to add counter (+1) to an completed part of an Achievement
        /// Also be carefull while choosing what counter are you going to increment
        /// </summary>
        /// <param name="item">Some Achievements do require items to be collected.</param>
        /// <param name="firstCounter">If there are no items required</param>
        public static void AddToCounter(Item item = null, bool firstCounter = true)
        {
            if (item)
            {
                if (Singleton.switchIndex < 1)
                {
                    if (item.ItemInfo.ItemName == "Wood")
                        Singleton.counter++;
                    else if (item.ItemInfo.ItemName == "Iron")
                        Singleton.counter2++;
                }
                if (Singleton.switchIndex == 4)
                {
                    if (item.ItemInfo.ItemName == "Ancient Axe")
                        Singleton.counter2++;
                }
                if (Singleton.switchIndex == 5)
                {
                    if (item.ItemInfo.ItemName == "Vipershroom")
                        Singleton.counter2++;
                }
                if (Singleton.switchIndex == 6)
                {
                    if (item.ItemInfo.ItemName == "Creaky Tuber")
                        Singleton.counter2++;
                }
            }
            else
            {
                if (firstCounter && Singleton.RequiredAmount > Singleton.counter)
                {
                    Singleton.counter++;
                }
                else if (Singleton.RequiredAmount2 > Singleton.counter2)
                {
                    Singleton.counter2++;
                }
            }

            EvaluateGoal();
            UpdateCanvas(Singleton.switchIndex);
        }

        /// <summary>
        ///Displays the appropriate monolog text and goals depending on how far the player has progressed through the achievements
        /// </summary>
        /// <param name="index">Index of a Canvas </param>
        public static void UpdateCanvas(int index)
        {
            Singleton.switchIndex = index;
            switch (Singleton.switchIndex)
            {
                case 0:
                    Singleton.RequiredAmount = 6; Singleton.RequiredAmount2 = 6;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[0] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[1] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 1:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[2] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[3] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 2:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 0;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[4] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )"; break;
                case 3:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[5] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[6] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 4:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[7] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[8] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 5:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 5;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[9] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[10] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 6:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 5;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[11] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[12] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 7:
                    Singleton.RequiredAmount = 3; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[13] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[14] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 8:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[16] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[17] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 9:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 0;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[15] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )";
                    break;
                //case 10:
                //    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex]; break;
                case 10:
                    Singleton.RequiredAmount = 0; Singleton.RequiredAmount2 = 0;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex]; break;

            }
        }

        /// <summary>
        ///Check if the appropriate goals are completed and if true, jump to the next Achievement
        /// <summary>
        /// </summary>
        private static void EvaluateGoal()
        {
            Singleton.Completed = (Singleton.counter >= Singleton.RequiredAmount && Singleton.counter2 >= Singleton.RequiredAmount2);
            if(Singleton.Completed)
            {
                Singleton.switchIndex++;
                Singleton.counter = 0; Singleton.counter2 = 0;
            }
        }
        /// <summary>
        /// Some Achievements do require to reach some locations.
        /// Use this to check if the target was reached.
        /// You'll need a name of the object.
        /// </summary>
        /// <param name="other">Collider of a target</param>
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.name)
            {
                case "AncientTradeRouteTrigger":
                    if (Singleton.switchIndex == 1)
                    {
                        AddToCounter();
                        Destroy(other);
                    }
                    break;
                case "OpenMindedCapitalTrigger":
                    if (Singleton.switchIndex == 2)
                    {
                        AddToCounter();
                    }

                    break;
                case "EasternMountainTrailTrigger":
                    if (Singleton.switchIndex == 4)
                    {
                        AddToCounter();
                    }
                    break;
                case "UphillPathTrigger":
                    if (Singleton.switchIndex == 5)
                    {
                        AddToCounter();
                    }
                    break;
                case "GreenLeafPathTrigger":
                    if (Singleton.switchIndex == 6)
                    {
                        AddToCounter();
                    }
                    break;
                case "ImperialistCityTrigger":
                    if (Singleton.switchIndex == 8)
                    {
                        AddToCounter();
                    }
                    break;
                case "EmpirialRoadwayTrigger":
                    if (Singleton.switchIndex == 9)
                    {
                        AddToCounter();
                    }
                    break;                 
            }
        }
    }
}
