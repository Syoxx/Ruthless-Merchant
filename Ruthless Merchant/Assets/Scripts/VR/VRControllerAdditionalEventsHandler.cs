using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace RuthlessMerchant
{
    public class VRControllerAdditionalEventsHandler : MonoBehaviour
    {
        public delegate void TriggerClickEventHandler(object source, EventArgs args);
        public event TriggerClickEventHandler TriggerClicked;

        private SteamVR_TrackedObject trackedObj;
        private SteamVR_TrackedController trackedCon;
        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

        public Vector2 TriggerPressed;
        private Vector2 prevTriggerPressed;

        void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
            trackedCon = GetComponent<SteamVR_TrackedController>();
        }

        void Start()
        {
            prevTriggerPressed = Vector2.zero;

            trackedCon.PadTouched           += OnTouchpadTouched;
            trackedCon.PadClicked           += OnTouchpadClicked;
            trackedCon.MenuButtonClicked    += OnMenuButtonClicked;
            trackedCon.Gripped              += OnGripped;
        }

        void Update()
        {
            if (controller == null)
            {
                Debug.LogError("Controller not initialized");
                return;
            }

            TriggerPressed = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

            if (TriggerPressed.x != prevTriggerPressed.x)
            {
                Debug.Log("Trigger Pressed: " + TriggerPressed + "\n");
            }

            if (TriggerPressed.x > 0.96f)
            {
                OnTriggerClicked();
            }

            prevTriggerPressed = TriggerPressed;
        }

        protected virtual void OnTriggerClicked()
        {
            Debug.Log("Trigger clicked");

            if (TriggerClicked != null)
            {
                TriggerClicked(this, EventArgs.Empty);
            }
        }

        void OnTouchpadTouched(object sender, ClickedEventArgs e)
        {
            Debug.Log("Touchpad touched, X: " + e.padX + ", Y: " + e.padY);
        }

        void OnTouchpadClicked(object sender, ClickedEventArgs e)
        {
            Debug.Log("Touchpad clicked, X: " + e.padX + ", Y: " + e.padY);
        }

        void OnMenuButtonClicked(object sender, ClickedEventArgs e)
        {
            Debug.Log("MenuButton Clicked");
        }

        void OnGripped(object sender, ClickedEventArgs e)
        {
            Debug.Log("Gripped");
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
