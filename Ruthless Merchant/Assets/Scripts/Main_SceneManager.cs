using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : MonoBehaviour
{
    public static Main_SceneManager Singleton;

    static List<string> LoadedScenes;

    public enum SceneToLoad
    {
        Islandtesting,
        IslandtestingCode
    }

    [SerializeField]
    public SceneToLoad sceneToLoad;

    private void Awake()
    {
        Singleton = this;
        LoadedScenes = new List<string>();
    }

    void Start()
    {
        LoadSceneAdditively(sceneToLoad.ToString());
    }

    public static void LoadSceneAdditively(string sceneName)
    {
        if (LoadedScenes == null)
            LoadedScenes = new List<string>();

        LoadedScenes.Add(sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void UnLoadScene(string sceneName)
    {
        LoadedScenes.Remove(sceneName);
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
