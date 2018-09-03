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
    private Light lightContinue, lightStartGame, lightCredits, lightExit;

    [SerializeField]
    private float rayDistance = 3;

    [SerializeField, Tooltip("Time used to Fade")]
    private float fadeTime = 2f;

    [SerializeField, Tooltip("Image used for fading")]
    Image fadeImage;

    [SerializeField, Tooltip("Image shown while loading main Scene")]
    Image loadingImage;

    [SerializeField, Tooltip("Coin Object on the LoadingScreen")]
    GameObject loadingCoin;

    private Camera playerAttachedCamera;
    private AsyncOperation loadSceneOperator;
    private bool levelLoadingInitiated = false;

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
        lightStartGame.enabled = false;
        lightContinue.enabled = false;
        lightCredits.enabled = false;
        lightExit.enabled = false;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.transform.tag.Equals("MenuObject"))
            {
                
                    switch (hit.collider.gameObject.name)
                    {
                        case "StartGame":
                            startGame();
                            break;
                        case "QuitGame":
                            QuitGame();
                            break;
                        case "Continue":
                            Continue();
                            break;
                        case "Credits":
                            Credits();
                            break;
                    }
            }
        }
    }

    private void Continue()
    {
        lightContinue.enabled = true;

    }
    private void startGame()
    {
        lightStartGame.enabled = true;
        if (Input.GetMouseButtonDown(0) && !levelLoadingInitiated)
        {
            levelLoadingInitiated = true;
            fadeImage.FadingWithCallback(1f, fadeTime, delegate
            {
                LoadLevelAsync();
            });
        }
    }

    private void QuitGame()
    {
        lightExit.enabled = true;
        if (Input.GetMouseButtonDown(0))
        {
            fadeImage.FadingWithCallback(1f, fadeTime, delegate
            {
                Application.Quit();
            });
        }
    }

    private void Credits()
    {
        lightCredits.enabled = true;
        fadeImage.FadingWithCallback(0f, 2f, delegate
         {
             SceneManager.LoadScene("Credits");
         });
    }

    public void LoadLevelAsync()
    {
        loadingCoin.SetActive(true);
        loadingImage.color = Color.white;
        fadeImage.FadingWithCallback(0f, 2f, delegate
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
                fadeImage.FadingWithCallback(1f, 2f, delegate
                {
                    loadSceneOperator.allowSceneActivation = true;
                });
            }
        }
    }

}
