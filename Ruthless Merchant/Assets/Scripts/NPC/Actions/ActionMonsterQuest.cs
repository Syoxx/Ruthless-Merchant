using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionMonsterQuest : ActionMove
    {

        //private Fighter fighter;
        private Hero hero;

        public ActionMonsterQuest() : base(ActionPriority.Medium)
        {

        }

        public ActionMonsterQuest(ActionPriority priority) : base(priority)
        {

        }

        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            base.EndAction(executeEnd);
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            parent.AddNewWaypoint(new Waypoint(other.transform.position, true, 3.0f), true);
            hero = parent as Hero;
            parent.Reacting = true;
            base.StartAction(parent, other);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update(float deltaTime)
        {
            //if (parent.CurrentReactTarget == null || other != parent.CurrentReactTarget.gameObject)
            //{
            //    parent.SetCurrentAction(new ActionIdle(), null, true);
            //    return;
            //}

            //if (other.gameObject == null)
            {
                // TODO: evaluate in KillGoal.cs
            }

            float distance = Vector3.Distance(other.transform.position, parent.transform.position);
            if (distance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                parent.Waypoints.Clear();
                parent.SetCurrentAction(new ActionIdle(), null, true);
            }
            else /*if (hero.HuntDistance >= distance)*/
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
