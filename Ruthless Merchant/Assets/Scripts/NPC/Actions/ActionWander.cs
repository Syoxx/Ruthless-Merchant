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

        /// <summary>
        /// Constructor of wander action
        /// </summary>
        public ActionWander() : base(ActionPriority.Low)
        {

        }

        /// <summary>
        /// Constructor of wander action
        /// </summary>
        /// <param name="priority">Action priority</param>
        public ActionWander(ActionPriority priority) : base(priority)
        {

        }

        /// <summary>
        /// Constructor of wander action
        /// </summary>
        /// <param name="moveRange">Max. move range</param>
        /// <param name="minWaitDuration">Min. duration between moves</param>
        /// <param name="maxWaitDuration">Max. duration between moves</param>
        /// <param name="priority">Action priority</param>
        public ActionWander(float moveRange, int minWaitDuration, int maxWaitDuration, ActionPriority priority) : base(priority)
        {
            this.moveRange = moveRange;
            this.minWaitDuration = minWaitDuration;
            this.maxWaitDuration = maxWaitDuration;
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
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

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public override void EndAction(bool executeEnd = true)
        {
            base.EndAction(executeEnd);
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
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
