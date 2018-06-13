using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class SpawnArgs : EventArgs
    {
        public Transform SpawnedObject;

        public SpawnArgs(Transform spawnedObject)
        {
            SpawnedObject = spawnedObject;
        }
    }

}
