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

    private Vector2 padAxisVector;

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
            padAxisVector = new Vector2(device.GetAxis().x,device.GetAxis().y);

            Debug.Log("VECTOR: " + padAxisVector);
            Debug.Log("ClickedPad Vectors: " + device.GetAxis().x + " " + device.GetAxis().y);
        }


        //Trackpad Down (0, -1)
        if (device.GetAxis().x > -0.3 && device.GetAxis().x < 0.3 && device.GetAxis().y < -0.8)
        {
            Debug.Log("Trackpad Down clicked.");
            //Put scene switch here
        }

        //Trackpad Up (0, +1)
        if (device.GetAxis().x > -0.3 && device.GetAxis().x < 0.3 && device.GetAxis().y > +0.8)
        {
            Debug.Log("Trackpad Up clicked.");
            //Put scene switch here
        }

        //Trackpad Left (-1, 0)
        if (device.GetAxis().y > -0.3 && device.GetAxis().y < 0.3 && device.GetAxis().x < -0.8)
        {
            Debug.Log("Trackpad Left clicked.");
            //Put scene switch here
        }

        //Trackpad Right (+1, 0)
        if (device.GetAxis().y > -0.3 && device.GetAxis().y < 0.3 && device.GetAxis().x > 0.8)
        {
            Debug.Log("Trackpad Right clicked.");
            //Put scene switch here
        }
    }

    

    // Update is called once per frame
    void Update ()
    {
        device = SteamVR_Controller.Input((int) trackedObj.index);
        
    }
}
