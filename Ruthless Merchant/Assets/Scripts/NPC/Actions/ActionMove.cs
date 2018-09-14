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
        protected Animator animator;

        /// <summary>
        /// Action move
        /// </summary>
        public ActionMove() : base(ActionPriority.Low)
        {

        }

        /// <summary>
        /// Action move
        /// </summary>
        /// <param name="priority">Action priority</param>
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
                if (value >= 0 && ((parent.Waypoints.Count > 0 && value < parent.Waypoints.Count) || (parent.Waypoints.Count == 0 && value <= parent.Waypoints.Count)))
                    waypointIndex = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            base.StartAction(parent, other);
            animator = parent.gameObject.GetComponent<Animator>();

            if (agent == null)
                agent = parent.GetComponent<NavMeshAgent>();

            if (other != null)
                parent.AddNewWaypoint(new Waypoint(other.transform, true, 0), true);

            if (parent.Waypoints.Count > 0)
            {
                SetDestination();
            }
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public override void Update(float deltaTime)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                elapsedWaitTime += deltaTime;
                SetDestination();
            }
            else
            {               
                agent.isStopped = false;
                parent.RotateToNextTarget(agent.steeringTarget + parent.transform.forward, false);
            }
            MoveAnimation();
            DeadlockFixer();
        }

        private float elapsedDeadlockTime = 0.0f;
        private float maxDeadlockTime = 3.0f;
        private bool checkDeadlock = false;
        private void DeadlockFixer()
        {
            if (checkDeadlock)
            {
                if (agent.pathPending)
                {
                    elapsedDeadlockTime += Time.deltaTime;
                    if (elapsedDeadlockTime > maxDeadlockTime)
                    {
                        NavMeshPath path = new NavMeshPath();
                        if (agent.CalculatePath(parent.CurrentWaypoint.Value.GetPosition(), path))
                        {
                            agent.SetPath(path);
                        }
                        checkDeadlock = false;
                    }
                }
                else
                {
                    checkDeadlock = false;
                    elapsedDeadlockTime = 0.0f;
                }
            }
        }

        private void MoveAnimation()
        {
            if (agent.velocity != Vector3.zero)
            {
                animator.SetBool("IsWalking",true);
            }
            else
            {
                animator.SetBool("IsWalking",false);
            }
        }

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
            animator.SetBool("IsWalking", false);
            base.EndAction(executeEnd);
        }

        /// <summary>
        /// Sets the next waypoint
        /// </summary>
        protected void SetDestination()
        {
            if (parent.Waypoints.Count <= 0 || waypointIndex < 0 || waypointIndex >= parent.Waypoints.Count)
            {
                waypointIndex = 0;
                parent.CurrentWaypoint = null;
                parent.SetCurrentAction(new ActionIdle(), null, true);
                return;
            }

            Waypoint waypoint = parent.Waypoints[waypointIndex];
            if (elapsedWaitTime >= waypoint.WaitTime - 1.0f)
                parent.RotateToNextTarget(waypoint.GetPosition(), false);

            if (elapsedWaitTime >= waypoint.WaitTime)
            {
                elapsedWaitTime = 0.0f;
                agent.SetDestination(waypoint.GetPosition());
                checkDeadlock = true;
                elapsedDeadlockTime = 0.0f;

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
