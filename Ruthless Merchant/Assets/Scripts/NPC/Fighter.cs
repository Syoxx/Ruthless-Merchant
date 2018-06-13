using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Fighter : NPC
    {
        [SerializeField]
        [Range(0, 1000)]
        protected int huntDistance = 10;

        [SerializeField]
        [Range(1, 1000)]
        protected int attackDistance = 2;

        [SerializeField]
        private bool patrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        public bool PaartolActive
        {
            get { return patrolActive; }
        }

        public int HuntDistance
        {
            get
            {
                return huntDistance;
            }
        }

        public int AttackDistance
        {
            get
            {
                return attackDistance;
            }
        }

        /*
        Keep distance to enemey (check directions) => DONE?
        follow victim => DONE?
        keep victim in view => DONE?
        Fix Raycast
        */

        public override void Start()
        {
            base.Start();
            HealthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
            
            if (patrolActive)
            {
                patrolActive = false;
                if(PossiblePatrolPaths != null && PossiblePatrolPaths.Length > 0)
                    PatrolPoints = GetRandomPath(PossiblePatrolPaths, false, 3);

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
                if (PatrolPoints != null)
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
                    moveAction.StartAction(this, null);
                    moveAction.WaypointIndex = nearestIndex;
                    CurrentAction = moveAction;
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
                {
                    ActionIdle idle = new ActionIdle();
                    idle.StartAction(this, null);
                    CurrentAction = idle;
                }
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
                        ActionAttack attack = new ActionAttack();
                        attack.StartAction(this, character.gameObject);
                        CurrentAction = attack;
                    }
                    else
                    {
                        ActionHunt hunt = new ActionHunt();
                        hunt.StartAction(this, character.gameObject);
                        CurrentAction = hunt;
                    }
                }
                else
                {
                    ActionFlee flee = new ActionFlee();
                    flee.StartAction(this, character.gameObject);
                    CurrentAction = flee;
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