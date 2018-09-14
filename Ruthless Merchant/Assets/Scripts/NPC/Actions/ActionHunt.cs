//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{
    public class ActionHunt : ActionMove
    {

        private Fighter fighter;

        /// <summary>
        /// Action Hunt
        /// </summary>
        public ActionHunt() : base(ActionPriority.Medium)
        {

        }

        /// <summary>
        /// Action Hunt
        /// </summary>
        /// <param name="priority">Action priority</param>
        public ActionHunt(ActionPriority priority) : base(priority)
        {

        }

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            base.EndAction(executeEnd);
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            fighter = parent as Fighter;
            parent.Reacting = true;
            base.StartAction(parent, other);
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public override void Update(float deltaTime)
        {
            if (parent.CurrentReactTarget == null || other != parent.CurrentReactTarget.gameObject)
            {
                parent.SetCurrentAction(new ActionIdle(), null, true);
                return;
            }

            float distance = Vector3.Distance(other.transform.position, parent.transform.position);
            if (distance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                parent.Waypoints.Clear();
                parent.SetCurrentAction(new ActionIdle(), null, true);
            }
            else if (fighter.HuntDistance >= distance)
            {
                if (parent.Waypoints.Count == 0 || !parent.Reacting || Vector3.Distance(other.transform.position, parent.Waypoints[0].GetPosition()) > 1.0f)
                {
                    parent.Reacting = true;
                    parent.AddNewWaypoint(new Waypoint(other.transform.position, true, 0), true);
                    parent.ChangeSpeed(NPC.SpeedType.Run);
                    waypointIndex = 0;
                    NavMeshPath path = new NavMeshPath();
                    if(agent.CalculatePath(other.transform.position, path))
                    {
                        agent.SetPath(path);
                    }
                }
            }
            parent.RotateToNextTarget(other.transform.position, false);
            
            base.Update(deltaTime);
        }
    }
}
