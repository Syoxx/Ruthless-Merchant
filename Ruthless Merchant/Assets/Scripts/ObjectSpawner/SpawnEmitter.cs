using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class SpawnEmitter : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnObject = null;

        [SerializeField]
        [Range(0, 1000)]
        private float intervall = 1;

        [SerializeField]
        [Range(1, 1000)]
        private int count = 1;

        [SerializeField]
        private string[] possiblePaths;

        private ObjectSpawner spawner;
        private float elapsedTime = 0f;

        private void Start()
        {
            spawner = GetComponent<ObjectSpawner>();
            spawner.OnObjectSpawned += Spawner_OnObjectSpawned;
        }

        private void Spawner_OnObjectSpawned(object sender, SpawnArgs e)
        {
            if (possiblePaths != null && possiblePaths.Length > 0)
            {
                NPC npc = e.SpawnedObject.GetComponent<NPC>();
                npc.SetRandomPath(possiblePaths, 3);
            }
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= intervall)
            {
                spawner.Spawn(spawnObject, count);
                elapsedTime = 0;
            }
        }
    }
}
