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

        public string[] Monolog =
        {
       "Enough fooling around, time to make some money. It looks like there is nothing in my storage, so I should go out there and find materials near the “Ancient Trade Route”. \n" ,
        "This seems to be a very ressource-rich area. I should build a outpost so I can quick travel to this destination via my map\n" ,
        "Now that I gathered everything I need to make new friends and money, I should travel to the capital of the Open-Minded.\n" ,
        "They say the Open-Minded are a strange culture. They live in little wooden houses and I don’t think they take hygiene that serious. But they are great in doing ambushes. I should strengthen this culture and sell them weapons in order to weaken the other clan and raise the need of better weapons.\n" ,
        "I just received an order from a hero of the Imperialists. They want me to steal a special weapon at “Eastern Mountain Trail” and deliver it to their capital. It is probably stored in their military tent…\n",
        "Since the way to the Imperialists capital is quite long. I can make some money on my way with selling potions I can craft. Someone once told me many Vipershrooms can be found near “Uphill Path”.\n",
        "Across the bridge near “Green Leaf Path” is a known place for many other ingredients like Creaky Tuber that I could use.\n",
        "Now I can brew some potion and sell these for a good price.\n",
        "I should arrive at the “Imperialist capital” soon and finally sell this special weapon. These Imperialists are a strong faction. But they are hiding behind great stone walls. They love all the stuff of the ancient civilization. I should keep this in mind.\n",
        "Before I arrive at the “Imperialist capital” I should stop by at “Imperial Roadway” and talk to a hero about a mission.",
        "A rather large disbalance has arisen between both fractions. I should sell lots of good weapons to the Imperalists now, so they can fight back. However, I should always try to keep a balance…\n"
    };

        private string[] Goal =
        {
        "\nCollect Wood  ",
        "\nCollect Iron  ",

        "\nReach “Ancient Trade Route” ",
        "\nClick E on the sign next to the tent ",

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

        "Reach “Imperial Capital” ",

        "Reach “Imperial Broadway” (0/1)",
        "Order a collection mission from a hero (0/1)"
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
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 0;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[15] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )"  ;
                    break;
                case 9:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[16] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[17] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 10:
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
