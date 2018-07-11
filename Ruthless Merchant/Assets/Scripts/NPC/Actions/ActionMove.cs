//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{
    public class ActionMove : ActionNPC
    {
        protected float elapsedWaitTime;
        protected int waypointIndex;
        protected NavMeshAgent agent;

        public ActionMove() : base(ActionPriority.Low)
        {

        }

        public ActionMove(ActionPriority priority) : base(priority)
        {

        }

        /// <summary>
        /// Current waypoint index
        /// </summary>
        public int WaypointIndex
        {
            get
            {
                return waypointIndex;
            }
            set
            {
                if (value >= 0 &&  ((parent.Waypoints.Count > 0 && value < parent.Waypoints.Count) || (parent.Waypoints.Count == 0 && value <= parent.Waypoints.Count)))
                    waypointIndex = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            base.StartAction(parent, other);
            agent = parent.GetComponent<NavMeshAgent>();
        }

        public override void Update(float deltaTime)
        {
            if (!agent.pathPending && agent.remainingDistance < agent.baseOffset)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                elapsedWaitTime += deltaTime;
                SetDestination();
            }
            else
            {
                agent.isStopped = false;
                parent.RotateToNextTarget(agent.steeringTarget + parent.transform.forward, true);
            }
        }

        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
            base.EndAction(executeEnd);
        }
        /// <summary>
        /// Sets the next waypoint
        /// </summary>
        private void SetDestination()
        {
            if (parent.Waypoints.Count <= 0 || waypointIndex < 0 || waypointIndex >= parent.Waypoints.Count)
            {
                waypointIndex = 0;
                parent.CurrentWaypoint = null;
                return;
            }

            Waypoint waypoint = parent.Waypoints[waypointIndex];
            if (elapsedWaitTime >= waypoint.WaitTime - 1.0f)
                parent.RotateToNextTarget(waypoint.GetPosition(), false);

            if (elapsedWaitTime >= waypoint.WaitTime)
            {
                elapsedWaitTime = 0.0f;
                agent.SetDestination(waypoint.GetPosition());
                agent.isStopped = false;
                agent.updateRotation = false;
                parent.CurrentWaypoint = waypoint;

                if (waypoint.RemoveOnReached)
                    parent.Waypoints.RemoveAt(waypointIndex);
                else
                    waypointIndex++;

                if (waypointIndex >= parent.Waypoints.Count)
                    waypointIndex = 0;
            }
        }
    }
}
