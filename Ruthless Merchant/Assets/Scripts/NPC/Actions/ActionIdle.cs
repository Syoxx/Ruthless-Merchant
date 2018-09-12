//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionIdle : ActionNPC
    {
        private float elapsedIdleTime;

        /// <summary>
        /// Action Idle
        /// </summary>
        public ActionIdle() : base (ActionPriority.None)
        {

        }

        /// <summary>
        /// Action Idle
        /// </summary>
        /// <param name="priority">Action priority</param>
        public ActionIdle(ActionPriority priority) : base(priority)
        {

        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            elapsedIdleTime = 0;
            base.StartAction(parent, other);
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public override void Update(float deltaTime)
        {
            elapsedIdleTime += deltaTime;
            
        }
    }
}
