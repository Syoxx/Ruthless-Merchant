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

    private void Awake()
    {
        singleton = this;
    }

    private string[] Monolog = {
        "Enough fooling around, time to make some money. It looks like there is nothing in my storage, so I should go out there and find materials. ",
        "Finally I have enough materials again. I should go to a smith, so he can make a weapon out of it. There is a good smith in my cave, but I could also visit a smith in one of the military camps all over good old Isle Elysia. ",
        "I love brand new weapons. They look amazingly sharp. I can see it in front of my inner eye, the flesh it cuts. But back to business. I should sell it to one of the fools in the Outposts. However, I could also dismantle it at a workbench to retrieve some materials. ",
        "The weapons will be in the hands of some fools in a short time. But I must be careful. If I sell too many weapons to one of the two factions, the war could end sooner than expected. But I want to make money, so I have to keep it going.",
        "These Imperialists are a strong faction. But they are hiding behind great stone walls. They love all the stuff of the ancient civilization. I should keep this in mind.",
        "The Openminded are a strange faction. They live in little wooden houses and I don’t think they take hygiene that serious. But they are great in doing ambushes. "
    };

    public static void CreateMonolog(int achievementIndex)
    {
        Debug.Log("FindMaterial" + singleton.Monolog[achievementIndex]);
        singleton.TextObject.text = singleton.Monolog[achievementIndex];
    }

}
