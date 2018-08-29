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

        public ActionPatrol() : base(ActionPriority.Low)
        {

        }

        public ActionPatrol(ActionPriority priority) : base(priority)
        {

        }

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
