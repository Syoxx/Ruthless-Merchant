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
        public int Level = 1;

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

        /// <summary>
        /// Init Hero
        /// </summary>
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

        /// <summary>
        /// Update hero
        /// </summary>
        public override void Update()
        {
            if (Outpost != null)
            {
                if (
                    (Outpost.IsHeroAway || Outpost.Owner != faction) &&
                    (Waypoints.Count == 0 || waypoints[0].Transform != Outpost.Target))
                {
                    AddNewWaypoint(new Waypoint(Outpost.Target, true, 0), true);
                    SetCurrentAction(new ActionMove(ActionNPC.ActionPriority.Low), null, CurrentAction is ActionWander || CurrentAction is ActionIdle);
                }
                else if (!Outpost.IsHeroAway)
                {
                    if (Outpost.IsUnderAttack)
                    {
                        Transform target = Outpost.GetClosestAttacker(this);
                        if(!(CurrentAction is ActionAttack))
                            SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.High), target.gameObject, false, true);
                    }
                    else
                    {
                        if (CurrentAction is ActionIdle || CurrentAction == null)
                        {
                            SetCurrentAction(new ActionWander(), null);
                            TryPickupEquipment(Outpost);
                        }
                    }
                }
            }

            base.Update();
        }
    }
}