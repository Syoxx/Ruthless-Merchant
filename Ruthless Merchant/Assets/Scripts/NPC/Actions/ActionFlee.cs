//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionFlee : ActionMove
    {
        /*TODO:
         * save position on start
         * return to start position (only if idle for 5 seconds)
         */

        public ActionFlee() : base(ActionPriority.High)
        {

        }

        public ActionFlee(ActionPriority priority) : base(priority)
        {

        }

        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Reacting = true;
            parent.ChangeSpeed(NPC.SpeedType.Flee);
            Vector3 direction = other.transform.position - parent.transform.position;
            direction.Normalize();

            parent.AddNewWaypoint(new Waypoint(direction * 20, true, 0), true);
            base.StartAction(parent, other);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
