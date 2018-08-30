using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CityTrigger : CaptureTrigger
    {
        private ObjectSpawner spawner;
       
        [Header("Hero spawn settings")]
        [SerializeField]
        private Transform HeroPrefab;

        [SerializeField, Range(1, 100)]
        private int startLevel = 1;

        [SerializeField, Range(1, 100)]
        private int maxLevel = 10;

        private int spawnedHeros = 0;
        private int currentLevel = 1;

        protected override void Start()
        {
            spawner = GetComponent<ObjectSpawner>();
            foreach (CaptureTrigger outpost in outpostsToFreidenker)
            {
                outpost.OnHeroRemoved += CaptureTriggerF_OnHeroRemoved;
            }

            foreach (CaptureTrigger outpost in outpostsToImperialist)
            {
                outpost.OnHeroRemoved += CaptureTriggerI_OnHeroRemoved;
            }
        }

        protected override void Update()
        {
        }

        protected override void CaptureTriggerF_OnHeroRemoved(object sender, EventArgs e)
        {
            SpawnHero(sender);
        }

        protected override void CaptureTriggerI_OnHeroRemoved(object sender, EventArgs e)
        {
            SpawnHero(sender);
        }

        private void SpawnHero(object sender)
        {
            if (sender != null && sender is CaptureTrigger)
            {
                CaptureTrigger outpost = sender as CaptureTrigger;
                if(outpost.Owner == owner)
                {
                    Transform newHero = spawner.ForceSpawn(HeroPrefab);
                    Hero heroScript = newHero.GetComponent<Hero>();
                    outpost.Hero = heroScript;
                    heroScript.Level = currentLevel;
                    TryIncreaseLevel();
                }
            }
        }

        /// <summary>
        /// Tries to increase the spawn level of heros
        /// </summary>
        private void TryIncreaseLevel()
        {
            if (currentLevel < maxLevel)
            {
                if (++spawnedHeros == currentLevel)
                {
                    currentLevel++;
                    spawnedHeros = 0;
                }
            }
        }
    }
}