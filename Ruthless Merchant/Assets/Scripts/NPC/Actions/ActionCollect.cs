using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionCollect : ActionMove
    {
        private CollectionGoal goal;
        public ActionCollect(CollectionGoal goal, ActionPriority actionPriority = ActionPriority.Medium) : base(actionPriority)
        {
            this.goal = goal;
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            parent.AddNewWaypoint(new Waypoint(other.transform.position, true, 1.0f), true);
            base.StartAction(parent, other);
            this.other = other;      
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public override void Update(float deltaTime)
        {
            if (!agent.pathPending)
            {
                if (other != null)
                {
                    float distanceToObj = Vector3.Distance(agent.transform.position, other.transform.position);
                    if (distanceToObj <= agent.stoppingDistance)
                    {
                        if (other.GetComponent<Item>() != null)
                        {
                            goal.CollectableFound(other.GetComponent<Item>());
                            UnityEngine.GameObject.DestroyImmediate(other);
                        }
                        else
                        {
                            Debug.Log("seems like he found ironVein");
                            goal.CollectableFound(null);
                            UnityEngine.GameObject.DestroyImmediate(other);
                        }

                        //parent.SetCurrentAction(new ActionIdle(), null, true);

                        if (!goal.Completed)
                            goal.CalcNextWaypoint();
                    }
                    
                }
            }
            base.Update(deltaTime);
        }

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public override void EndAction(bool executeEnd = true)
        {
            base.EndAction(executeEnd);
        }
    }
}