using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace RuthlessMerchant
{
    public class VRControllerTriggerHandler : MonoBehaviour
    {
        #region

        public SteamVR_TrackedObject TrackedObject { get; private set; }
        public SteamVR_TrackedController TrackedController { get; private set; }

        public delegate void TriggerClickEventHandler(object source, EventArgs args);
        public delegate void TriggerUnclickEventHandler(object source, EventArgs args);

        public event TriggerClickEventHandler TriggerClicked;
        public event TriggerUnclickEventHandler TriggerUnclicked;
        
        /// <summary>
        /// If the controller's trigger is in a clicked state.
        /// </summary>
        public bool TriggerClick { get; private set; }

        /// <summary>
        /// The press amount of the controller's trigger.
        /// </summary>
        public float TriggerPressAmount { get; private set; }

        /// <summary>
        /// The press amount of the controller's trigger in the last frame.
        /// </summary>
        float prevTriggerPressed;

        SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)TrackedObject.index); } }
        #endregion


        // DEBUGGING PURPOSES
        [SerializeField]
        bool debugLog;

        /// <summary>
        /// Awake 
        /// </summary>
        void Awake()
        {
            TrackedObject = GetComponent<SteamVR_TrackedObject>();
            TrackedController = GetComponent<SteamVR_TrackedController>();
        }

        /// <summary>
        /// Start 
        /// </summary>
        /// <value>
        /// 
        /// </value>
        void Start()
        {
            prevTriggerPressed = 0;

            //TrackedController.PadTouched           += OnTouchpadTouched;
            //TrackedController.PadClicked           += OnTouchpadClicked;
            //TrackedController.MenuButtonClicked    += OnMenuButtonClicked;
            //TrackedController.Gripped              += OnGripped;
        }

        /// <summary>
        /// Handles the OnTriggerClicked and OnTriggerUnclicked event firing.
        /// </summary>
        void Update()
        {
            if (device == null)
            {
                Debug.LogError("Controller not initialized");
                return;
            }

            TriggerPressAmount = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;

            if (TriggerPressAmount != prevTriggerPressed)
            {
                if (debugLog)
                    Debug.Log("Trigger Pressed: " + TriggerPressAmount + "\n");

                if (TriggerPressAmount == 1)
                    OnTriggerClicked();

                else if(TriggerClick)
                    OnTriggerUnclicked();
            }

            prevTriggerPressed = TriggerPressAmount;
        }

        //NOTE: Change description
        /// <summary>
        /// Sets a bool true if the fuckintrigger is clicked
        /// </summary>
        protected virtual void OnTriggerClicked()
        {
            TriggerClick = true;

            if (debugLog)
                Debug.Log("Trigger Clicked!");

            if (TriggerClicked != null)
                TriggerClicked(this, EventArgs.Empty);
        }

        //NOTE: Change description
        /// <summary>
        /// Sets the bool false again if trigger isn't clicked anymore
        /// </summary>
        protected virtual void OnTriggerUnclicked()
        {
            TriggerClick = false;

            if (debugLog)
                Debug.Log("Trigger Unclicked!");

            if (TriggerUnclicked != null)
                TriggerUnclicked(this, EventArgs.Empty);
        }

        //void OnTouchpadTouched(object sender, ClickedEventArgs e)
        //{
        //    Debug.Log("Touchpad touched, X: " + e.padX + ", Y: " + e.padY);
        //}

        //void OnTouchpadClicked(object sender, ClickedEventArgs e)
        //{
        //    Debug.Log("Touchpad clicked, X: " + e.padX + ", Y: " + e.padY);
        //}

        //void OnMenuButtonClicked(object sender, ClickedEventArgs e)
        //{
        //    Debug.Log("MenuButton Clicked");
        //}

        //void OnGripped(object sender, ClickedEventArgs e)
        //{
        //    Debug.Log("Gripped");
        //}
    }
}
