//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionPatrol : ActionMove
    {
        private Hero hero;

        /// <summary>
        /// Action patrol
        /// </summary>
        public ActionPatrol() : base(ActionPriority.Low)
        {

        }

        /// <summary>
        /// Action patrol
        /// </summary>
        /// <param name="priority">Priority of the action</param>
        public ActionPatrol(ActionPriority priority) : base(priority)
        {

        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            if (parent is Hero)
            {
                hero = parent as Hero;

                if (hero.PatrolPoints != null && hero.PatrolPoints.Length > 0)
                {
                    float minDistance = float.MaxValue;
                    int nearestIndex = hero.Waypoints.Count;
                    for (int i = 0; i < hero.PatrolPoints.Length; i++)
                    {
                        float distance = Vector3.Distance(hero.transform.position, hero.PatrolPoints[i].GetPosition());
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestIndex = hero.Waypoints.Count;
                        }
                        hero.Waypoints.Add(hero.PatrolPoints[i]);
                    }

                    hero.ChangeSpeed(NPC.SpeedType.Walk);
                    ActionMove moveAction = new ActionMove();
                    hero.SetCurrentAction(moveAction, null, true);
                    moveAction.WaypointIndex = nearestIndex;
                }
                base.StartAction(hero, other);
            }
            else
            {
                parent.SetCurrentAction(new ActionIdle(), null, true, false);
            }
        }

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                if (hero != null)
                {
                    for (int i = 0; i < hero.PatrolPoints.Length; i++)
                    {
                        hero.Waypoints.Remove(hero.PatrolPoints[i]);
                    }

                    if (hero.CurrentAction != null && hero.CurrentAction is ActionMove)
                        ((ActionMove)hero.CurrentAction).WaypointIndex = 0;

                    if (hero.CurrentAction == null)
                        hero.SetCurrentAction(new ActionIdle(), null);
                }
            }
            base.EndAction(executeEnd);
        }
    }
}
