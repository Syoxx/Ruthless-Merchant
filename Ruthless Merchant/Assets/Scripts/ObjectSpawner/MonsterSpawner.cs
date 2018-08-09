using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class MonsterSpawner : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        [Tooltip("Tag of the Monsters")]
        private string monsterTag;

        [SerializeField]
        [Tooltip("Maximum Number of active Monsters")]
        private int maxActiveMonsters;

        [SerializeField]
        [Tooltip("Place Monster Prefab here")]
        private GameObject monsterPrefab;

        [SerializeField]
        [Tooltip("Number of Spawn Points and there Reference")]
        private List<GameObject> SpawnLocations;

        private List<GameObject> ListOfActiveMonsters = new List<GameObject>();
        private GameObject[] activeMonsters = null;
        private GameObject usedSpawnLocation;
        private System.Random rnJesus = new System.Random();
        private GameObject spawnedMonster;
        #endregion

        #region MonoBehavior Loop
        // Update is called once per frame
        //Checks if the desired Number of Monsters are active, Initiates Spawn if not
        void Update()
        {
            activeMonsters = CheckActiveMonsters(activeMonsters);
            if (activeMonsters.Length < maxActiveMonsters || activeMonsters == null)
                InitiateSpawn(activeMonsters, SpawnLocations);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks for all active Monsters
        /// </summary>
        /// <param name="monsterArray"></param>
        /// <returns></returns>
        public GameObject[] CheckActiveMonsters(GameObject[] monsterArray)
        {
            monsterArray = null;
            monsterArray = GameObject.FindGameObjectsWithTag(monsterTag);
            return monsterArray;
        }

        /// <summary>
        /// Initiates the Spawn of new Monsters
        /// Checks where the active Monsters where spawned and spawns a monster at an unused spawn location
        /// </summary>
        /// <param name="monsterArray"></param>
        /// <param name="spawnlocationList"></param>
        public void InitiateSpawn(GameObject[] monsterArray, List<GameObject> spawnlocationList)
        {
            if (monsterArray == null)
            {
                GameObject spawnLocation = GetSpawnLocation(spawnlocationList);
                spawnedMonster = Instantiate(monsterPrefab, spawnLocation.transform);
                spawnedMonster.GetComponent<Monster>().usedSpawnLocation = spawnLocation;
            }

            else
            {
                for (int i = 0; i < monsterArray.Length; i++)
                {
                    usedSpawnLocation = monsterArray[i].GetComponent<Monster>().usedSpawnLocation;
                    if (spawnlocationList.Contains(usedSpawnLocation))
                        spawnlocationList.Remove(usedSpawnLocation);
                }
                GameObject spawnLocation = GetSpawnLocation(spawnlocationList);
                spawnedMonster = Instantiate(monsterPrefab, spawnLocation.transform);
                spawnedMonster.GetComponent<Monster>().usedSpawnLocation = spawnLocation;
            }
        }

        /// <summary>
        /// chooses a random spawn Location for a new Monster
        /// </summary>
        /// <param name="eligableSpawns"></param>
        /// <returns></returns>
        public GameObject GetSpawnLocation(List<GameObject> eligableSpawns)
        {
            GameObject[] eligableSpawnsArray = eligableSpawns.ToArray();
            int randomSpawn = rnJesus.Next(0, eligableSpawnsArray.Length);
            return eligableSpawnsArray[randomSpawn];
        }
        #endregion
    }
}
