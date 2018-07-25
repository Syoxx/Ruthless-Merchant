//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Hero : Warrior
    {
        private QuestItem quest;

        public CaptureTrigger Outpost;

        [Header("Patrol settings")]
        [SerializeField]
        private bool patrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        public bool PaartolActive
        {
            get { return patrolActive; }
        }

        public override void Start()
        {
            if (patrolActive)
            {
                patrolActive = false;
                if (PossiblePatrolPaths != null && PossiblePatrolPaths.Length > 0)
                    PatrolPoints = new List<Waypoint>(GetRandomPath(PossiblePatrolPaths, false, 3)).ToArray();

                SetCurrentAction(new ActionPatrol(ActionNPC.ActionPriority.Low), null);
            }
            base.Start();
        }

        public override void Update()
        {
            if(Outpost != null)
            {
                if (Outpost.IsHeroAway && quest.Equals(default(QuestItem)) && (Waypoints.Count == 0 || waypoints[0].Transform != Outpost.transform))
                {
                    AddNewWaypoint(new Waypoint(Outpost.transform, true, 0), true);
                    SetCurrentAction(new ActionMove(ActionNPC.ActionPriority.Medium), null, true);
                }
                else if (!Outpost.IsHeroAway)
                {
                    if(CurrentAction is ActionIdle || CurrentAction == null)
                        SetCurrentAction(new ActionWander(), null);
                }
            }

            base.Update();
        }
    }
}