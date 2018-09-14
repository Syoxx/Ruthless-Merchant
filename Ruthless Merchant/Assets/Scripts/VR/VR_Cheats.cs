using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple script implements developer tool "cheats" to switch scenes with trackpad of left controller.
/// </summary>
public class VR_Cheats : MonoBehaviour
{
    // Variables to get controller
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private SteamVR_TrackedController controller;

    // Variable to see the vectors of trackpad axis
    private Vector2 padAxisVector;
    
    /// <summary>
    /// Initialize tracked objects and controller to put events on it
    /// </summary>
	void Start ()
	{
	    trackedObj = GetComponent<SteamVR_TrackedObject>();

	    controller = GetComponent<SteamVR_TrackedController>();
        controller.PadClicked += Controller_PadClicked;
	}

    /// <summary>
    /// Scene switcher which switches scene if hair trigger is pressed and pad is clicked in a direction, e.g. haitrigger + trackpad left.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Controller_PadClicked(object sender, ClickedEventArgs e)
    {
        if (device.GetAxis().x != 0 || device.GetAxis().y != 0)
        {
            padAxisVector = new Vector2(device.GetAxis().x,device.GetAxis().y);

            Debug.Log("VECTOR: " + padAxisVector);
            Debug.Log("ClickedPad Vectors: " + device.GetAxis().x + " " + device.GetAxis().y);
        }


        // If hairtrigger and trackpad Down (0, -1) pressed switch scene
        if (device.GetAxis().x > -0.3 && device.GetAxis().x < 0.3 && device.GetAxis().y < -0.8)
        {
            Debug.Log("Trackpad Down clicked.");
            // Switch to VR_NewMainMenu
            SceneManager.LoadScene("VR_NewMainMenu");
        }

        // If hairtrigger and trackpad Up (0, +1) pressed switch scene
        if (device.GetAxis().x > -0.3 && device.GetAxis().x < 0.3 && device.GetAxis().y > +0.8)
        {
            Debug.Log("Trackpad Up clicked.");
            // Switch to VR_TutorialScene
            SceneManager.LoadScene("VR_TutorialScene");
        }

        // If hairtrigger and trackpad Left (-1, 0) pressed switch scene
        if (device.GetAxis().y > -0.3 && device.GetAxis().y < 0.3 && device.GetAxis().x < -0.8)
        {
            Debug.Log("Trackpad Left clicked.");
            // Switch to VR_SmithTradeScene
            //NOTE: Put correct scene name in here
            //SceneManager.LoadScene("VR_TradeScene");
        }

        // If hairtrigger and trackpad right (+1, 0) pressed switch scene
        if (device.GetAxis().y > -0.3 && device.GetAxis().y < 0.3 && device.GetAxis().x > 0.8)
        {
            Debug.Log("Trackpad Right clicked.");
            // Switch to VR_IslandScene
            SceneManager.LoadScene("VR_IslandScene");
        }
    }



    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

    }
}
