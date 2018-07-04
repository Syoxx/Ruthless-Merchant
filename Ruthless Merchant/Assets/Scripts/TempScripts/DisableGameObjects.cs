using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjects : MonoBehaviour {

    GameObject[] npcArray;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            npcArray = GameObject.FindGameObjectsWithTag("NPC");
            Debug.Log("disabling all NPCs");
            foreach (GameObject go in npcArray)
            {
                go.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("enabling all NPCs");
            foreach (GameObject go in npcArray)
            {
                go.SetActive(true);
            }
        }
	}
}
