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
        private float captureValue = 0;

        private Hero hero;
        private Dictionary<Faction, int> capturingUnits;
        private List<NPC> capturingUnitsList;

        private Renderer flagRenderer;

        [SerializeField]
        private bool isLaneSplitter = false;

        [SerializeField]
        private CaptureTrigger[] outpostsToFreidenker;

        [SerializeField]
        private CaptureTrigger[] outpostsToImperialist;

        public float CaptureValue
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

                UpdateFlagColor();
            }
        }

        public bool IsLaneSplitter
        {
            get
            {
                return isLaneSplitter;
            }
        }

        // Use this for initialization
        void Start()
        {
            capturingUnits = new Dictionary<Faction, int>();
            capturingUnitsList = new List<NPC>();
            if (owner == Faction.Freidenker)
                captureValue = -100;
            else if (owner == Faction.Imperialisten)
                captureValue = 100;

            Transform parentFlag = transform.Find("Flag");
            if (parentFlag != null)
            {
                Transform obj = parentFlag.Find("Flag");

                if (obj != null && obj.CompareTag("Flag"))
                {
                    flagRenderer = obj.GetComponent<Renderer>();
                    UpdateFlagColor();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (hero == null && capturingUnits != null)
            {
                //int freidenkerCount = capturingUnits.ContainsKey(Faction.Freidenker) ? capturingUnits[Faction.Freidenker] : 0;
                //int imperialistenCount = capturingUnits.ContainsKey(Faction.Imperialisten) ? capturingUnits[Faction.Imperialisten] : 0;
                int monsterCount = capturingUnits.ContainsKey(Faction.Monster) ? capturingUnits[Faction.Monster] : 0;

                float capValue = 0;
                for (int i = 0; i < capturingUnitsList.Count; i++)
                {                   
                    if(capturingUnitsList[i].Faction == Faction.Freidenker)
                    {
                        capValue -= capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                    }
                    else if(capturingUnitsList[i].Faction == Faction.Imperialisten)
                    {
                        capValue += capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                    }
                    else if(capturingUnitsList[i].Faction == Faction.Monster)
                    {
                        if (captureValue < 0)
                            capValue += capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                        else if (captureValue > 0)
                            capValue -= capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                    }
                }

                if (monsterCount <= 0 || System.Math.Abs(CaptureValue + capValue) < System.Math.Abs(CaptureValue))
                {
                    if (hero != null)
                    {
                        if (owner == Faction.Freidenker)
                        {
                            if (CaptureValue + capValue > -50)
                                CaptureValue = -50;

                        }
                        else if (owner == Faction.Imperialisten)
                        {
                            if (CaptureValue + capValue < 50)
                                CaptureValue = 50;
                        }
                    }
                    else
                    {
                        CaptureValue += capValue;
                    }
                }
               /* if (freidenkerCount > imperialistenCount)
                {
                    if (CaptureValue > 0 || freidenkerCount > imperialistenCount + monsterCount)
                        CaptureValue--;

                }
                else if (imperialistenCount > freidenkerCount)
                {
                    if (CaptureValue < 0 || imperialistenCount > freidenkerCount + monsterCount)
                        CaptureValue++;
                }*/

                if (captureValue <= -50)
                    Owner = Faction.Freidenker;
                else if (captureValue >= 50)
                    Owner = Faction.Imperialisten;
                else
                    Owner = Faction.Neutral;
            }
        }

        private void UpdateFlagColor()
        {
            if (flagRenderer != null)
            {
                if (owner == Faction.Freidenker)
                    flagRenderer.material.color = Color.green;
                else if (owner == Faction.Imperialisten)
                    flagRenderer.material.color = Color.red;
                else
                    flagRenderer.GetComponent<Renderer>().material.color = Color.gray;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                NPC npc = other.GetComponent<NPC>();
                if (!capturingUnits.ContainsKey(npc.Faction))
                    capturingUnits.Add(npc.Faction, 0);

                capturingUnitsList.Add(npc);
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

                    capturingUnitsList.Remove(npc);
                }
            }
        }
    }
}
