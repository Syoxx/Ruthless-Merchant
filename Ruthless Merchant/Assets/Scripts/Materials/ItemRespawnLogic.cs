using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RuthlessMerchant
{

    public class ItemRespawnLogic : MonoBehaviour {

        #region Private Fields

        [Header("Item/Material/Ingredient/Quest Item which should be spawned")]
        [SerializeField]
        [Tooltip("Place desired Prefab here")]
        private GameObject itemToSpawn;

        [Header("Radius in which Items are spawned/checked")]
        [SerializeField]
        private int spawnRadius;

        [Header("Number of Items which are simultaneously spawned")]
        [SerializeField]
        private int maxNrOfItems;

        [Header("Time between Respawns in Seconds")]
        [SerializeField]
        private float respawnTime = 5;

        [SerializeField]
        private List<GameObject> itemsInRadius;

        [SerializeField]
        private GameObject[] spawnLocations;

        private Transform spawnPosition;
        private GameObject spawnedItem;
        private bool isAccessable;
        private static System.Random rnJesus = new System.Random();
        private float currentTimer;
        private int currentlyActiveItems;
        #endregion

        #region Gameplay Loop
        // Use this for initialization
        void Start() {
            currentTimer = respawnTime;
            itemsInRadius = new List<GameObject>();
            gameObject.GetComponent<SphereCollider>().radius = spawnRadius;
        }

        // Update is called once per frame
        void Update()
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0.0f)
            {
                OnTimedEvent();
                currentTimer = respawnTime;
            }

            foreach (GameObject item in itemsInRadius)
            {
                if (item == null)
                    itemsInRadius.Remove(item);
            }
        }

        #endregion

        #region Methods

        private void CheckItemsInRadius()
        {
            foreach (var item in spawnLocations)
            {
                if (item.GetComponent<ContainingItemInformation>().CheckContainingItem(itemToSpawn))
                    currentlyActiveItems++;
            }

            if (currentlyActiveItems < maxNrOfItems)
            {
                int nrOfItemsToSpawn = maxNrOfItems - currentlyActiveItems;

                for (int i = 0; i < nrOfItemsToSpawn; i++)
                {
                    SpawnItem();
                }
            }
        }

        private void SpawnItem()
        {
            spawnPosition = GetSpawnPosition();
            spawnedItem = Instantiate(itemToSpawn, spawnPosition.position, UnityEngine.Random.rotation);
            isAccessable = CheckAccessibility(spawnedItem);

            if (!isAccessable)
            {
                Destroy(spawnedItem);
            }
        }

        private Transform GetSpawnPosition()
        {
            Transform newPosition;
            int indexNewPos = rnJesus.Next(0, spawnLocations.Length);
            if (spawnLocations[indexNewPos].GetComponent<ContainingItemInformation>().CheckContainingItem(itemToSpawn))
                newPosition = spawnLocations[indexNewPos].transform;
            return newPosition;
        }

        private bool CheckAccessibility(GameObject objectToCheck)
        {
            Vector3 raycastDirection = transform.position - objectToCheck.transform.position;
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, raycastDirection, out hitInfo);
            if (hitInfo.Equals(objectToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnTimedEvent()
        {
            Debug.Log("time Expired");
            CheckItemsInRadius();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == itemToSpawn.tag)
            {
                Debug.Log("item is inside collider");
                itemsInRadius.Add(other.gameObject);
            }
        }

        #endregion
    }
}
