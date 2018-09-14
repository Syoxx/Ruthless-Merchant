using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class CollectionGoals : MonoBehaviour
    {
        /// <summary>
        /// This class handles the various collection quests that the player can choose from in the outposts and displaying the right button
        /// </summary>

        [SerializeField]
        private List<CollectionGoal> collectionGoals;

        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private Transform Page14_Panel;

        [SerializeField]
        private Transform Page15_Panel;

        [SerializeField]
        private GameObject iconPrefab;

        private List<CollectionGoal> CollectionGoalClones = new List<CollectionGoal>();
        private bool questingEnabled;
        private bool isQuestBeingTracked = false;
        private CollectionGoal collectionGoal;
        private Transform buttonParent;

        private List<GameObject> buttons = new List<GameObject>();


        public CollectionGoal CollectionGoal
        {
            get
            {
                if (collectionGoal == null)
                {
                    collectionGoal = GetComponent<CollectionGoal>();
                }
                return collectionGoal;
            }
        }


        private void Start()
        {
            collectionGoal = GetComponent<CollectionGoal>();           
        }

        /// <summary>
        ///get goal-script from hero and check if player has enough money for a the appropriate reward
        /// </summary>
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("NPC") && collectionGoal == null)
            {
                collectionGoal = other.gameObject.GetComponent<CollectionGoal>();
            }
            if (other.gameObject.CompareTag("Player"))
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i] != null)
                    {
                        if (buttons[i].GetComponent<QuestButton>().isDisabled)
                        {
                            if (i < collectionGoals.Count)
                            {
                                if (Player.Singleton.Inventory.PlayerMoney >= collectionGoals[i].Reward)
                                {
                                    buttons[i].GetComponent<QuestButton>().DefaultColor();
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        ///enable questing when player enters outpost and instantiate buttons
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = true;
                isQuestBeingTracked = false;

                for (int questindex = 0; questindex < collectionGoals.Count; questindex++)
                {
                    if (collectionGoals[questindex].Completed || collectionGoals[questindex].InProgress)
                    {
                        isQuestBeingTracked = true;
                        break;
                    }
                }

                for (int i = 0; i < collectionGoals.Count; i++)
                {
                    int localIndex = i;

                    for (int k = 0; k < buttons.Count; k++)
                    {
                        if (buttons[k])
                        {
                            if (collectionGoals[i].QuestTitle == buttons[k].GetComponent<QuestDisplayedData>().Name.text)
                            {
                                i++;
                                break;
                            }
                        }
                    }
                    if (i < collectionGoals.Count)
                    {

                        if (isQuestBeingTracked)
                        {
                            if (i > 1)
                            {
                                buttonParent = Page15_Panel;
                            }
                            else
                            {
                                buttonParent = Page14_Panel;
                            }
                        }
                        else 
                        {
                            if (i > 2)
                            {
                                buttonParent = Page15_Panel;
                            }
                            else
                            {
                                buttonParent = Page14_Panel;
                            }
                        }

                        GameObject questButton = Instantiate(buttonPrefab, buttonParent) as GameObject;
                        QuestDisplayedData questData = questButton.GetComponent<QuestDisplayedData>();
                        questData.Name.text = collectionGoals[i].QuestTitle;
                        questData.Description.text = collectionGoals[i].Description;
                        questData.Reward.text = "Reward: " + collectionGoals[i].Reward.ToString() + "$";
                        questData.ReputationGain.text = "Reputation: " + collectionGoals[i].ReputationGain.ToString() + "%";
                        for (int j = 0; j < collectionGoals[i].collectables.Count; j++)
                        {
                            Texture image = collectionGoals[i].collectables[j].icon;
                            GameObject iconClone = Instantiate(iconPrefab, questButton.transform.Find("itemIcon"));
                            RawImage iconImage = iconClone.GetComponent<RawImage>();
                            iconImage.texture = image;
                            Text counter = iconClone.GetComponentInChildren<Text>();
                            counter.text = collectionGoals[i].collectables[j].requiredAmount.ToString() + "X";
                        }
                        buttons.Add(questButton);

                        Button btn = questButton.GetComponent<Button>();
                        btn.onClick.AddListener(delegate { AssignQuest(localIndex, questButton); });

                        if (Player.Singleton.Inventory.PlayerMoney < collectionGoals[i].Reward)
                        {
                            //collectionGoals[i].GreyOutButton();
                            questButton.GetComponent<QuestButton>().GreyOut();
                        }

                    }
                }
            }
        }

        /// <summary>
        ///disable questing when player exits outpost and delete unassigned buttons
        /// <summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {   
                
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i])
                    {
                        if (!buttons[i].GetComponent<QuestButton>().inProgress)
                            Destroy(buttons[i]);
                    }
                }
            }
            questingEnabled = false;

        }

        /// <summary>
        ///Assign selected quest to hero and start calculating the waypoints
        /// </summary>
        public void AssignQuest(int index, GameObject button)
        {
            if (Player.Singleton.Inventory.PlayerMoney >= collectionGoals[index].Reward)
            {

                Debug.Log("assign quest: " + index);
                //Debug.Log("goes in");
                if (collectionGoal != null && questingEnabled)
                {
                    Debug.Log("AssignQuest quest and collectiongoal != null");
                    if (collectionGoal.Completed || !collectionGoal.InProgress || collectionGoal.collectables.Count <= 0 || (Player.Singleton.Inventory.PlayerMoney >= collectionGoal.Reward))
                    {
                        //{
                        //Debug.Log("AssignedQuest: " + index);
                        List<Collectables> tempCollectables = new List<Collectables>();
                        for (int i = 0; i < collectionGoals[index].collectables.Count; i++)
                        {
                            tempCollectables.Add(collectionGoals[index].collectables[i].Clone());
                        }
                        //if (index == 0)
                        //{
                        //    Button button = quest1Prefab.GetComponentInChildren<Button>();


                        //Button questButton = button/*.GetComponent<QuestButton>()*/;

                        collectionGoal.FillList(tempCollectables, collectionGoals[index].ReputationGain, collectionGoals[index].Reward, button);


                        //}
                        //if(index == 1)
                        //{
                        //    Button button = quest2Prefab.GetComponentInChildren<Button>();
                        //    QuestButton questButton = button.GetComponent<QuestButton>();
                        //    collectionGoal.FillList(tempCollectables, questButton);
                        //}
                        //if (index == 2)
                        //{
                        //    Button button = quest3Prefab.GetComponentInChildren<Button>();
                        //    QuestButton questButton = button.GetComponent<QuestButton>();
                        //    collectionGoal.FillList(tempCollectables, questButton);
                        //}
                        collectionGoal.CalcNextWaypoint();

                    }
                }
            }

        }
    }
}
