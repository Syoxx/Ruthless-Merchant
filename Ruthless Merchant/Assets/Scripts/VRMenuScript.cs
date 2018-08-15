using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RuthlessMerchant;

public class VRMenuScript : MonoBehaviour
{
    [SerializeField, Tooltip("Name of the Scene which should be loaded to start the game")]
    private string gamePlayScene = "main";

    [SerializeField]
    private Light lightContinue, lightStartGame, lightOptions;

    [SerializeField]
    private float rayDistance = 3;

    [SerializeField, Tooltip("Image used for fading")]
    Image fadeImage;

    private Camera playerAttachedCamera;

    // Use this for initialization
    void Start()
    {
        playerAttachedCamera = GetComponentInChildren<Camera>();
        if (fadeImage == null)
            fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = playerAttachedCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        lightStartGame.enabled = false;
        lightContinue.enabled = false;
        lightOptions.enabled = false;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.transform.tag.Equals("MenuObject"))
            {
                
                    switch (hit.collider.gameObject.name)
                    {
                        case "StartGame":
                            startGame();
                            break;
                        case "Options":
                            Options();
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

    private void Continue()
    {
        Debug.Log("Continue");
        lightContinue.enabled = true;

    }
    private void startGame()
    {
        fadeImage.FadingWithCallback(1f, 2f, delegate
        {
            SceneManager.LoadScene(gamePlayScene);
        });
        lightStartGame.enabled = true;
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame");
    }

    private void Options()
    {
        lightOptions.enabled = true;
        Debug.Log("Options");
    }
}
