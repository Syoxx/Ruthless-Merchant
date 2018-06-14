//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public struct SpawnInfo
    {
        public Transform SpawnObject;
        public int Count;

        public SpawnInfo(Transform spawnObject, int count)
        {
            SpawnObject = spawnObject;
            Count = count;
        }
    }
}
