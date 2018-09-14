using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_LoadingSceneHandler : MonoBehaviour
{
    private string nextScene;


	// Use this for initialization
	void Start ()
	{
        nextScene = PlayerPrefs.GetString("sceneToLoad");
        PlayerPrefs.DeleteAll();
	    //SceneManager.LoadSceneAsync(nextScene);
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
