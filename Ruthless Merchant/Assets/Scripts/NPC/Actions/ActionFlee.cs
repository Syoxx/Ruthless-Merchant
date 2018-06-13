using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionFlee : ActionMove
    {
        public override void StartAction(NPC parent, GameObject other)
        {
            parent.ChangeSpeed(NPC.SpeedType.Run);

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
