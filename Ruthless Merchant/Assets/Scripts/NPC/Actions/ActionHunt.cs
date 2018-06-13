using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionHunt : ActionMove
    {
        private Fighter fighter;
        public override void EndAction()
        {
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            fighter = parent as Fighter;
            base.StartAction(parent, other);
        }

        public override void Update(float deltaTime)
        {
            float distance = Vector3.Distance(other.transform.position, parent.transform.position);
            if (distance <= agent.baseOffset + 0.5f)
            {
                agent.isStopped = true;
                parent.Waypoints.Clear();

                parent.CurrentAction = null;
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
