using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjects : MonoBehaviour {

    private GameObject[] npcArray;
    private GameObject worldGameObject;

    [Header("Press L to disable all NPCs, O to enable")]
    [SerializeField]
    private int Placeholder;

    [Header("Press 8 to disable world, 9 to enable")]
    [SerializeField]
    private int Placeholder2;
    // Use this for initialization
    void Start () {
        worldGameObject = GameObject.FindGameObjectWithTag("WorldGameObject");
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

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("disabling empty GameObject for World");
            worldGameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("enabling empty GameObject for World");
            worldGameObject.SetActive(true);
        }


	}
}
