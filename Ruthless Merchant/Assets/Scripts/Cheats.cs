//Author: Marcel Croonenbroeck

using UnityEngine;

/// <summary>
/// Attach to Eventsystem in scene
/// </summary>
public class Cheats : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
    {
        //Disable Tutorial
		if (Input.GetKey(KeyCode.F1))
        {
            GameObject gobj = GameObject.Find("Tutorial");
            if (gobj != null && gobj.activeSelf)
            {
                gobj.SetActive(false);
                Debug.Log("Tutorial disabled");
            }
        }
        
        //Teleport to Imperialist city
        if(Input.GetKey(KeyCode.F2))
        {
            GameObject gobj = GameObject.Find("Main City Imperalists");
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
    }
}
