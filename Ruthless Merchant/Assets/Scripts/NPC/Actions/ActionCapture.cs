//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{
    public class ActionCapture : ActionIdle
    {
        private CaptureTrigger trigger;

        private NavMeshAgent agent;
        private float elapsedTime = 0.0f;
        private float maxMoveTime = 3.0f;
        /// <summary>
        /// Action capture is used to capture outpost
        /// </summary>
        public ActionCapture() : base(ActionPriority.Medium)
        {

        }

        /// <summary>
        /// Action capture is used to capture outpost
        /// </summary>
        /// <param name="priority">Action priority</param>
        public ActionCapture(ActionPriority priority) : base(priority)
        {

        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            if(other != null)
                trigger = other.GetComponent<CaptureTrigger>();

            agent = parent.GetComponent<NavMeshAgent>();

            base.StartAction(parent, other);
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public override void Update(float deltaTime)
        {
            if (trigger != null)
            {
                if (trigger.Owner == parent.Faction)
                {
                    parent.SetCurrentAction(new ActionIdle(), null, true, true);
                }
                else
                {
                    if (agent != null && !agent.isStopped)
                    {
                        elapsedTime += deltaTime;
                        if (elapsedTime >= maxMoveTime)
                        {
                            agent.isStopped = true;
                        }
                    }
                }
            }
            base.Update(deltaTime);
        }
    }
}
