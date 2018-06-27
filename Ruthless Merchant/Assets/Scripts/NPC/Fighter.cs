//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Fighter : NPC
    {
        [SerializeField]
        [Range(0, 100)]
        protected float huntDistance = 5;

        [SerializeField]
        [Range(1, 100)]
        protected float attackDistance = 1.5f;

        [SerializeField]
        private bool patrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        public bool PaartolActive
        {
            get { return patrolActive; }
        }

        public float HuntDistance
        {
            get
            {
                return huntDistance;
            }
        }

        public float AttackDistance
        {
            get
            {
                return attackDistance;
            }
        }

        public override void Start()
        {
            base.Start();
            HealthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
            
            if (patrolActive)
            {
                patrolActive = false;
                if(PossiblePatrolPaths != null && PossiblePatrolPaths.Length > 0)
                    PatrolPoints = new List<Waypoint>(GetRandomPath(PossiblePatrolPaths, false, 3)).ToArray();

                Patrol();
            }
        }

        public override void Update()
        {
            base.Update();
            if (CurrentAction == null || !Reacting)
            {
                ChangeSpeed(SpeedType.Walk);
                Patrol();
            }
            else
            {
                AbortPatrol();
            }
        }

        private void HealthSystem_OnHealthChanged(object sender, DamageAbleObject.HealthArgs e)
        {
            if (e.ChangedValue < 0 && e.Sender != null)
            {
                AddNewWaypoint(new Waypoint(e.Sender.transform, true, 0));
            }
        }

        public void Patrol()
        {
            if (!patrolActive)
            {
                if (PatrolPoints != null && PatrolPoints.Length > 0)
                {
                    patrolActive = true;
                    float minDistance = float.MaxValue;
                    int nearestIndex = waypoints.Count;
                    for (int i = 0; i < PatrolPoints.Length; i++)
                    {
                        float distance = Vector3.Distance(transform.position, PatrolPoints[i].GetPosition());
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestIndex = waypoints.Count;
                        }
                        waypoints.Add(PatrolPoints[i]);
                    }

                    ChangeSpeed(SpeedType.Walk);
                    ActionMove moveAction = new ActionMove();
                    SetCurrentAction(moveAction, null);
                    moveAction.WaypointIndex = nearestIndex;
                }
            }
        }

        public void AbortPatrol()
        {
            if (patrolActive)
            {
                patrolActive = false;
                for (int i = 0; i < PatrolPoints.Length; i++)
                {
                    waypoints.Remove(PatrolPoints[i]);
                }

                if(CurrentAction != null && CurrentAction is ActionMove)
                    ((ActionMove)CurrentAction).WaypointIndex = 0;

                if (CurrentAction == null)
                    SetCurrentAction(new ActionIdle(), null);
            }
        }

        public override void React(Character character, bool isThreat)
        {
            if (isThreat)
            {
                if (((float)HealthSystem.Health / character.HealthSystem.Health) > 0.15f)
                {
                    float distance = Vector3.Distance(transform.position, character.transform.position);
                    if (distance <= attackDistance)
                    {
                        if (CurrentAction == null || !(CurrentAction is ActionAttack))
                            SetCurrentAction(new ActionAttack(), character.gameObject);
                    }
                    else
                    {
                        if (CurrentAction == null || !(CurrentAction is ActionHunt))
                            SetCurrentAction(new ActionHunt(), character.gameObject);
                    }
                }
                else
                {
                    if (CurrentAction == null || !(CurrentAction is ActionFlee))
                        SetCurrentAction(new ActionFlee(), character.gameObject);
                }
            }
            else
            {
                //TODO: whatever...
            }
        }

        public override void React(Item item)
        {
            //TODO: Pickup item
            Reacting = true;
        }
    }
}