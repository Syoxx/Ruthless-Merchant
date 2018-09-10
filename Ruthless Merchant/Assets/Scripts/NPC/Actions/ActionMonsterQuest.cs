using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionMonsterQuest : ActionMove
    {

        //private Fighter fighter;
        private Hero hero;
        private GameObject monster;
        private KillGoal questGoal;

        public ActionMonsterQuest() : base(ActionPriority.Medium)
        {

        }

        public ActionMonsterQuest(KillGoal goal, ActionPriority priority) : base(priority)
        {
            questGoal = goal;
        }

        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            base.EndAction(executeEnd);
        }

        /// <summary>
        /// Commences monster hunt quest and gives hero a waypoint
        /// </summary>
        /// <param name="parent">
        /// Hero that received the quest
        /// </param>
        /// <param name="other">
        /// Targeted monster
        /// </param>
        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            parent.AddNewWaypoint(new Waypoint(other.transform.position, true, 3.0f), true);
            hero = parent as Hero;
            monster = other;
            this.other = other;
            parent.Reacting = true;
            base.StartAction(parent, null);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update(float deltaTime)
        {
            //if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            //{
            //    Debug.Log("close enough");
            //    if (other != null)
            //    {
            //        Debug.Log("Monster !=null");
            //        if (other.GetComponent<Character>().IsDying)
            //        {
            //            Debug.Log("Enemy killed");
            //            questGoal.EnemyKilled();
            //            parent.SetCurrentAction(new ActionIdle(), null, true);
            //            if (!questGoal.Completed)
            //                questGoal.CalcNextWayPoint();
            //        }
            //    }
            //}
            //if (parent.CurrentReactTarget == null || other != parent.CurrentReactTarget.gameObject)
            //{
            //    parent.SetCurrentAction(new ActionIdle(), null, true);
            //    return;
            //}

            //if (monster == null)
            //{
            //    questGoal.EnemyKilled();
            //}

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
