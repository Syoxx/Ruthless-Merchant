﻿using UnityEngine;

namespace RuthlessMerchant
{
    public class MonsterSpawnEmitter : SpawnEmitter
    {
        [SerializeField, Range(0, 1000), Tooltip("Minimum spawn interval")]
        private float minIntervall;
        [SerializeField, Range(0, 1000), Tooltip("Maximum spawn interval")]
        private float maxIntervall;

        [SerializeField, Tooltip("Start interval when monster died")]
        private bool startTimeOnMonsterDeath = true;

        private Monster monster = null;

        protected override void Start()
        {
            base.Start();
            intervall = Random.Range(minIntervall, maxIntervall);
            spawner.OnObjectSpawned += Spawner_OnObjectSpawned;
        }

        protected override void Update()
        {
            if (startTimeOnMonsterDeath)
            {
                if(monster == null)
                    base.Update();
            }
            else
            {
                base.Update();
            }
        }

        private void Spawner_OnObjectSpawned(object sender, SpawnArgs e)
        {
            monster = e.SpawnedObject.GetComponent<Monster>();
            monster.SetCurrentAction(new ActionIdle(), null, false, true);
            intervall = Random.Range(minIntervall, maxIntervall);
        }
    }
}
