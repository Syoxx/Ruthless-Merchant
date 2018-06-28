using System;
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
        private SteamVR_TrackedController trackedCon;

        private Vector2 prevTriggerPressed;

        void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
            trackedCon = GetComponent<SteamVR_TrackedController>();
        }

        void Start()
        {
            prevTriggerPressed = Vector2.zero;

            trackedCon.TriggerClicked += OnTriggerClicked;
            trackedCon.PadTouched += OnTouchpadTouched;
            trackedCon.PadClicked += OnTouchpadClicked;
            trackedCon.MenuButtonClicked += OnMenuButtonClicked;
            trackedCon.Gripped += OnGripped;
            trackedCon.SteamClicked += OnSteamClicked;
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

            //if (controller.GetTouch(touchpad))
            //{
            //    axis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            //    debugMessage += "Touchpad Touch Input! X: " + axis.x + ", Y: " + axis.y + "\n";
            //}

            //if (controller.GetPressDown(touchpad))
            //{
            //    axis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            //    debugMessage += "Touchpad Click Input! X: " + axis.x + ", Y: " + axis.y + "\n";
            //}

            Vector2 triggerPressed = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

            if (triggerPressed.x != prevTriggerPressed.x)
            {
                debugMessage += "Hairtrigger Pressed: " + triggerPressed + "\n";
            }

            prevTriggerPressed = triggerPressed;

            //if (triggerPressed.x > 0.96f)
            //{
            //    debugMessage += "Hairtrigger Click Input!" + "\n";
            //}

            //if (controller.GetPressDown(appMenuButton))
            //{
            //    debugMessage += "Application-Menu Button Input!" + "\n";
            //    TriggerPulse((ushort)10000);
            //}

            //if (controller.GetPressDown(gripButton))
            //{
            //    debugMessage += "Grip Button Input!" + "\n";
            //}

            Debug.Log(debugMessage);
        }

        void OnTriggerClicked(object sender, ClickedEventArgs e)
        {
            Debug.Log("Trigger clicked");
            TriggerPulse(1000000);
        }

        void OnTouchpadTouched(object sender, ClickedEventArgs e)
        {
            Debug.Log("Touchpad touched");
        }

        void OnTouchpadClicked(object sender, ClickedEventArgs e)
        {
            Debug.Log("Touchpad clicked");
        }

        void OnMenuButtonClicked(object sender, ClickedEventArgs e)
        {
            Debug.Log("MenuButton Clicked");
        }

        void OnGripped(object sender, ClickedEventArgs e)
        {
            Debug.Log("Gripped");
        }

        void OnSteamClicked(object sender, ClickedEventArgs e)
        {
            Debug.Log("Steam Clicked");
        }

        IEnumerator TriggerPulse(int microseconds)
        {
            ushort rest = 0;

            if(microseconds > 3500)
            {
                rest = (ushort)(microseconds - 3500);
                microseconds = 3500;
            }

            controller.TriggerHapticPulse((ushort)microseconds);

            if (rest > 0)
            {
                yield return new WaitForSeconds(0.0035f);
                StartCoroutine(TriggerPulse(rest));
            }

            yield return null;
        }
    }
}
