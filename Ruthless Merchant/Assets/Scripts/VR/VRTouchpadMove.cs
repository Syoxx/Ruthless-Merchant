using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace RuthlessMerchant
{
    public class VRTouchpadMove : MonoBehaviour
    {
        [SerializeField] private Transform rig;

        ulong touchpad = SteamVR_Controller.ButtonMask.Touchpad;
        ulong appMenuButton = SteamVR_Controller.ButtonMask.ApplicationMenu;
        ulong gripButton = SteamVR_Controller.ButtonMask.Grip;

        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
        private SteamVR_TrackedObject trackedObj;

        // Set axis values to zero
        private Vector2 axis = Vector2.zero;

        float debugTimer = 0;
        float debugTimerLimit = 0.25f;

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

                if (rig != null)
                {
                    rig.position += (transform.right * axis.x + transform.forward * axis.y) * Time.deltaTime;
                    rig.position = new Vector3(rig.position.x, 0, rig.position.z);
                }
            }

            if(controller.GetPressDown(touchpad))
            {
                axis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                debugMessage += "Touchpad Click Input! X: " + axis.x + ", Y: " + axis.y + "\n";
            }

            if (controller.hairTriggerDelta != 0)
            {
                debugMessage += "Hairtrigger Input! Delta: " + controller.hairTriggerDelta;
            }

            if (controller.GetHairTriggerDown())
            {
                debugMessage += "Hairtrigger Click Input!" + "\n";
                controller.TriggerHapticPulse(50);
            }

            if(controller.GetPressDown(appMenuButton))
            {
                debugMessage += "Application-Menu Button Input!" + "\n";
                controller.TriggerHapticPulse(200);
            }

            if (controller.GetPressDown(gripButton))
            {
                debugMessage += "Grip Button Input!" + "\n";
                controller.TriggerHapticPulse(100);
            }

            debugTimer += Time.deltaTime;

            if (debugMessage != "" && debugTimer > debugTimerLimit)
            {
                Debug.Log(debugMessage);
                debugTimer = 0;
            }
        }
    }

}