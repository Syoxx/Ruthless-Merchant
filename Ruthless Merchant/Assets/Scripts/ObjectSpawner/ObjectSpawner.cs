//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

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
        protected virtual void Start()
        {
            blockingObjects = new List<Collider>();
            spawnQueue = new Queue<SpawnInfo?>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (blockingObjects !=null && spawnQueue != null && blockingObjects.Count == 0)
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

        /// <summary>
        /// Adds an object to the spawn queue and spawns a given count of these objects
        /// </summary>
        /// <param name="spawnObject">Object to spawn</param>
        /// <param name="count">Spawn count</param>
        public void Spawn(Transform spawnObject, int count)
        {
            spawnQueue.Enqueue(new SpawnInfo(spawnObject, count));
        }

        /// <summary>
        /// Spawns a object
        /// </summary>
        /// <param name="spawnObject">Object to spawn</param>
        /// <returns>Returns the spawned object</returns>
        public virtual Transform ForceSpawn(Transform spawnObject)
        {
            return Instantiate(spawnObject, transform.position, transform.rotation);
        }

        /// <summary>
        /// Spawns a object
        /// </summary>
        /// <param name="spawnObject">Object to spawn</param>
        /// <param name="count">Numbers to spawn</param>
        /// <returns>Returns all spawned objects</returns>
        public virtual Transform[] ForceSpawn(Transform spawnObject, int count)
        {
            Transform[] spawnedObjects = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                spawnedObjects[i] = Instantiate(spawnObject, transform.position, transform.rotation);
            }

            return spawnedObjects;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("NPC"))
                blockingObjects.Add(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            blockingObjects.Remove(other);
        }
    }
}