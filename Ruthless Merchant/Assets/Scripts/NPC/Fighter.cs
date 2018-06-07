using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Fighter : NPC
    {
        private int huntDistance;

        [SerializeField]
        private bool patrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        public bool PaartolActive
        {
            get { return patrolActive; }
        }

        public override void Start()
        {
            base.Start();

            if (patrolActive)
            {
                patrolActive = false;
                if(PossiblePatrolPaths != null && PossiblePatrolPaths.Length > 0)
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
            if (!patrolActive)
            {
                if (PatrolPoints != null)
                {
                    patrolActive = true;
                    float minDistance = float.MaxValue;
                    int nearestIndex = waypoints.Count;
                    for (int i = 0; i < PatrolPoints.Length; i++)
                    {
                        float distance = Vector3.Distance(transform.position, PatrolPoints[i].GetPosition());
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestIndex = waypoints.Count;
                        }
                        waypoints.Add(PatrolPoints[i]);
                    }

                    waypointIndex = nearestIndex;
                }
            }
        }

        public void AbortPatrol()
        {
            if (patrolActive)
            {
                patrolActive = false;
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