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
        [Header("NPC Fighter settings")]
        [SerializeField, Tooltip("If the distance to a character is smaller than the hunting distance, the NPC follows the character")]
        [Range(0, 100)]
        protected float huntDistance = 5;

        [SerializeField, Tooltip("If the distance to a character is smaller then the attacking distance, the npc attacks the character")]
        [Range(1, 100)]
        protected float attackDistance = 1.5f;

        [Header("Patrol settings")]
        [SerializeField]
        private bool patrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        private CaptureTrigger currentTrigger = null;
        private CaptureTrigger lastTrigger = null;

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

            if(CurrentAction == null || (CurrentAction is ActionIdle && !(CurrentAction is ActionCapture)))
            {
                CaptureTrigger trigger = currentTrigger;
                if (currentTrigger == null)
                    trigger = lastTrigger;

                if (trigger != null)
                {
                    if (trigger.Owner == faction)
                    {
                        ChangeSpeed(SpeedType.Walk);
                        SelectNextOutpost(trigger);
                    }
                    else
                    {
                        AddNewWaypoint(new Waypoint(trigger.transform, true, 0));
                        SetCurrentAction(new ActionMove(), null);
                    }
                }
            }
        }

        private void HealthSystem_OnHealthChanged(object sender, DamageAbleObject.HealthArgs e)
        {
            if (CurrentAction == null || CurrentAction is ActionIdle)
            {
                if (e.ChangedValue < 0 && e.Sender != null && HealthSystem.Health > 0)
                {
                    SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.Medium), e.Sender.gameObject, false, true);
                }
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
        }

        public override void React(Item item)
        {
            //TODO: Pickup item
            Reacting = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CaptureTrigger"))
            {
                if (CurrentAction == null || CurrentAction is ActionMove || CurrentAction is ActionIdle)
                {
                    CaptureTrigger trigger = other.GetComponent<CaptureTrigger>();
                    if (trigger != null)
                    {
                        if (trigger.Owner != faction)
                        {
                            currentTrigger = trigger;
                            SetCurrentAction(new ActionCapture(ActionNPC.ActionPriority.Medium), trigger.gameObject, false, false);
                        }
                        else
                        {
                            SelectNextOutpost(trigger);
                        }
                    }
                }
            }
        }

        private void SelectNextOutpost(CaptureTrigger trigger)
        {
            if (trigger != null)
            {
                List<CaptureTrigger> possibleTriggers = new List<CaptureTrigger>();
                if (Faction == Faction.Freidenker)
                {
                    if (trigger.OutpostsToImperialist != null && trigger.OutpostsToImperialist.Length > 0)
                    {
                        if (trigger.IsLaneSplitter && laneSelectionIndex < trigger.OutpostsToImperialist.Length)
                        {
                            possibleTriggers.Add(trigger.OutpostsToImperialist[laneSelectionIndex]);
                        }
                        else
                        {
                            possibleTriggers.Add(trigger.OutpostsToImperialist[0]);
                            for (int i = 1; i < trigger.OutpostsToImperialist.Length; i++)
                            {
                                if (trigger.OutpostsToImperialist[i].Owner != Faction.Freidenker)
                                    possibleTriggers.Add(trigger.OutpostsToImperialist[i]);
                            }
                        }
                    }
                }
                else if (Faction == Faction.Imperialisten)
                {
                    if (trigger.OutpostsToFreidenker != null && trigger.OutpostsToFreidenker.Length > 0)
                    {
                        if (trigger.IsLaneSplitter && laneSelectionIndex < trigger.OutpostsToFreidenker.Length)
                        {
                            possibleTriggers.Add(trigger.OutpostsToFreidenker[laneSelectionIndex]);
                        }
                        else
                        {
                            possibleTriggers.Add(trigger.OutpostsToFreidenker[0]);
                            for (int i = 1; i < trigger.OutpostsToFreidenker.Length; i++)
                            {
                                if (trigger.OutpostsToFreidenker[i].Owner != Faction.Imperialisten)
                                    possibleTriggers.Add(trigger.OutpostsToFreidenker[i]);
                            }
                        }
                    }
                }

                if (possibleTriggers.Count > 0)
                {
                    int selectedPath = Random.Range(0, possibleTriggers.Count);
                    AddNewWaypoint(new Waypoint(possibleTriggers[selectedPath].transform, true, 0));
                    SetCurrentAction(new ActionMove(), null);
                    lastTrigger = possibleTriggers[selectedPath];
                }
            }
        }

        /// <summary>
        /// Selects a path from capture points
        /// </summary>
        /*protected void SelectPath(CaptureTrigger captureTrigger)
        {
            if (captureTrigger != null)
            {
                if (Faction == Faction.Freidenker)
                {
                    if (currentTrigger.OutpostsToImperialist != null && currentTrigger.OutpostsToImperialist.Length > 0)
                    {

                        int selectedPath = Random.Range(0, currentTrigger.OutpostsToImperialist.Length);
                        ö
                        
                            currentTrigger = currentTrigger.OutpostsToImperialist[selectedPath];
                            currentPath.Add(currentTrigger);
                        
                    }
                }
                else if (Faction == Faction.Imperialisten)
                {
                    if (currentTrigger.OutpostsToFreidenker != null && currentTrigger.OutpostsToFreidenker.Length > 0)
                    {
                        int selectedPath = Random.Range(0, currentTrigger.OutpostsToFreidenker.Length);
                            currentTrigger = currentTrigger.OutpostsToFreidenker[selectedPath];
                            currentPath.Add(currentTrigger);
                        
                    }
                }
            }
        }*/
    }
}