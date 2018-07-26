using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CityTrigger : CaptureTrigger
    {
        private ObjectSpawner spawner;

        [SerializeField]
        private Transform HeroPrefab;

        protected override void Start()
        {
            spawner = transform.GetComponent<ObjectSpawner>();
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
                }
            }
        }
    }
}