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
    private GameObject pauseMenuUI;
    private GameObject currentState;
    private GameObject pauseMenu;
    private GameObject settingsMenu;
    private GameObject loadMenu;
    private GameObject saveMenu;
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
        pauseMenu = GameObject.Find("PauseMenuObject");
        settingsMenu = GameObject.Find("SettingsMenuObject");
        loadMenu = GameObject.Find("LoadMenuObject");
        saveMenu = GameObject.Find("SaveMenuObject");
        currentState = pauseMenu;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("TS_Scene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        loadMenu.SetActive(false);
        saveMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 0f;
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
        GameObject newState = pauseMenu;

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
        }

        currentState.SetActive(false);
        currentState = newState;
        currentState.SetActive(true);
    }
    #endregion
}
