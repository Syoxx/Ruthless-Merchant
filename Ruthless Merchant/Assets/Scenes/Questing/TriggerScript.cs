using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class TriggerScript : MonoBehaviour
    {
        //[System.Serializable]
        [SerializeField]
        List<Collectables> CollectablesInTrigger;

        CollectionGoal collectionGoal;
        
        GameObject TriggeredObject;

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

        private void OnTriggerEnter(Collider other)
        {
            collectionGoal = other.GetComponent<CollectionGoal>();
            collectionGoal.FillList(CollectablesInTrigger);

        }
    }
    [System.Serializable]
    public class Collectables
    {
        public Material material;
        public int requiredAmount;
        //[HideInInspector]
        public int currentAmount;
        public bool completed;


        public Collectables(Material Material, int RequiredAmount, int CurrentAmount, bool Completed)
        {
            material = Material;
            requiredAmount = RequiredAmount;
            currentAmount = CurrentAmount;
            completed = Completed;
        }
    }
}
