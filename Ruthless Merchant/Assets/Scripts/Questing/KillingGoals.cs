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

        private bool questingEnabled;
        private KillGoal killGoal;

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag("NPC"))
                killGoal = other.GetComponent<KillGoal>();
        }
        
        public void AssignQuest(int localIndex, GameObject button)
        {
            if (killGoal != null && questingEnabled)
            {
                Debug.Log("AssignQuest quest and collectiongoal != null");
                if (killGoal.Completed || !killGoal.InProgress)
                {
                    //{
                    //Debug.Log("AssignedQuest: " + index);
                    Debug.Log(KillGoalsList[localIndex].RequiredAmount);
                    killGoal.SetTargetList(button, KillGoalsList[localIndex].RequiredAmount);

                    killGoal.CalcNextWayPoint();

                }
            }
        }

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
                        
                        buttons.Add(questButton);

                        Button btn = questButton.GetComponent<Button>();
                        btn.onClick.AddListener(delegate { AssignQuest(localIndex, questButton); });
                    }
                }
            }


        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = false;
            }
        }
    }
}
