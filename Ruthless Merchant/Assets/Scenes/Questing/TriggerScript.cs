using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class TriggerScript : MonoBehaviour
    {
        //[System.Serializable]
        [SerializeField]
        public List<Collectables> CollectablesInTrigger;

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
            //collectionGoal = other.GetComponent<CollectionGoal>();
            //collectionGoal.FillList(CollectablesInTrigger);
            //Debug.Log(other.gameObject);

        }
    }
    [System.Serializable]
    public class Collectables
    {
        //public Material material;
        public int requiredAmount;
        //[HideInInspector]
        public int currentAmount;
        public bool completed;
        public Texture icon;




        public Item item;
        public Collectables(Item Material, int RequiredAmount, int CurrentAmount, bool Completed, Texture Icon)
        {
            item = Material;
            requiredAmount = RequiredAmount;
            currentAmount = CurrentAmount;
            completed = Completed;
            icon = Icon;
        }

        public Collectables Clone()
        {
            return new Collectables(item, requiredAmount, currentAmount, completed, icon);
        }


        //public Collectables(Material Material, int RequiredAmount, int CurrentAmount, bool Completed, Texture Icon)
        //{
        //    material = Material;
        //    requiredAmount = RequiredAmount;
        //    currentAmount = CurrentAmount;
        //    completed = Completed;
        //    icon = Icon;
        //}

        //public Collectables Clone()
        //{
        //    return new Collectables(material, requiredAmount, currentAmount, completed, icon);
        //}
    }
}
