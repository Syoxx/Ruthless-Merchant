using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RuthlessMerchant;

public class VRMenuScript : MonoBehaviour
{
    [SerializeField, Tooltip("Name of the Scene which should be loaded to start the game")]
    private string gamePlayScene = "Islandtesting";

    [SerializeField]
    private Light lightContinue, lightStartGame, lightSettings;

    [SerializeField]
    private float rayDistance = 3;

    [SerializeField, Tooltip("Time used to Fade")]
    private float fadeTime = 2f;

    [SerializeField, Tooltip("Image used for fading")]
    Image fadeImage;

    [SerializeField, Tooltip("Image shown while loading main Scene")]
    Image loadingImage;

    [SerializeField, Tooltip("Drag a SwitchScenePNL there")]
    private GameObject PNL_BgSwitchScene;

    private Camera playerAttachedCamera;
    private AsyncOperation loadSceneOperator;

    // Use this for initialization
    void Start()
    {
        playerAttachedCamera = GetComponentInChildren<Camera>();
        if (fadeImage == null)
            fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
        if (loadingImage == null)
            loadingImage = GameObject.FindGameObjectWithTag("LoadingImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = playerAttachedCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Debug.DrawRay(transform.position, ray.direction, Color.green);

        lightStartGame.enabled = false;
        lightContinue.enabled = false;
        lightSettings.enabled = false;

        if (!PNL_BgSwitchScene.activeSelf)
        {
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.transform.tag.Equals("MenuObject"))
                {
                
                    switch (hit.collider.gameObject.name)
                    {
                        case "StartGame":
                            startGame();
                            break;
                        case "Settings":
                            Settings();
                            break;
                        case "QuitGame":
                            QuitGame();
                            break;
                        case "Continue":
                            Continue();
                            break;
                    }
                }
            }
        }
    }

    void Update()
    {
        OpenSceneSwitcher();
    }



    #region SwitchSceneManager

    public void OpenSceneSwitcher()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (PNL_BgSwitchScene.activeSelf)
            {
                PNL_BgSwitchScene.SetActive(false);
            }
            else
            {
                PNL_BgSwitchScene.SetActive(true);
            }
        }
    }
    public void Button1()
    {

    }
    public void Button2()
    {

    }
    public void Button3()
    {

    }
    public void Button4()
    {

    }
    #endregion
    private void Continue()
    {
        lightContinue.enabled = true;

    }
    private void startGame()
    {
        lightStartGame.enabled = true;
        if (Input.GetMouseButtonDown(0))
        {
            fadeImage.FadingWithCallback(1f, fadeTime, delegate
            {
                LoadLevelAsync();
            });
        }
    }

    private void QuitGame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fadeImage.FadingWithCallback(1f, fadeTime, delegate
            {
                Application.Quit();
            });
        }
    }

    private void Settings()
    {
        lightSettings.enabled = true;
    }

    public void LoadLevelAsync()
    {
        loadingImage.FadingWithCallback(1f, 2f, delegate
        {
            Debug.Log("displaying Load Image");
        });
        StartCoroutine(LoadLevelCOR());
    }

    IEnumerator LoadLevelCOR()
    {
        yield return new WaitForSeconds(1.5f);
        loadSceneOperator = SceneManager.LoadSceneAsync(gamePlayScene);
        loadSceneOperator.allowSceneActivation = false;
        while (!loadSceneOperator.isDone)
        {
            yield return 0;
            if (loadSceneOperator.progress >= 0.9f)
            {
                loadingImage.FadingWithCallback(0f, 2f, delegate
                {
                    loadSceneOperator.allowSceneActivation = true;
                });
            }
        }
    }
}
