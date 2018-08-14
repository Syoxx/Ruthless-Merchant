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

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.AddNewWaypoint(new Waypoint(other.transform.position, true, 3.0f), true);

            base.StartAction(parent, null);
            this.other = other;      
        }

        public override void Update(float deltaTime)
        {
            if (!agent.pathPending && agent.remainingDistance < agent.baseOffset)
            {
                if (other != null)
                {
                    //Collect
                    goal.CollectableFound(other.GetComponent<Material>());
                    UnityEngine.GameObject.DestroyImmediate(other);

                    //Set new goal and Action
                    parent.SetCurrentAction(new ActionIdle(), null, true, true);
                    goal.CalcNextWaypoint();
                }
            }
            base.Update(deltaTime);
        }

        public override void EndAction(bool executeEnd = true)
        {
            base.EndAction(executeEnd);
        }
    }
}