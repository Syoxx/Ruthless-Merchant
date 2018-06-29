using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Fabian Subat, Nikolas Pietrek
/// </summary>
public class MenuControl : MonoBehaviour {

    #region Fields
    private bool gameIsPaused = false;
	[SerializeField]
	private string gamePlaySceneName, mainMenuSceneName;
    [SerializeField]
    private GameObject pauseMenuUI, pauseMenu, settingsMenu, loadMenu, saveMenu;
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SwitchMenu(MenuStates.Pause);
        //Player.isCursorLocked = true;
        RuthlessMerchant.Player.IsPaused = false;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        //Player.isCursorLocked = false;
        RuthlessMerchant.Player.IsPaused = true;
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
        SwitchMenu(MenuStates.Settings);
    }

    public void PauseMenu()
    {
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
