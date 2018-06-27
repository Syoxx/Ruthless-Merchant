using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace RuthlessMerchant
{
    public class VRControllerInputDebug : MonoBehaviour
    {
        [SerializeField] private Transform rig;

        ulong touchpad = SteamVR_Controller.ButtonMask.Touchpad;
        ulong appMenuButton = SteamVR_Controller.ButtonMask.ApplicationMenu;
        ulong gripButton = SteamVR_Controller.ButtonMask.Grip;

        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
        private SteamVR_TrackedObject trackedObj;

        // Set axis values to zero
        private Vector2 axis = Vector2.zero;

        void Start()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        void Update()
        {
            if (controller == null)
            {
                Debug.Log("Controller not initialized");
                return;
            }

            string debugMessage = "";
            var device = SteamVR_Controller.Input((int)trackedObj.index);

            if (controller.GetTouch(touchpad))
            {
                axis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                debugMessage += "Touchpad Touch Input! X: " + axis.x + ", Y: " + axis.y + "\n";
            }

            if (controller.GetPressDown(touchpad))
            {
                axis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                debugMessage += "Touchpad Click Input! X: " + axis.x + ", Y: " + axis.y + "\n";
            }

            Vector2 triggerPressed = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

            debugMessage += "Hairtrigger Pressed: " + triggerPressed + "\n";

            if (triggerPressed.x > 0.96f)
            {
                debugMessage += "Hairtrigger Click Input!" + "\n";
            }

            if (controller.GetPressDown(appMenuButton))
            {
                debugMessage += "Application-Menu Button Input!" + "\n";
                controller.TriggerHapticPulse(200);
            }

            if (controller.GetPressDown(gripButton))
            {
                debugMessage += "Grip Button Input!" + "\n";
            }

            Debug.Log(debugMessage);
        }
    }
}
