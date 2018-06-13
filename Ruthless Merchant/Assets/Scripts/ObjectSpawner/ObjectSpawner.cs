using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ObjectSpawner : MonoBehaviour
    {
        private List<Collider> blockingObjects;

        private Queue<SpawnInfo?> spawnQueue;
        private int objectsToSpawn = 0;
        private Transform currentSpawnObject;

        public event EventHandler<SpawnArgs> OnObjectSpawned;

        // Use this for initialization
        private void Start()
        {
            blockingObjects = new List<Collider>();
            spawnQueue = new Queue<SpawnInfo?>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (blockingObjects.Count == 0)
            {
                if (objectsToSpawn == 0 && spawnQueue.Count > 0)
                {
                    SpawnInfo? info = spawnQueue.Dequeue();

                    if (info.HasValue)
                    {
                        objectsToSpawn = info.Value.Count;
                        currentSpawnObject = info.Value.SpawnObject;
                    }
                }

                if (objectsToSpawn > 0)
                {
                    Transform spawnedObject = ForceSpawn(currentSpawnObject, 1)[0];
                    objectsToSpawn--;

                    if (OnObjectSpawned != null)
                        OnObjectSpawned.Invoke(this, new SpawnArgs(spawnedObject));
                }
            }
        }

        public void Spawn(Transform spawnObject, int count)
        {
            spawnQueue.Enqueue(new SpawnInfo(spawnObject, count));
        }

        public Transform ForceSpawn(Transform spawnObject)
        {
            return Instantiate(spawnObject, transform.position, transform.rotation);
        }

        public Transform[] ForceSpawn(Transform spawnObject, int count)
        {
            Transform[] spawnedObjects = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                spawnedObjects[i] = Instantiate(spawnObject, transform.position, transform.rotation);
            }

            return spawnedObjects;
        }

        private void OnTriggerEnter(Collider other)
        {
            blockingObjects.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            blockingObjects.Remove(other);
        }
    }
}