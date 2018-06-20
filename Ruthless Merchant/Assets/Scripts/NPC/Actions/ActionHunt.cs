//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionHunt : ActionMove
    {
        private Fighter fighter;
        public override void EndAction()
        {
            parent.Reacting = false;
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            fighter = parent as Fighter;
            parent.Reacting = true;
            base.StartAction(parent, other);
        }

        public override void Update(float deltaTime)
        {
            if(parent.CurrentReactTarget == null || other != parent.CurrentReactTarget.gameObject)
            {
                parent.SetCurrentAction(new ActionIdle(), null);
                return;
            }

            float distance = Vector3.Distance(other.transform.position, parent.transform.position);
            if (distance <= agent.baseOffset)
            {
                agent.isStopped = true;
                parent.Waypoints.Clear();

                parent.SetCurrentAction(new ActionIdle(), null);
            }
            else if (fighter.HuntDistance >= distance)
            {
                parent.Reacting = true;
                parent.AddNewWaypoint(new Waypoint(other.transform.position, true, 0), true);
                parent.ChangeSpeed(NPC.SpeedType.Run);
            }
            parent.RotateToNextTarget(other.transform.position, false);

            base.Update(deltaTime);
        }
    }
}
