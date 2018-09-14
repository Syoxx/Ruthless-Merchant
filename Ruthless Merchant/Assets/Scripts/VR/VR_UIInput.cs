using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace RuthlessMerchant
{
    [RequireComponent(typeof(SteamVR_LaserPointer))]
    public class VR_UIInput : MonoBehaviour
    {
        [SerializeField]
        private Light continueLight, exitLight, startLight, creditsLight;
        
        //[SerializeField]
        //private Image fadeImage, loadingImage;

        [SerializeField]
        private GameObject loadingCoin;

        [SerializeField]
        private string gamePlayScene, creditScene;

        private SteamVR_LaserPointer laserPointer;
        private SteamVR_TrackedController trackedController;

        private bool hairTriggerTriggered;


        private AsyncOperation loadSceneOperator;
        private bool levelLoadingInitiated;
        

        private void Start()
        {
            continueLight.enabled = false;
            exitLight.enabled = false;
            startLight.enabled = false;
            creditsLight.enabled = false;

            hairTriggerTriggered = false;

            loadingCoin.SetActive(false);
            levelLoadingInitiated = false;
            
        }

        private void OnEnable()
        {
            laserPointer = GetComponent<SteamVR_LaserPointer>();
            laserPointer.PointerIn -= HandlePointerIn;
            laserPointer.PointerIn += HandlePointerIn;
            laserPointer.PointerOut -= HandlePointerOut;
            laserPointer.PointerOut += HandlePointerOut;

            trackedController = GetComponent<SteamVR_TrackedController>();
            if (trackedController == null)
            {
                trackedController = GetComponentInParent<SteamVR_TrackedController>();
            }
            trackedController.TriggerClicked -= HandleTriggerClicked;
            trackedController.TriggerClicked += HandleTriggerClicked;
            trackedController.TriggerUnclicked -= HandleTriggerUnclicked;
            trackedController.TriggerUnclicked += HandleTriggerUnclicked;
        }

        private void HandleTriggerClicked(object sender, ClickedEventArgs e)
        {
            hairTriggerTriggered = true;
        }

        private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
        {
            hairTriggerTriggered = false;
        }

        private void HandlePointerIn(object sender, PointerEventArgs e)
        {
            if (e.target.tag == "MenuObject")
            {
                string goName = e.target.name;

                switch (goName)
                {
                    case "Continue":
                        Continue(true);
                        break;
                    case "StartGame":
                        StartGame(true);
                        break;
                    case "Credits":
                        Credits(true);
                        break;
                    case "QuitGame":
                        QuitGame(true);
                        break;
                }
            }
        }

        private void HandlePointerOut(object sender, PointerEventArgs e)
        {
            if (e.target.tag == "MenuObject")
            {
                string goName = e.target.name;

                switch (goName)
                {
                    case "Continue":
                        Continue(false);
                        break;
                    case "StartGame":
                        StartGame(false);
                        break;
                    case "Credits":
                        Credits(false);
                        break;
                    case "QuitGame":
                        QuitGame(false);
                        break;
                }
            }
        }

        private void QuitGame(bool activated)
        {
            if (activated)
            {
                exitLight.enabled = true;
                if (hairTriggerTriggered)
                {
                    Application.Quit();
                }
                //if (hairTriggerTriggered && !levelLoadingInitiated)
                //{
                //    levelLoadingInitiated = true;
                //    fadeImage.FadingWithCallback(1f, 2f, delegate
                //    {
                //        Application.Quit();
                //    });
                //}
            }

            else
            {
                exitLight.enabled = false;
            }
        }

        private void Credits(bool activated)
        {
            if (activated)
            {
                creditsLight.enabled = true;
                if (hairTriggerTriggered)
                {
                    SceneManager.LoadScene(creditScene);
                }

                //if (hairTriggerTriggered && !levelLoadingInitiated)
                //{
                //    levelLoadingInitiated = true;
                //    fadeImage.FadingWithCallback(1f, 2f, delegate
                //    {
                //        SceneManager.LoadScene(creditScene);
                //    });
                //}
            }

            else
            {
                creditsLight.enabled = false;
            }
        }

        private void StartGame(bool activated)
        {
            if (activated)
            {
                startLight.enabled = true;
                if (hairTriggerTriggered)
                {
                    VR_SceneSwitcher.Singleton.LoadScene("VR_TutorialScene");
                }

                //if (hairTriggerTriggered && !levelLoadingInitiated)
                //{
                //    levelLoadingInitiated = true;
                //    fadeImage.FadingWithCallback(1f, 2f, delegate
                //    {
                //        LoadLevelAsync();
                //    });
                //}
            }

            else
            {
                startLight.enabled = false;
            }
        }

        private void Continue(bool activated)
        {
            if (activated)
            {
                startLight.enabled = true;
            }

            else
            {
                startLight.enabled = false;
            }
        }

        //public void LoadLevelAsync()
        //{
        //    loadingImage.FadingWithCallback(1f, 2f, delegate
        //    {
        //        loadingCoin.SetActive(true);
        //    });
        //    StartCoroutine(LoadLevelCOR());
        //}

        //IEnumerator LoadLevelCOR()
        //{
        //    yield return new WaitForSeconds(1.5f);
        //    loadSceneOperator = SceneManager.LoadSceneAsync(gamePlayScene);
        //    loadSceneOperator.allowSceneActivation = false;
        //    while (!loadSceneOperator.isDone)
        //    {
        //        yield return 0;
        //        if (loadSceneOperator.progress >= 0.9f)
        //        {
        //            loadingImage.FadingWithCallback(0f, 2f, delegate
        //            {
        //                loadSceneOperator.allowSceneActivation = true;
        //            });
        //        }
        //    }
        //}
    }
}