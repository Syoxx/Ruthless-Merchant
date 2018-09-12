//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionCapture : ActionIdle
    {
        private CaptureTrigger trigger;

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
            }
            base.Update(deltaTime);
        }
    }
}
