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
        private Faction owner;
        private Hero hero;
        private Dictionary<Faction, int> capturingUnits;

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
        }

        // Update is called once per frame
        void Update()
        {
            if(hero != null)
            {
                //Capture
                Debug.Log("Capturing");
                foreach (KeyValuePair<Faction, int> item in capturingUnits)
                {
                    Debug.Log(item.Key.ToString() + ": " + item.Value.ToString());
                }
            }
            else
            {
                Debug.Log("Hero is still alive!");
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
