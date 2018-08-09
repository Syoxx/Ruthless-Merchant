using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMenuScript : MonoBehaviour
{
    public Light lightContinue, lightStartGame, lightOptions;
    public float rayDistance = 3;
    private Camera playerAttachedCamera;
    // Use this for initialization
    void Start()
    {
        playerAttachedCamera = GetComponentInChildren<Camera>();
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
        Debug.Log("StartedGame");
        lightStartGame.enabled = true;
    }

    private void QuitGame()
    {
        Debug.Log("QuitGame");
    }

    private void Options()
    {
        lightOptions.enabled = true;
        Debug.Log("Options");
    }
}
