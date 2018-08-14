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
            base.StartAction(parent, other);
        }

        public override void Update(float deltaTime)
        {
            if (!agent.pathPending && agent.remainingDistance < agent.baseOffset)
            {
                //Collect
                Debug.Log("other: " + other);
                goal.CollectableFound(other.GetComponent<Material>());
                GameObject.Destroy(other);
            }
            base.Update(deltaTime);
        }

        public override void EndAction(bool executeEnd = true)
        {
            if(executeEnd)
            {
                //
            }
            base.EndAction(executeEnd);
        }
    }
}