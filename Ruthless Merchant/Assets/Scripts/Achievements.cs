using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RuthlessMerchant {
    public class Achievements : MonoBehaviour {

        public TextMeshProUGUI textMesh;
        [HideInInspector]
        public int switchIndex;
        public static Achievements Singleton;
        private int counter;
        private int counter2;
        private int RequiredAmount;
        private int RequiredAmount2;
        [HideInInspector]
        public bool Completed;

        public string[] Monolog =
        {
        "Enough fooling around, time to make some money. It looks like there is nothing in my storage, so I should go out there and find materials near the “Ancient Trade Route”. \n" ,
        "This seems to be a very ressource-rich area. I should build a outpost so I can quick travel to this destination via my map\n" ,
        "Now that I gathered everything I need to make new friends and money, I should travel to the capital of the Open-Minded.\n" ,
        "They say the Open-Minded are a XXXXX culture. Blabla about open minded. I should strengthen this culture and sell them weapons in order to weaken the other clan and raise the need of better weapons.\n" ,
        "I just received an order from a hero of the Imperialists. They want me to steal a special weapon at “Eastern Mountain Trail” and deliver it to their capital. It is probably stored in their military tent…\n",
        "Since the way to the Imperialists capital is quite long. I can make some money on my way with selling potions I can craft. Someone once told me many Vipershrooms can be found near “Uphill Path”.\n",
        "Across the bridge near “Green Leaf Path” is a known place for many other ingredients like Creaky Tuber that I could use.\n",
        "Now I can brew some potion and sell these for a good price.\n",
        "I should arrive at the “Imperialist capital” soon and finally sell this special weapon. Blablabla about Imperialisten.\n",
        "A rather large disbalance has arisen between both fractions. I should sell lots of good weapons to the Imperalists now, so they can fight back. However, I should always try to keep a balance…\n"
    };

        private string[] Goal =
        {
        "\nCollect Wood  ",
        "\nCollect Iron  ",

        "Reach “Ancient Trade Route” ",
        "Click E on the lot next to the tent ",

        "Reach “Open-Minded Capital” ",

        "Craft Weapons (Iron Mace) at smith ",
        "Sell Weapons at trader "
    };

        private void Awake()
        {
            Singleton = this;
            UpdateCanvas(0);
        }

        public static void AddToCounter(Item item = null, bool firstCounter = true)
        {
            if (item)
            {
                if (item.ItemInfo.ItemName == "Wood")
                    Singleton.counter++;
                else if (item.ItemInfo.ItemName == "Iron")
                    Singleton.counter2++;
            }
            else
            {
                if (firstCounter)
                    Singleton.counter++;
                else
                    Singleton.counter2++;
            }

            EvaluateGoal();
            UpdateCanvas(Singleton.switchIndex);
        }
        public static void UpdateCanvas(int index)
        {
            Singleton.switchIndex = index;
            switch (Singleton.switchIndex)
            {
                case 0:
                    Singleton.RequiredAmount = 6; Singleton.RequiredAmount2 = 9;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[0] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[1] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 1:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 1;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[2] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[3] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
                case 2:
                    Singleton.RequiredAmount = 1; Singleton.RequiredAmount2 = 0;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[4] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )"; break;
                case 3:
                    Singleton.RequiredAmount = 3; Singleton.RequiredAmount2 = 3;
                    Singleton.textMesh.text = Singleton.Monolog[Singleton.switchIndex] + Singleton.Goal[5] + "( " + Singleton.counter + " / " + Singleton.RequiredAmount + " )" + Singleton.Goal[6] + "( " + Singleton.counter2 + " / " + Singleton.RequiredAmount2 + " )"; break;
            }
        }

        private static void EvaluateGoal()
        {
            Singleton.Completed = (Singleton.counter >= Singleton.RequiredAmount && Singleton.counter2 >= Singleton.RequiredAmount2);
            if(Singleton.Completed)
            {
                Singleton.switchIndex++;
                Singleton.counter = 0; Singleton.counter2 = 0;
            }
        }
    } }
