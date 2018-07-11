using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace RuthlessMerchant
{
    /// <summary>
    /// Menu helper class.
    /// Includes menu switcher, button handler, toggle handler and calls scene manager.
    /// </summary>
    public class VRMenuManager : MonoBehaviour
    {
        #region MenuSwitcher

        // Menu states
        private enum MenuStates
        {
            Main,
            Start,
            Load,
            Settings
        };

        private GameObject currentState;

        // Menu panel objects
        public GameObject mainMenu;
        public GameObject startMenu;
        public GameObject loadMenu;
        public GameObject settingsMenu;


        /// <summary>
        /// Set current state to main menu and set active on true.
        /// </summary>
        void Awake()
        {
            currentState = mainMenu;
            currentState.SetActive(true);
        }


        /// <summary>
        /// Take the desired MenuState to switch
        /// case on menu and determine the new state.
        /// </summary>
        private void SwitchMenu(MenuStates menu)
        {
            // GameObject to put a new state on it
            GameObject newState;

            switch (menu)
            {
                case MenuStates.Main:
                    // Sets new state to main menu
                    newState = mainMenu;
                    break;

                case MenuStates.Start:
                    // Sets new state to start game menu
                    newState = startMenu;
                    break;

                case MenuStates.Load:
                    // Sets new state to load game menu
                    newState = loadMenu;
                    break;

                case MenuStates.Settings:
                    // Sets new state to settings menu
                    newState = settingsMenu;
                    break;

                default:
                    newState = mainMenu;
                    break;
            }

            // Set the current state to inactive
            currentState.SetActive(false);
            // Reassign the current state to the new state
            currentState = newState;
            // Activate current state
            currentState.SetActive(true);
        }

        #endregion

        #region ButtonHandler

        /// <summary>
        /// Method for what happens if you click on start game button
        /// </summary>
        public void OnStartGameButton()
        {
            // Change menu state to start
            SwitchMenu(MenuStates.Start);
        }

        /// <summary>
        /// Method for what happens if you click on load game button
        /// </summary>
        public void OnLoadGameButton()
        {
            // Change menu state to load game
            SwitchMenu(MenuStates.Load);
        }

        /// <summary>
        /// Method for what happens if you click on options button
        /// </summary>
        public void OnSettingsButton()
        {
            // Change menu state to MenuStates.Options
            SwitchMenu(MenuStates.Settings);
        }

        /// <summary>
        /// Method for what happens if you click on back button
        /// </summary>
        public void OnBackButton()
        {
            // Change menu state
            SwitchMenu(MenuStates.Main);
        }

        /// <summary>
        /// Method for what happens if you click on exit button
        /// </summary>
        public void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion
}