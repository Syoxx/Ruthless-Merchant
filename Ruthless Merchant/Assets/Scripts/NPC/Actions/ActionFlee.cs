//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionFlee : ActionMove
    {

        /// <summary>
        /// Action flee
        /// </summary>
        public ActionFlee() : base(ActionPriority.High)
        {

        }

        /// <summary>
        /// Action flee
        /// </summary>
        /// <param name="priority">Action priority</param>
        public ActionFlee(ActionPriority priority) : base(priority)
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
            base.EndAction();
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Reacting = true;
            parent.ChangeSpeed(NPC.SpeedType.Flee);
            Vector3 direction = other.transform.position - parent.transform.position;
            direction.Normalize();

            parent.AddNewWaypoint(new Waypoint(direction * 20, true, 0), true);
            base.StartAction(parent, other);
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
