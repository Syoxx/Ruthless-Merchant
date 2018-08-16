using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RuthlessMerchant
{
    public class QuestAdder : MonoBehaviour
    {
        CollectionGoal collectionGoal;

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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("NPC"))
            {
                Debug.Log("Collided");
                List<Collectables> collectables = collectionGoal.collectables;
                //collision.gameObject.AddComponent<CollectionGoal>();
                //CollectionGoal collectionGoals = collision.gameObject.GetComponent<CollectionGoal>();
                //////collectionGoal = collision.gameObject.AddComponent<CollectionGoal>();
                //collectionGoals.FillList(collectables);

                collectionGoal = collision.gameObject.GetComponent<CollectionGoal>();
                collectionGoal.FillList(collectables);
            }
        }
    }
}
