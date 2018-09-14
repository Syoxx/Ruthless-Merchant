using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{
    public class VR_SceneSwitcher : MonoBehaviour
    {
        public static VR_SceneSwitcher Singleton;

        enum LoadingState
        {
            none,
            notLoaded,
            loadingSceneLoaded,
            finished
        }

        static LoadingState loadingState;

        public const string LoadingSceneName = "VR_LoadingScene";

        public static string SceneToLoadName;

        static AsyncOperation loadSceneOperator;

        bool loadSceneAugmented;

        private bool isloading;

        void Awake()
        {
            Singleton = this;

            switch (loadingState)
            {
                case LoadingState.loadingSceneLoaded:
                    Debug.Log(SceneToLoadName);
                    loadSceneOperator = SceneManager.LoadSceneAsync(SceneToLoadName);
                    StartCoroutine(LoadLevelCOR());
                    break;
            }
        }

        public void LoadScene(string sceneName)
        {
            if (!isloading)
            {
                loadingState = LoadingState.notLoaded;
                SceneToLoadName = sceneName;
                loadSceneOperator = SceneManager.LoadSceneAsync(LoadingSceneName);
                StartCoroutine(LoadLevelCOR());
                isloading = true;
            }
        }

        IEnumerator LoadLevelCOR()
        {
            loadSceneOperator.allowSceneActivation = false;

            while (!loadSceneOperator.isDone)
            {
                yield return 0;

                if (loadSceneOperator.progress >= 0.9f && !loadSceneAugmented)
                {
                    loadingState++;
                    loadSceneAugmented = true;
                    loadSceneOperator.allowSceneActivation = true;
                }
            }
        }
    }
}