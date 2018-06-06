using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Fighter : NPC
    {
        private int huntDistance;

        public bool PatrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        public override void Start()
        {
            base.Start();

            if (PatrolActive)
            {
                PatrolActive = false;
                PatrolPoints = GetRandomPath(PossiblePatrolPaths, false, 3);
                Patrol();
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public void SearchTarget()
        {
            throw new System.NotImplementedException();
        }

        public void Hunt()
        {
            throw new System.NotImplementedException();
        }

        public void TryAttack()
        {
            throw new System.NotImplementedException();
        }

        public void Patrol()
        {
            if (!PatrolActive)
            {
                PatrolActive = true;
                float minDistance = float.MaxValue;
                int nearestIndex = waypoints.Count;
                for (int i = 0; i < PatrolPoints.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, PatrolPoints[i].GetPosition());
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        nearestIndex = waypoints.Count;
                    }
                    waypoints.Add(PatrolPoints[i]);
                }

                waypointIndex = nearestIndex;
            }
        }

        public void AbortPatrol()
        {
            if (PatrolActive)
            {
                PatrolActive = false;
                for (int i = 0; i < PatrolPoints.Length; i++)
                {
                    waypoints.Remove(PatrolPoints[i]);
                }

                waypointIndex = 0;
            }
        }

        public override void Flee()
        {
            throw new System.NotImplementedException();
        }
    }
}