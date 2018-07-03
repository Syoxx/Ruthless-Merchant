using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{

    public class IronDeposit : InteractiveWorldObject
    {
        [SerializeField]
        [Tooltip("Insert Item Prefab for the desired spawned Item here")]
        private GameObject ironPrefab;

        [Header("Irons in Deposits")]
        [Tooltip("Number of Irons in Deposit will be Randomized between these to Variables")]
        [SerializeField]
        private int minNrOfIron = 1, maxNrOfIron = 3;

        private int numberOfIrons;
        private Transform spawnPosition;
        [Header("Random Spawn Position")]
        [SerializeField]
        [Tooltip("Nearest and Furstest Possible Spawn Location for Iron relative to the IronDeposit")]
        private int ironSpawnRandomizerMin = 1, ironSpawnRandomizerMax = 5;
        // Use this for initialization
        public override void Start()
        {
            numberOfIrons = Random.Range(minNrOfIron, maxNrOfIron);
        }

        // Update is called once per frame
        public override void Update()
        {

        }

        public override void Interact(GameObject caller)
        {
            DropItems();
        }

        private void DropItems()
        {
            for (int i = 1; i <= numberOfIrons; i++)
            {
                if (i % 2 == 0)
                    SpawnIronItem(ironSpawnRandomizerMin, ironSpawnRandomizerMax);
                else
                    SpawnIronItem(ironSpawnRandomizerMax * -1, ironSpawnRandomizerMin * -1);
                if (i == numberOfIrons)
                    Destroy(this.gameObject);
            }
        }

        private void SpawnIronItem(int spawnRandomizerMin, int spawnRandomizerMax)
        {
            spawnPosition = this.transform;
            spawnPosition.position += new Vector3(Random.Range(spawnRandomizerMin, spawnRandomizerMax), 0, Random.Range(spawnRandomizerMin, spawnRandomizerMax));
            Instantiate(ironPrefab,spawnPosition.position, spawnPosition.rotation);
        }
    }
}
