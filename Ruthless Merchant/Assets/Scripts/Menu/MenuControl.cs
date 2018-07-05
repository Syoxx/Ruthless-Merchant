using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Fabian Subat, Nikolas Pietrek
/// </summary>
public class MenuControl : MonoBehaviour {

    #region Fields
    private bool gameIsPaused = false;
	[SerializeField]
	private string gamePlaySceneName, mainMenuSceneName;
    [SerializeField]
    private GameObject BookPrefab, pauseMenu, settingsMenu, loadMenu, saveMenu;
    private GameObject currentState;
    #endregion

    private enum MenuStates
    {
        Pause,
        Settings,
        Load,
        Save
    };

    #region Methods
    public void Awake()
    {
        currentState = pauseMenu;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(gamePlaySceneName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (gameIsPaused)
        //        Resume();
        //    else
        //    {
        //        Pause();
        //    }
        //}
    }

    public void Resume()
    {
        BookPrefab.SetActive(false);
        Time.timeScale = 1f;
        SwitchMenu(MenuStates.Pause);
        RuthlessMerchant.Player.RestrictCamera = false;
        gameIsPaused = false;
    }

    private void Pause()
    {
        BookPrefab.SetActive(true);
        Time.timeScale = 0f;
        RuthlessMerchant.Player.RestrictCamera = true;
        gameIsPaused = true;
    }

    public void SaveGameMenu()
    {
        SwitchMenu(MenuStates.Save);
    }

    public void LoadGameMenu()
    {
        SwitchMenu(MenuStates.Load);
    }

    public void SettingsMenu()
    {
        // Gets a button in settings menu
        Button aSettingsButton = null;
        if (settingsMenu.transform.GetChild(1) != null)
        {
            aSettingsButton = settingsMenu.transform.GetChild(1).gameObject.GetComponent<Button>();

            // Makes eventsystem select that button 
            if (aSettingsButton != null)
            {
                EventSystem.current.SetSelectedGameObject(aSettingsButton.gameObject);
            }
        }        

        SwitchMenu(MenuStates.Settings);
    }

    public void PauseMenu()
    {
        // Gets a button
        Button aMenuButton = null;
        if (pauseMenu.transform.GetChild(0) != null)
        {
            aMenuButton = pauseMenu.transform.GetChild(0).gameObject.GetComponent<Button>();

            // Makes eventsystem select that button
            if (aMenuButton != null)
            {
                EventSystem.current.SetSelectedGameObject(aMenuButton.gameObject);
            }
        }

        SwitchMenu(MenuStates.Pause);
    }

    private void SwitchMenu(MenuStates menu)
    {
        GameObject newState;

        switch (menu)
        {
            case MenuStates.Pause:
                newState = pauseMenu;
                break;
            case MenuStates.Settings:
                newState = settingsMenu;
                break;
            case MenuStates.Load:
                newState = loadMenu;
                break;
            case MenuStates.Save:
                newState = saveMenu;
                break;
            default:
                newState = pauseMenu;
                break;
        }

        currentState.SetActive(false);
        currentState = newState;
        currentState.SetActive(true);
    }
    #endregion
}
