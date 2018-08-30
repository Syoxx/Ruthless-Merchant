using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class CollectionGoals : MonoBehaviour
    {

        [SerializeField]
        private List<CollectionGoal> collectionGoals;
        private List<CollectionGoal> CollectionGoalClones = new List<CollectionGoal>();
        private bool questingEnabled;

        private CollectionGoal collectionGoal;

        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private Transform buttonParent;
        [SerializeField]
        private GameObject iconPrefab;


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

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("NPC") && collectionGoal == null)
            {
                collectionGoal = other.gameObject.GetComponent<CollectionGoal>();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = true;

                for (int i = 0; i < collectionGoals.Count; i++)
                {
                    int localIndex = i;

                    for (int k = 0; k < buttons.Count; k++)
                    {
                        if (buttons[k])
                        {
                            if (collectionGoals[i].QuestTitle == buttons[k].GetComponent<QuestDisplayedData>().Name.text)
                            {
                                Debug.Log("collectiongoal: " + i);
                                Debug.Log("button:" + k);
                                Debug.Log("dont instantiate");
                                i++;
                                break;
                            }
                        }
                    }
                    if (i < collectionGoals.Count)
                    {
                        Debug.Log("0 shouldnt inst: " + i);
                        GameObject questButton = Instantiate(buttonPrefab, buttonParent) as GameObject;
                        QuestDisplayedData questData = questButton.GetComponent<QuestDisplayedData>();
                        questData.Name.text = collectionGoals[i].QuestTitle;
                        questData.Description.text = collectionGoals[i].Description;
                        questData.Reward.text = "Reward: " + collectionGoals[i].Reward.ToString() + "$";
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
                    }
                }
            }
        }

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
            //questingEnabled = false;

        }
        private void Update()
        {

        }
        public void AssignQuest(int index, GameObject button)
        {
            Debug.Log("assign quest: " + index);
            //Debug.Log("goes in");
            if (collectionGoal != null && questingEnabled)
            {

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
                    collectionGoal.FillList(tempCollectables, button);


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
