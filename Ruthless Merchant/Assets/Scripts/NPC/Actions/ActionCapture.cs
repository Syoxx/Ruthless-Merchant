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

        public ActionCapture() : base(ActionPriority.Medium)
        {

        }

        public ActionCapture(ActionPriority priority) : base(priority)
        {

        }

        public override void StartAction(NPC parent, GameObject other)
        {
            if(other != null)
                trigger = other.GetComponent<CaptureTrigger>();

            base.StartAction(parent, other);
        }

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
