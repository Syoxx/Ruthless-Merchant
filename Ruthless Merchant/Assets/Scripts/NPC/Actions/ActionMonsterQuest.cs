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
    }
}
