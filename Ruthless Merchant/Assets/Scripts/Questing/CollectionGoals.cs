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
        [SerializeField]
        private GameObject quest1Prefab;
        [SerializeField]
        private GameObject quest2Prefab;
        [SerializeField]
        private GameObject quest3Prefab;

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
                //Instantiate(quest1Prefab, QuestParent);
                //Instantiate(quest2Prefab, QuestParent);
                //Instantiate(quest3Prefab, QuestParent);
                ////quest1Prefab.GetComponent<Button>().onClick.AddListener(delegate { AssignQuest(0); });
                ////quest1Prefab.GetComponent<Button>().onClick.AddListener(delegate { AssignQuest(1); });
                ////quest1Prefab.GetComponent<Button>().onClick.AddListener(delegate { AssignQuest(2); });
                //quest1Prefab.GetComponentInChildren<Button>().onClick.AddListener(() => AssignQuest(0));
                if(quest1Prefab!=null)
                    quest1Prefab.gameObject.SetActive(true);
                if (quest2Prefab != null)
                    quest2Prefab.gameObject.SetActive(true);
                if (quest3Prefab != null)
                    quest3Prefab.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            //questingEnabled = false;
        }
        private void Update()
        {

        }
        public void AssignQuest(int index)
        {
            Debug.Log("goes in");
            if (collectionGoal != null && questingEnabled)
            {
                Debug.Log("AssignedQuest");
                    List<Collectables> tempCollectables = new List<Collectables>();
                    for (int i = 0; i < collectionGoals[index].collectables.Count; i++)
                    {
                        tempCollectables.Add(collectionGoals[index].collectables[i].Clone());
                    }
                    collectionGoal.FillList(tempCollectables);
                    collectionGoal.CalcNextWaypoint();
                
            }
        }


    }
}
