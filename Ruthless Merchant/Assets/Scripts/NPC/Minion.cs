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
        private CaptureTrigger currentTrigger = null;
        private CaptureTrigger lastTrigger = null;

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            if (CurrentAction == null || (CurrentAction is ActionIdle && !(CurrentAction is ActionCapture)))
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

            base.Update();
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

        /// <summary>
        /// Chooses the next outpost to goto
        /// </summary>
        /// <param name="trigger">Current outpost</param>
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

                if(possibleTriggers.Count > 0)
                {
                    int selectedPath = Random.Range(0, possibleTriggers.Count);
                    AddNewWaypoint(new Waypoint(possibleTriggers[selectedPath].transform, true, 0));
                    SetCurrentAction(new ActionMove(), null);
                    lastTrigger = possibleTriggers[selectedPath];
                }
            }
        }
    }
}