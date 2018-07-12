using UnityEngine;
using System;

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

        [SerializeField]
        [Tooltip("Maximum Distance allowed from Iron Deposit to allow Iron to spawn")]
        [Range(0.2f, 5f)]
        private float maxSpawnDistance;

        [SerializeField]
        [Tooltip("Check to enable Rare Item Drop")]
        private bool allowRareItemDrop = false;

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
        private Vector3 ironForce;
        #endregion



        #region Gameplay Loop
        // Use this for initialization
        public override void Start()
        {
            numberOfIrons = UnityEngine.Random.Range(minNrOfIron, maxNrOfIron);
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
            if (allowRareItemDrop)
                DropRareItem();

            for (int i = 1; i <= numberOfIrons; i++)
            {
                SpawnItem(ironPrefab, true, i);
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
        private void SpawnItem(GameObject itemPrefab, bool isIron, int nrOfItem)
        {
            spawnPosition = this.transform;
            //spawnPosition.position += new Vector3(Random.Range(spawnRandomizerMin, spawnRandomizerMax), 0, Random.Range(spawnRandomizerMin, spawnRandomizerMax));
            GameObject spawnedIron = Instantiate(itemPrefab,spawnPosition.position, spawnPosition.rotation);
            spawnedIron.GetComponent<Rigidbody>().AddForce(CalculateForce(isIron, nrOfItem));
        }

        /// <summary>
        /// Calculates if a Rare Item Drops and Initiates Spawning
        /// </summary>
        private void DropRareItem()
        {
            float randomRoll = UnityEngine.Random.Range(0f, 100f);
            if (randomRoll <= rareItemDropChance)
                SpawnItem(rareItemPrefab, false, 1);
        }

        private Vector3 CalculateForce(bool isIron, int nrOfItem)
        {
            //double spawnDegree = (360 / numberOfIrons) * nrOfItem;
            //ironForce.x = (float)(maxSpawnDistance * Math.Cos(spawnDegree));
            //if (ironForce.x < 0)
            //    ironForce.x = ironForce.x * -1;
            //ironForce.z = (float)(maxSpawnDistance * Math.Sin(spawnDegree));
            //if (ironForce.z < 0)
            //    ironForce.z = ironForce.z * -1;
            ironForce.x = 100f;
            if (nrOfItem % 2 == 0)
                ironForce.x = ironForce.x * -1;
            ironForce.z = 100f;
            ironForce.y = 1f;
            return ironForce;
        }
        #endregion
    }
}
