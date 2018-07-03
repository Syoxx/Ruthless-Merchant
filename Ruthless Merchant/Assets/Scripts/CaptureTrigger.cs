//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CaptureTrigger : MonoBehaviour
    {
        [SerializeField]
        private Faction owner = Faction.None;
        private int captureValue = 0;

        private Hero hero;
        private Dictionary<Faction, int> capturingUnits;

        [SerializeField]
        private CaptureTrigger[] outpostsToFreidenker;

        [SerializeField]
        private CaptureTrigger[] outpostsToImperialist;

        public int CaptureValue
        {
            get
            {
                return captureValue;
            }
            set
            {
                captureValue = value;
                if (captureValue < -100)
                    captureValue = -100;
                else if (captureValue > 100)
                    captureValue = 100;
            }
        }

        public CaptureTrigger[] OutpostsToFreidenker
        {
            get
            {
                return outpostsToFreidenker;
            }
        }

        public CaptureTrigger[] OutpostsToImperialist
        {
            get
            {
                return outpostsToImperialist;
            }
        }

        /// <summary>
        /// Get or set the hero of the capture point and sets the owners faction to the faction of the hero
        /// </summary>
        public Hero Hero
        {
            get
            {

                return hero;

            }

            set
            {
                hero = value;
                if (hero != null)
                    owner = hero.Faction;
                else
                    owner = Faction.None;
            }
        }

        /// <summary>
        /// Sets the owner faction. If the new faction doesn't match the heros faction, the hero is set to null.
        /// </summary>
        public Faction Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
                if (hero != null && hero.Faction != owner)
                    hero = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            capturingUnits = new Dictionary<Faction, int>();
            if (owner == Faction.Freidenker)
                captureValue = -100;
            else if (owner == Faction.Imperialisten)
                captureValue = 100;
        }

        // Update is called once per frame
        void Update()
        {
            if (hero == null && capturingUnits != null)
            {
                int freidenkerCount = capturingUnits.ContainsKey(Faction.Freidenker) ? capturingUnits[Faction.Freidenker] : 0;
                int imperialistenCount = capturingUnits.ContainsKey(Faction.Imperialisten) ? capturingUnits[Faction.Imperialisten] : 0;
                int monsterCount = capturingUnits.ContainsKey(Faction.Monster) ? capturingUnits[Faction.Monster] : 0;

                if (freidenkerCount > imperialistenCount)
                {
                    if (CaptureValue > 0 || freidenkerCount > imperialistenCount + monsterCount)
                        CaptureValue--;

                }
                else if (imperialistenCount > freidenkerCount)
                {
                    if (CaptureValue < 0 || imperialistenCount > freidenkerCount + monsterCount)
                        CaptureValue++;
                }

                if (captureValue < -25)
                    Owner = Faction.Freidenker;
                else if (captureValue > 25)
                    Owner = Faction.Imperialisten;
                else
                    Owner = Faction.Neutral;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                NPC npc = other.GetComponent<NPC>();
                if (!capturingUnits.ContainsKey(npc.Faction))
                    capturingUnits.Add(npc.Faction, 0);

                capturingUnits[npc.Faction]++;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                NPC npc = other.GetComponent<NPC>();
                if (capturingUnits.ContainsKey(npc.Faction))
                {
                    capturingUnits[npc.Faction]--;
                    if (capturingUnits[npc.Faction] <= 0)
                        capturingUnits.Remove(npc.Faction);
                }
            }
        }
    }
}
