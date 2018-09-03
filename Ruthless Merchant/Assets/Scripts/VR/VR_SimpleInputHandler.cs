using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_SimpleInputHandler : MonoBehaviour
{
    //// Controller Touchpad
    //private Valve.VR.EVRButtonId Button1 = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    //// Controller Hair Trigger
    //private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    //private SteamVR_Controller.Device controller {get { return SteamVR_Controller.Input((int) trackedObj.index); } }
    private SteamVR_TrackedController trackedObj;


    private GameObject pickup;

    // Use this for initialization
    void Start ()
    {
        trackedObj = GetComponent<SteamVR_TrackedController>();
        trackedObj.TriggerClicked += Grab;
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Grab(object sender, ClickedEventArgs e)
    {

    }

	// Update is called once per frame
	void Update ()
	{


	    //if (controller == null)
	    //{
     //       Debug.Log("Controller not initialized.");
	    //    return;
	    //}

     //   if (controller.GetTouchDown(Button1) && pickup != null)
     //   {
     //       pickup.transform.parent = this.transform;
     //       pickup.GetComponent<Rigidbody>().useGravity = false;
     //       pickup.GetComponent<Rigidbody>().isKinematic = true;
     //   }

	    //if (controller.GetTouchUp(Button1) && pickup != null)
	    //{
	    //    pickup.transform.parent = null;
	    //    pickup.GetComponent<Rigidbody>().useGravity = true;
	    //    pickup.GetComponent<Rigidbody>().isKinematic = false;
     //   }
	}
    private void OnTriggerEnter(Collider collider)
    {
        pickup = collider.gameObject;
    }

    private void OnTriggerExit(Collider collider)
    {
        pickup = null;
    }
}