using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{

    public class ItemRespawnLogic : MonoBehaviour {

        #region Private Fields

        [Header("Item/Material/Ingredient/Quest Item which should be spawned")]
        [SerializeField]
        [Tooltip("Place desired Prefab here")]
        private GameObject itemToSpawn;

        [Header("Number of Items which are simultaneously spawned")]
        [SerializeField]
        private int maxNrOfItems;

        [Header("Time between Respawns in Seconds")]
        [SerializeField]
        private float respawnTime = 5;

        [SerializeField]
        private GameObject[] spawnLocations;

        private Transform spawnPosition;
        private GameObject spawnedItem;
        private static System.Random rnJesus = new System.Random();
        private float currentTimer;
        private int currentlyActiveItems;
        private List<GameObject> emptySpawners = new List<GameObject>();
        #endregion

        #region Gameplay Loop
        // Use this for initialization
        void Start() {
            currentTimer = respawnTime;
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
        }

        #endregion

        #region Methods

        private void CheckItemsInRadius()
        {
            foreach (var item in spawnLocations)
            {
                if (item.GetComponent<ContainingItemInformation>().CheckContainingItem(itemToSpawn))
                    currentlyActiveItems++;
                else
                    emptySpawners.Add(item);
            }

            if (currentlyActiveItems < maxNrOfItems)
            {
                int nrOfItemsToSpawn = maxNrOfItems - currentlyActiveItems;

                for (int i = 0; i < nrOfItemsToSpawn; i++)
                {
                    SpawnItem(emptySpawners);
                }
            }
        }

        private void SpawnItem(List<GameObject> eligableSpawners)
        {
            GameObject[] eligableSpawnersArray = eligableSpawners.ToArray();
            GameObject selectedSpawnLocation = GetSpawnLocation(eligableSpawnersArray);

            spawnPosition = selectedSpawnLocation.transform;
            spawnedItem = Instantiate(itemToSpawn, spawnPosition.position, UnityEngine.Random.rotation);
        }

        public GameObject GetSpawnLocation(GameObject[] spawnArray)
        {
            int spawnLocationIdentifier = rnJesus.Next(0, spawnArray.Length);
            return spawnArray[spawnLocationIdentifier];
        }

        private void OnTimedEvent()
        {
            Debug.Log("time Expired");
            CheckItemsInRadius();
        }

        #endregion
    }
}
