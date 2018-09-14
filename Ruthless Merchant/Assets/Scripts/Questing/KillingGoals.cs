using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class KillingGoals : MonoBehaviour
    {
        [SerializeField]
        private List<KillGoal> KillGoalsList;

        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private Transform buttonParent;
        private List<GameObject> buttons = new List<GameObject>();
        private CaptureTrigger outpostTrigger;
        private bool questingEnabled;
        private KillGoal killGoal;

        private void Start()
        {
            outpostTrigger = GetComponent<CaptureTrigger>();
        }

        /// <summary>
        /// Detects player in outpost and updates the quest availability if requirements are met
        /// </summary>
        /// <param name="other">
        /// Player collider
        /// </param>
        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag("NPC"))
                killGoal = other.GetComponent<KillGoal>();
            if (other.gameObject.CompareTag("Player"))
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i] != null)
                    {
                        if (buttons[i].GetComponent<QuestButton>().isDisabled)
                        {
                            if (Player.Singleton.Inventory.PlayerMoney >= KillGoalsList[i].Reward)
                            {
                                KillGoalsList[i].DefaultColor();
                            }
                        }
                    }

                }
            }
        }
        
        /// <summary>
        /// Assigns the quest when it gets clicked
        /// </summary>
        /// <param name="localIndex">
        /// Index of button
        /// </param>
        /// <param name="button">
        /// Button that was clicked
        /// </param>
        public void AssignQuest(int localIndex, GameObject button)
        {
            if (killGoal == null && !(outpostTrigger.IsHeroAway))
            {
                killGoal = outpostTrigger.Hero.GetComponent<KillGoal>();
            }

            if (killGoal != null && questingEnabled)
            {

                Debug.Log("AssignQuest quest and collectiongoal != null");
                if (killGoal.Completed || !killGoal.InProgress || (Player.Singleton.Inventory.PlayerMoney >= killGoal.Reward))
                {
                    //{
                    //Debug.Log("AssignedQuest: " + index);
                    Debug.Log(KillGoalsList[localIndex].RequiredAmount);
                    killGoal.SetTargetList(button, KillGoalsList[localIndex].RequiredAmount, KillGoalsList[localIndex].ReputationGain);

                    killGoal.CalcNextWayPoint();

                }


            }
        }

        /// <summary>
        /// Displays quest button when player is at the corresponding outpost
        /// </summary>
        /// <param name="other">
        /// Player collider
        /// </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = true;

                for (int i = 0; i < KillGoalsList.Count; i++)
                {
                    int localIndex = i;

                    for (int k = 0; k < buttons.Count; k++)
                    {
                        if (buttons[k])
                        {
                            if (KillGoalsList[i].QuestTitle == buttons[k].GetComponent<QuestDisplayedData>().Name.text)
                            {
                                Debug.Log("collectiongoal: " + i);
                                Debug.Log("button:" + k);
                                Debug.Log("dont instantiate");
                                i++;
                                break;
                            }
                        }
                    }
                    if (i < KillGoalsList.Count)
                    {
                        Debug.Log("0 shouldnt inst: " + i);
                        GameObject questButton = Instantiate(buttonPrefab, buttonParent) as GameObject;
                        QuestDisplayedData questData = questButton.GetComponent<QuestDisplayedData>();
                        questData.Name.text = KillGoalsList[i].QuestTitle;
                        questData.Description.text = KillGoalsList[i].Description;
                        questData.Reward.text = "Reward: " + KillGoalsList[i].Reward.ToString() + "$";
                        questData.ReputationGain.text = "Reputation: " + KillGoalsList[i].ReputationGain.ToString() + "%";

                        buttons.Add(questButton);

                        Button btn = questButton.GetComponent<Button>();
                        btn.onClick.AddListener(delegate { AssignQuest(localIndex, questButton); });

                    }
                }
            }


        }

        /// <summary>
        /// Removes quest button when player leaves the outpost
        /// </summary>
        /// <param name="other">
        /// Player collider
        /// </param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = false;

                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i])
                    {
                        if (!buttons[i].GetComponent<QuestButton>().inProgress)
                            Destroy(buttons[i]);
                    }
                }
            }
        }
    }
}
