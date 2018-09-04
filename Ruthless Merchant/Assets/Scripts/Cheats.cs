//Author: Marcel Croonenbroeck

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
                GameObject player = GameObject.Find("NewPlayerPrefab");
                Tutorial tut = player.GetComponent<Tutorial>();
                tut.DisableTutorial();
            }

            //Teleport to Imperialist city
            if (Input.GetKey(KeyCode.F2))
            {
                GameObject gobj = GameObject.Find("Main City Imperalists 2");
                GameObject player = GameObject.Find("NewPlayerPrefab");
                if (gobj != null && player != null)
                {
                    player.transform.position = gobj.transform.position;
                    Debug.Log("Teleported to imperialist city");
                }

            }

            //Teleport to freemind city
            if (Input.GetKey(KeyCode.F3))
            {
                GameObject gobj = GameObject.Find("CityFreidenker");
                GameObject player = GameObject.Find("NewPlayerPrefab");
                if (gobj != null && player != null)
                {
                    player.transform.position = gobj.transform.position;
                    Debug.Log("Teleported to freeminds city");
                }
            }

            if (Input.GetKey(KeyCode.F4))
            {
                GameObject[] gobjs = GameObject.FindGameObjectsWithTag("CaptureTrigger");
                for (int i = 0; i < gobjs.Length; i++)
                {
                    CaptureTrigger trigger = gobjs[i].GetComponent<CaptureTrigger>();
                    trigger.Owner = Faction.Imperialisten;
                    trigger.CaptureValue = 100;
                }
            }

            if (Input.GetKey(KeyCode.F5))
            {
                GameObject[] gobjs = GameObject.FindGameObjectsWithTag("CaptureTrigger");
                for (int i = 0; i < gobjs.Length; i++)
                {
                    CaptureTrigger trigger = gobjs[i].GetComponent<CaptureTrigger>();
                    trigger.Owner = Faction.Freidenker;
                    trigger.CaptureValue = -100;
                }
            }

            if(Input.GetKey(KeyCode.F6))
            {
                GameObject player = GameObject.Find("NewPlayerPrefab");
                Tutorial tut = player.GetComponent<Tutorial>();
                tut.CollectionIsCompleted();
            }

            if(Input.GetKey(KeyCode.F7))
            {
                GameObject player = GameObject.Find("NewPlayerPrefab");
                Tutorial tut = player.GetComponent<Tutorial>();
                tut.WorkbenchIsCompleted();
                tut.AlchemyIsCompleted();
                tut.SmithIsCompleted();
            }

            if(Input.GetKey(KeyCode.F10))
            {
                Debug.Log("Praise Lord Crone!");
            }
        }
    }
}
