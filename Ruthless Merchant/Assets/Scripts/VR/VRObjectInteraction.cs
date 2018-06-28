using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class VRObjectInteraction : MonoBehaviour
    {
        private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

        private SteamVR_Controller.Device controller {  get { return SteamVR_Controller.Input((int)trackedObj.index);} }
        private SteamVR_TrackedObject trackedObj;

        private Valve.VR.VREvent_HapticVibration_t hapticVibration;
        
        // Use this for initialization
        void Start()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (controller == null)
            {
                Debug.Log("Controller not initialized.");
                return;
            }

            if (controller.GetPressDown(gripButton))
            {
                Debug.Log("Grip Button was just pressed.");
            }

            if (controller.GetPressUp(gripButton))
            {
                Debug.Log("Grip Button was just unpressed.");
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Trigger entered");
        }

        private void OnTriggerExit(Collider collider)
        {
            Debug.Log("Trigger exited.");
        }
    }
}