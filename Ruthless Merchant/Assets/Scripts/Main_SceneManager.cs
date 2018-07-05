using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : MonoBehaviour
{
    public Main_SceneManager Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        LoadSceneAdditively("Isleandtesting");
        LoadSceneAdditively("TradeScene");
    }

    public static void LoadSceneAdditively(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
