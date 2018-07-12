using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : MonoBehaviour
{
    public static Main_SceneManager Singleton;

    static List<string> LoadedScenes;

    [SerializeField]
    bool loadArtScene = false;

    private void Awake()
    {
        Singleton = this;
        LoadedScenes = new List<string>();
    }

    void Start()
    {
        LoadSceneAdditively("IslandtestingCode");

        if (loadArtScene)
        {
            LoadSceneAdditively("Islandtesting");
        }
    }

    public static void LoadSceneAdditively(string sceneName)
    {
        LoadedScenes.Add(sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public static void UnLoadScene(string sceneName)
    {
        LoadedScenes.Remove(sceneName);
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
