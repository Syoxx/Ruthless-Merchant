using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script implements developer tool to switch scenes with trackpad of right controller.
/// </summary>
public class VR_Cheats : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    private SteamVR_TrackedController controller;

	// Use this for initialization
	void Start ()
	{
	    trackedObj = GetComponent<SteamVR_TrackedObject>();

	    controller = GetComponent<SteamVR_TrackedController>();
        controller.PadClicked += Controller_PadClicked;
	}

    private void Controller_PadClicked(object sender, ClickedEventArgs e)
    {
        if (device.GetAxis().x != 0 || device.GetAxis().y != 0)
        {
            Debug.Log("ClickedPad Vectors: " + device.GetAxis().x + " " + device.GetAxis().y);
        }

        // Get axis variable and check value 
        //if(device.GetAxis().x > 0.85 && )
    }

    

    // Update is called once per frame
    void Update ()
    {
        device = SteamVR_Controller.Input((int) trackedObj.index);
        
    }
}
