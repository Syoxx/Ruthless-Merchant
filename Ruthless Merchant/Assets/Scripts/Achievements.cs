using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Achievements : MonoBehaviour {

    [SerializeField]
    private GameObject AchievementPanel;
    [SerializeField]
    private TextMeshProUGUI TextObject;
    public static Achievements singleton;
    
    public int counter;
    private int requiredAmount = 5;

    public int stateNr;


    private void Awake()
    {
        singleton = this;
    }

    private string[] Monolog = {
        "Enough fooling around, time to make some money. It looks like there is nothing in my storage, so I should go out there and find materials. \n",
        "Finally I have enough materials again. I should go to a smith, so he can make a weapon out of it. There is a good smith in my cave, but I could also visit a smith in one of the military camps all over good old Isle Elysia. \n",
        "I love brand new weapons. They look amazingly sharp. I can see it in front of my inner eye, the flesh it cuts. But back to business. I should sell it to one of the fools in the Outposts. However, I could also dismantle it at a workbench to retrieve some materials. \n",
        "The weapons will be in the hands of some fools in a short time. But I must be careful. If I sell too many weapons to one of the two factions, the war could end sooner than expected. But I want to make money, so I have to keep it going. \n",
        "These Imperialists are a strong faction. But they are hiding behind great stone walls. They love all the stuff of the ancient civilization. I should keep this in mind.",
        "The Openminded are a strange faction. They live in little wooden houses and I don’t think they take hygiene that serious. But they are great in doing ambushes. \n"
    };
    private string[] Goal =
    {
        "\nFind any 5 Materials: " ,
        "\nCraft a random weapon: " ,
        "\nSell and break down a random weapon: "



    };

    public static void CreateMonolog(int achievementIndex, bool AddToText = false, bool isGoal = false)
    {
        //Debug.Log("FindMaterial" + singleton.Monolog[achievementIndex]);
        //if (AddToText)
        //    singleton.TextObject.text = singleton.TextObject.text + singleton.Monolog[achievementIndex];
        //else
        //    singleton.TextObject.text = singleton.Monolog[achievementIndex];

        //if (isGoal)
        //{
        //    singleton.TextObject.text = singleton.TextObject.text + singleton.Goal[achievementIndex] + singleton.counter.ToString();
        //}
        singleton.stateNr = achievementIndex+1;
        
    }
    public static void AddToCounter()
    {
        singleton.counter++;
        Debug.Log("counter:" + singleton.counter);
        //singleton.TextObject.text = singleton.TextObject.text.Substring(0, singleton.TextObject.text.Length - 1) + singleton.counter;
        //singleton.TextObject.text = singleton.TextObject.text + singleton.counter;
        UpdateCanvas(singleton.stateNr);
        CheckCounter();
    }

    private static void CheckCounter()
    {
        if (singleton.counter >= singleton.requiredAmount)
        {
            singleton.stateNr++;
            singleton.counter = 0;
            UpdateCanvas(singleton.stateNr);
        }
    }

    public static void UpdateCanvas(int achievementIndex)
    {
        singleton.stateNr = achievementIndex;
        Debug.Log("switch " + singleton.stateNr);
            switch (singleton.stateNr)
            {
                case 1:
                        singleton.requiredAmount = 5; 
                    singleton.TextObject.text = singleton.Monolog[0] + singleton.Goal[0] + singleton.counter + " / " + singleton.requiredAmount;
                        break;
                case 2: singleton.requiredAmount = 1; 
                    singleton.TextObject.text = singleton.Monolog[1] + singleton.Goal[1] + singleton.counter + " / " + singleton.requiredAmount;
                    break;
                case 3: singleton.requiredAmount = 1;
                    singleton.TextObject.text = singleton.Monolog[2] + singleton.Goal[2] + singleton.counter + " / " + singleton.requiredAmount;
                    break;
                case 4:
                    singleton.TextObject.text = singleton.Monolog[3] + singleton.Goal[3] + singleton.counter + " / " + singleton.requiredAmount;
                    break;
                case 5:
                    singleton.TextObject.text = singleton.Monolog[4] + singleton.Goal[4] + singleton.counter + " / " + singleton.requiredAmount;
                    break;
                case 6:
                    singleton.TextObject.text = singleton.Monolog[5] + singleton.Goal[5] + singleton.counter + " / " + singleton.requiredAmount;
                break;
        }
    }


}
