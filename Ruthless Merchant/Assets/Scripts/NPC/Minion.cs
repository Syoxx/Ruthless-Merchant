//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Minion : Warrior
    {
        private CaptureTrigger currentOutpost = null;
        private CaptureTrigger lastOutpost = null;
        public CaptureTrigger FirstOutpost = null;

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
            if (CurrentAction == null || (CurrentAction is ActionIdle && !(CurrentAction is ActionCapture)))
            {
                CaptureTrigger outpost = currentOutpost;
                if (currentOutpost == null)
                    outpost = lastOutpost;

                if (outpost != null)
                {
                    if (outpost.Owner == faction)
                    {
                        ChangeSpeed(SpeedType.Walk);
                        SelectNextOutpost(outpost);
                    }
                    else
                    {
                        AddNewWaypoint(new Waypoint(outpost.Target, true, 0));
                        SetCurrentAction(new ActionMove(), null);
                    }
                }
                else
                {
                    if (FirstOutpost != null)
                    {
                        AddNewWaypoint(new Waypoint(FirstOutpost.Target, true, 0));
                        SetCurrentAction(new ActionMove(), null);
                    }
                }
            }
        }

        /// <summary>
        /// On outpost entered (Capture / Select new outpost)
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CaptureTrigger"))
            {
                if (CurrentAction == null || CurrentAction is ActionMove || CurrentAction is ActionIdle)
                {
                    CaptureTrigger outpost = other.GetComponent<CaptureTrigger>();
                    if (outpost != null)
                    {
                        if (outpost.Owner != faction)
                        {
                            if (outpost.Hero == null || outpost.IsHeroAway)
                            {
                                currentOutpost = outpost;
                                SetCurrentAction(new ActionCapture(ActionNPC.ActionPriority.Medium), outpost.gameObject, false, false);
                            }
                            else
                            {
                                currentOutpost = outpost;
                                SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.High), outpost.Hero.gameObject, true, true);
                            }
                        }
                        else
                        {
                            TryPickupEquipment(outpost);
                            if(outpost.IsUnderAttack)
                            {
                                Transform attacker = outpost.GetClosestAttacker(this);
                                if (attacker != null)
                                    SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.High), attacker.gameObject, true, true);
                            }
                            SelectNextOutpost(outpost);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Chooses the next outpost to goto
        /// </summary>
        /// <param name="outpost">Current outpost</param>
        private void SelectNextOutpost(CaptureTrigger outpost)
        {
            if (outpost != null)
            {
                List<CaptureTrigger> possibleTriggers = new List<CaptureTrigger>();
                if (Faction == Faction.Freidenker)
                {
                    if (outpost.OutpostsToImperialist != null && outpost.OutpostsToImperialist.Length > 0)
                    {
                        if (outpost.IsLaneSplitter && laneSelectionIndex < outpost.OutpostsToImperialist.Length)
                        {
                            possibleTriggers.Add(outpost.OutpostsToImperialist[laneSelectionIndex]);
                        }
                        else
                        {
                            possibleTriggers.Add(outpost.OutpostsToImperialist[0]);
                            for (int i = 1; i < outpost.OutpostsToImperialist.Length; i++)
                            {
                                if (outpost.OutpostsToImperialist[i].Owner != Faction.Freidenker)
                                    possibleTriggers.Add(outpost.OutpostsToImperialist[i]);
                            }
                        }
                    }
                }
                else if (Faction == Faction.Imperialisten)
                {
                    if (outpost.OutpostsToFreidenker != null && outpost.OutpostsToFreidenker.Length > 0)
                    {
                        if (outpost.IsLaneSplitter && laneSelectionIndex < outpost.OutpostsToFreidenker.Length)
                        {
                            possibleTriggers.Add(outpost.OutpostsToFreidenker[laneSelectionIndex]);
                        }
                        else
                        {
                            possibleTriggers.Add(outpost.OutpostsToFreidenker[0]);
                            for (int i = 1; i < outpost.OutpostsToFreidenker.Length; i++)
                            {
                                if (outpost.OutpostsToFreidenker[i].Owner != Faction.Imperialisten)
                                    possibleTriggers.Add(outpost.OutpostsToFreidenker[i]);
                            }
                        }
                    }
                }

                if (possibleTriggers.Count > 0)
                {
                    int selectedPath = Random.Range(0, possibleTriggers.Count);
                    AddNewWaypoint(new Waypoint(possibleTriggers[selectedPath].Target, true, 0));
                    SetCurrentAction(new ActionMove(), null);
                    lastOutpost = possibleTriggers[selectedPath];
                }
            }
        }
    }
}