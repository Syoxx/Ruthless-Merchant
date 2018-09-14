using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_DestroyAsyncLoadingScene : MonoBehaviour
{

	// Use this for initialization
	void Awake ()
	{
	    SceneManager.UnloadSceneAsync("VR_LoadingScene");
    }
}
