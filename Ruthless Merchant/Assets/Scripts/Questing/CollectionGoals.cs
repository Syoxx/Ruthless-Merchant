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

        //[SerializeField]
        //public Transform QuestParent;
        //[SerializeField]
        //private GameObject quest1Prefab;
        //[SerializeField]
        //private GameObject quest2Prefab;
        //[SerializeField]
        //private GameObject quest3Prefab;

        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
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

                    
                    GameObject questButton = Instantiate(buttonPrefab, buttonParent) as GameObject;
                    QuestDisplayedData questData = questButton.GetComponent<QuestDisplayedData>();
                    questData.Name.text = collectionGoals[i].QuestTitle;
                    questData.Description.text = collectionGoals[i].Description;
                    questData.Reward.text = "Reward: " + collectionGoals[i].Reward.ToString() + "$";
                    buttons.Add(questButton);

                    Button btn = questButton.GetComponent<Button>();
                    btn.onClick.AddListener(delegate { AssignQuest(localIndex, btn); });
                }

                //if(quest1Prefab!=null)
                //    quest1Prefab.gameObject.SetActive(true);
                //if (quest2Prefab != null)
                //    quest2Prefab.gameObject.SetActive(true);
                //if (quest3Prefab != null)
                //    quest3Prefab.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {   
                questingEnabled = false;

                for (int i = 0; i < buttons.Count; i++)
                {
                    if(!buttons[i].GetComponent<QuestButton>().inProgress)
                        Destroy(buttons[i]);
                }
            }
            //questingEnabled = false;

        }
        private void Update()
        {

        }
        public void AssignQuest(int index, Button button)
        {
            
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
                    QuestButton questButton = button.GetComponent<QuestButton>();
                    collectionGoal.FillList(tempCollectables, questButton);
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
                //collectionGoal.CalcNextWaypoint();                
            }
        }


    }
}
