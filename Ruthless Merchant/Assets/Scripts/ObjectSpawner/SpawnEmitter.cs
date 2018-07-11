//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

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
        protected int count = 1;

        protected ObjectSpawner spawner;
        private float elapsedTime = 0f;

        protected virtual void Start()
        {
            spawner = GetComponent<ObjectSpawner>();
            elapsedTime = intervall;
        }

        protected virtual void Update()
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= intervall)
            {
                spawner.Spawn(spawnObject, count);
                elapsedTime = 0;
                SpawnQueued();
            }
        }

        protected virtual void SpawnQueued()
        {

        }
    }
}
