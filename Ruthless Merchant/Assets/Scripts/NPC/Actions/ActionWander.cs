using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{ 
    public class ActionWander : ActionMove
    {
        private const float maxReachTime = 10f;
        private float elapsedReachTime = 0.0f;
        private float moveRange = 3;
        private int minWaitDuration = 1;
        private int maxWaitDuration = 3;

        public ActionWander() : base(ActionPriority.Low)
        {

        }

        public ActionWander(ActionPriority priority) : base(priority)
        {

        }

        public ActionWander(float moveRange, int minWaitDuration, int maxWaitDuration, ActionPriority priority) : base(priority)
        {
            this.moveRange = moveRange;
            this.minWaitDuration = minWaitDuration;
            this.maxWaitDuration = maxWaitDuration;
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            agent = parent.GetComponent<NavMeshAgent>();
            parent.ChangeSpeed(NPC.SpeedType.Walk);
            for (int i = 0; i < 10; i++)
            {
                Vector3 wanderTarget = parent.transform.position + new Vector3(Random.Range((moveRange * -0.5f), moveRange * 0.5f), 0, Random.Range(moveRange * -0.5f, moveRange * 0.5f));
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(wanderTarget, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    parent.AddNewWaypoint(new Waypoint(wanderTarget, true, Random.Range(minWaitDuration, maxWaitDuration)), true);
                    break;
                }
            }

            base.StartAction(parent, other);
        }
       
        public override void EndAction(bool executeEnd = true)
        {
            base.EndAction(executeEnd);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            elapsedReachTime += deltaTime;
            if(elapsedReachTime >= maxReachTime)
            {
                if(!agent.isStopped)
                {
                    parent.SetCurrentAction(new ActionIdle(), null, true, true);
                }
                else
                {
                    elapsedReachTime = 0;
                }
            }
        }
    }
}
