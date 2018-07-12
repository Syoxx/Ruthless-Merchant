//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionPatrol : ActionMove
    {
        private Fighter fighter;

        public ActionPatrol()
        {

        }

        public ActionPatrol(ActionPriority priority) : base(priority)
        {

        }

        public override void StartAction(NPC parent, GameObject other)
        {
            if (parent is Fighter)
            {
                fighter = parent as Fighter;

                if (fighter.PatrolPoints != null && fighter.PatrolPoints.Length > 0)
                {
                    float minDistance = float.MaxValue;
                    int nearestIndex = fighter.Waypoints.Count;
                    for (int i = 0; i < fighter.PatrolPoints.Length; i++)
                    {
                        float distance = Vector3.Distance(fighter.transform.position, fighter.PatrolPoints[i].GetPosition());
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestIndex = fighter.Waypoints.Count;
                        }
                        fighter.Waypoints.Add(fighter.PatrolPoints[i]);
                    }

                    fighter.ChangeSpeed(NPC.SpeedType.Walk);
                    ActionMove moveAction = new ActionMove();
                    fighter.SetCurrentAction(moveAction, null, true);
                    moveAction.WaypointIndex = nearestIndex;
                }
                base.StartAction(fighter, other);
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
                if (fighter != null)
                {
                    for (int i = 0; i < fighter.PatrolPoints.Length; i++)
                    {
                        fighter.Waypoints.Remove(fighter.PatrolPoints[i]);
                    }

                    if (fighter.CurrentAction != null && fighter.CurrentAction is ActionMove)
                        ((ActionMove)fighter.CurrentAction).WaypointIndex = 0;

                    if (fighter.CurrentAction == null)
                        fighter.SetCurrentAction(new ActionIdle(), null);
                }
            }
            base.EndAction(executeEnd);
        }
    }
}
