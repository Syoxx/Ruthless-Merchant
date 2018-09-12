using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScrollingText : MonoBehaviour {

    [SerializeField]
    private string mainMenu = "newMainMenu";

    [SerializeField]
    private float scrollSpeed = 1;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(mainMenu);
	}
}
