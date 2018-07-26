using UnityEngine;

namespace RuthlessMerchant
{ 
    public class ActionWander : ActionMove
    {
        public ActionWander() : base(ActionPriority.Low)
        {

        }

        public ActionWander(ActionPriority priority) : base(priority)
        {

        }

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.ChangeSpeed(NPC.SpeedType.Walk);
            parent.AddNewWaypoint(new Waypoint(parent.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f)), true, Random.Range(1, 5)), true);
            base.StartAction(parent, other);
        }

        public override void EndAction(bool executeEnd = true)
        {
            base.EndAction(executeEnd);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
