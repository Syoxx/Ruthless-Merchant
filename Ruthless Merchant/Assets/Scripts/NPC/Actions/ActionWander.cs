using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{ 
    public class ActionWander : ActionMove
    {
        private const float maxReachTime = 10f;
        private float elapsedReachTime = 0.0f;

        public ActionWander() : base(ActionPriority.Low)
        {

        }

        public ActionWander(ActionPriority priority) : base(priority)
        {

        }

        public override void StartAction(NPC parent, GameObject other)
        {
            Debug.Log("Wander started");
            agent = parent.GetComponent<NavMeshAgent>();
            parent.ChangeSpeed(NPC.SpeedType.Walk);
            for (int i = 0; i < 10; i++)
            {
                Vector3 wanderTarget = parent.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f));
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(wanderTarget, path) && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                {
                    parent.AddNewWaypoint(new Waypoint(parent.transform.position + wanderTarget, true, Random.Range(2, 5)), true);
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
