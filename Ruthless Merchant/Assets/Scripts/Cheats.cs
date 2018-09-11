//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------
using UnityEngine;

namespace RuthlessMerchant
{
    /// <summary>
    /// Attach to Eventsystem in scene
    /// </summary>
    public class Cheats : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            //Disable Tutorial
            if (Input.GetKey(KeyCode.F1))
            {
                SkipTutorial();
            }

            //Teleport to Imperialist city
            if (Input.GetKey(KeyCode.F2))
            {
                TeleportToImperialists();
            }

            //Teleport to freemind city
            if (Input.GetKey(KeyCode.F3))
            {
                TeleportToFreeminds();
            }

            //Imperialist wins
            if (Input.GetKey(KeyCode.F4))
            {
                SetFactionVictory(Faction.Imperialisten);
            }

            //Freemind wins
            if (Input.GetKey(KeyCode.F5))
            {
                SetFactionVictory(Faction.Freidenker);
            }

            //Skip collection part of tutorial
            if(Input.GetKey(KeyCode.F6))
            {
                SkipCollectionTutorial();
            }

            //Skip crafting part of tutorial
            if(Input.GetKey(KeyCode.F7))
            {
                SkipPlayerCaveTutorial();
            }

            //Don't ask...
            if(Input.GetKey(KeyCode.F10))
            {
                Praise();
            }
        }

        /// <summary>
        /// Disables the tutorial
        /// </summary>
        private void SkipTutorial()
        {
            GameObject player = GameObject.Find("NewPlayerPrefab");
            Tutorial tut = player.GetComponent<Tutorial>();
            tut.DisableTutorial();
        }

        /// <summary>
        /// Teleports the player to the main city of the freeminds
        /// </summary>
        private void TeleportToFreeminds()
        {
            GameObject gobj = GameObject.Find("CityFreidenker");
            GameObject player = GameObject.Find("NewPlayerPrefab");
            if (gobj != null && player != null)
            {
                player.transform.position = gobj.transform.position;
                Debug.Log("Teleported to freeminds city");
            }
        }

        /// <summary>
        /// Teleports the player to the main city of the imperialists
        /// </summary>
        private void TeleportToImperialists()
        {
            GameObject gobj = GameObject.Find("Main City Imperalists 2");
            GameObject player = GameObject.Find("NewPlayerPrefab");
            if (gobj != null && player != null)
            {
                player.transform.position = gobj.transform.position;
                Debug.Log("Teleported to imperialist city");
            }

        }

        /// <summary>
        /// Skips the collection part of the tutorial
        /// </summary>
        private void SkipCollectionTutorial()
        {
            GameObject player = GameObject.Find("NewPlayerPrefab");
            Tutorial tut = player.GetComponent<Tutorial>();
            tut.CollectionIsCompleted();
        }

        /// <summary>
        /// Skips the player cave part of the tutorial
        /// </summary>
        private void SkipPlayerCaveTutorial()
        {
            GameObject player = GameObject.Find("NewPlayerPrefab");
            Tutorial tut = player.GetComponent<Tutorial>();
            tut.WorkbenchIsCompleted();
            tut.AlchemyIsCompleted();
            tut.SmithIsCompleted();
        }

        /// <summary>
        /// Set the owner of all outposts to a given faction
        /// </summary>
        /// <param name="faction">Owner faction</param>
        private void SetFactionVictory(Faction faction)
        {
            GameObject[] gobjs = GameObject.FindGameObjectsWithTag("CaptureTrigger");
            for (int i = 0; i < gobjs.Length; i++)
            {
                CaptureTrigger trigger = gobjs[i].GetComponent<CaptureTrigger>();
                if (faction == Faction.Freidenker)
                {
                    trigger.Owner = faction;
                    trigger.CaptureValue = -100;
                }
                else if(faction == Faction.Imperialisten)
                {
                    trigger.Owner = faction;
                    trigger.CaptureValue = 100;
                }
            }
        }

        /// <summary>
        /// Don't ask... :D
        /// </summary>
        private void Praise()
        {
            Debug.Log("Praise Lord Crone!");
        }
    }
}
