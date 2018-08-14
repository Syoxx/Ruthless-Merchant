using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class CollectionGoals : MonoBehaviour {

        [SerializeField]
        private List<CollectionGoal> collectionGoals;
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

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                collectionGoal = other.gameObject.GetComponent<CollectionGoal>();

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    collectionGoal.FillList(collectionGoals[0].collectables);
                    collectionGoal.CalcNextWaypoint();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    collectionGoal.FillList(collectionGoals[1].collectables);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    collectionGoal.FillList(collectionGoals[2].collectables);
                }
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {

            }
        }

    }
}
