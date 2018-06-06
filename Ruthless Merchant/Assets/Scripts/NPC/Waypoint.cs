using UnityEngine;

namespace RuthlessMerchant
{
    [System.Serializable]
    public struct Waypoint
    {
        public Vector3 Position;
        public Transform Transform;
        public bool RemoveOnReached;

        [Range(-1.0f, 30.0f)]
        public float WaitTime;

        public Waypoint(Vector3 position, bool removeOnReached, float waitTime)
        {
            Position = position;
            RemoveOnReached = removeOnReached;
            Transform = null;
            WaitTime = waitTime;
        }

        public Waypoint(Transform transform, bool removeOnReached, float waitTime)
        {
            Position = transform.position;
            RemoveOnReached = removeOnReached;
            Transform = transform;
            WaitTime = waitTime;
        }

        public Vector3 GetPosition()
        {
            if (Transform == null)
                return Position;

            return Transform.position;
        }
    }
}
