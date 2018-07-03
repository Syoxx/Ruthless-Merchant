using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{

    public class IronDeposit : InteractiveWorldObject
    {
        #region Private Fields
        [SerializeField]
        [Tooltip("Insert Item Prefab for the desired spawned Item here")]
        private GameObject ironPrefab;

        [SerializeField]
        [Tooltip("Insert Item Prefab for the desired spawned Rare Item here")]
        private GameObject rareItemPrefab;

        [Header("Irons in Deposits")]
        [Tooltip("Number of Irons in Deposit will be Randomized between these to Variables")]
        [SerializeField]
        private int minNrOfIron = 1, maxNrOfIron = 3;

        [Header("Random Spawn Position")]
        [SerializeField]
        [Tooltip("Nearest and Furstest Possible Spawn Location for Iron relative to the IronDeposit")]
        private int ironSpawnRandomizerMin = 1, ironSpawnRandomizerMax = 5;

        [Header("Rare Items")]
        [SerializeField]
        [Tooltip("Chance to Drop a rare Item")]
        [Range(0, 100)]
        private float rareItemDropChance = 20;


        private int numberOfIrons;
        private Transform spawnPosition;
        #endregion

        

        #region Gameplay Loop
        // Use this for initialization
        public override void Start()
        {
            numberOfIrons = Random.Range(minNrOfIron, maxNrOfIron);
        }

        // Update is called once per frame
        public override void Update()
        {

        }
        #endregion

        #region Methods
        public override void Interact(GameObject caller)
        {
            DropItems();
        }

        /// <summary>
        /// Initiates Spawning of dropped Items and Rare Items
        /// Destroys GameObject afterwards
        /// </summary>
        private void DropItems()
        {
            //DropRareItem();
            for (int i = 1; i <= numberOfIrons; i++)
            {
                if (i % 2 == 0)
                    SpawnItem(ironSpawnRandomizerMin, ironSpawnRandomizerMax, ironPrefab);
                else
                    SpawnItem(ironSpawnRandomizerMax * -1, ironSpawnRandomizerMin * -1, ironPrefab);
                if (i == numberOfIrons)
                    Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Spawns an Item at a Randomized Position near the GameObject
        /// </summary>
        /// <param name="spawnRandomizerMin"></param>
        /// <param name="spawnRandomizerMax"></param>
        /// <param name="itemPrefab"></param>
        private void SpawnItem(int spawnRandomizerMin, int spawnRandomizerMax, GameObject itemPrefab)
        {
            spawnPosition = this.transform;
            spawnPosition.position += new Vector3(Random.Range(spawnRandomizerMin, spawnRandomizerMax), 0, Random.Range(spawnRandomizerMin, spawnRandomizerMax));
            Instantiate(itemPrefab,spawnPosition.position, spawnPosition.rotation);
        }

        /// <summary>
        /// Calculates if a Rare Item Drops and Initiates Spawning
        /// </summary>
        private void DropRareItem()
        {
            float randomRoll = Random.Range(0f, 100f);
            if (randomRoll <= rareItemDropChance)
                SpawnItem(ironSpawnRandomizerMin, ironSpawnRandomizerMax, rareItemPrefab);
        }
        #endregion
    }
}
