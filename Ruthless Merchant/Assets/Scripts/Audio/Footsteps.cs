using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Footsteps : MonoBehaviour
    {

        [FMODUnity.EventRef]
        public string Steps;
        public float stepSpeed;

        private float initialStepSpeed;
        private string soundToPlay;
        private bool playerismoving;
        private bool isGrounded;
        private bool isCtrlPressed;
        private bool isShiftPressed;
        private FMOD.Studio.EventInstance stepSound;
        private FMOD.Studio.ParameterInstance floorMaterial;
        private Character character;

        [SerializeField, Tooltip("Enter the Gameobject of this character")]
        public GameObject movingCharacter;


        void CallFootsteps()
        {
            if (playerismoving == true && isGrounded)
            {
                //Debug.Log("Stepsound playing");
                FMODUnity.RuntimeManager.PlayOneShot(Steps, GetComponent<Transform>().position);
            }
        }

        void Start()
        {
            InvokeRepeating("CallFootsteps", 0, stepSpeed);
            stepSound = FMODUnity.RuntimeManager.CreateInstance(Steps);
            initialStepSpeed = stepSpeed;
        }

        void Update()
        {
            isGrounded = movingCharacter.GetComponent<Player>().IsGrounded;

            if(gameObject.tag == "Player")
            {
                checkMovementStatus();
            }

            if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
            {
                playerismoving = true;
            }
            else if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
            {
                playerismoving = false;
            }


        }

        /// <summary>
        /// Handles the rate at which Stepsounds are played depending on the state of movement
        /// </summary>
        private void checkMovementStatus()
        {
            if (Input.GetKey(KeyCode.LeftControl) && !isCtrlPressed)
            {
                isCtrlPressed = true;
                CancelInvoke("CallFootsteps");
                stepSpeed = stepSpeed * 1.5f;
                InvokeRepeating("CallFootsteps", 0, stepSpeed);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCtrlPressed = false;
                CancelInvoke("CallFootsteps");
                stepSpeed = initialStepSpeed;
                InvokeRepeating("CallFootsteps", 0, stepSpeed);
            }
            if (Input.GetKey(KeyCode.LeftShift) && !isShiftPressed)
            {
                isShiftPressed = true;
                CancelInvoke("CallFootsteps");
                stepSpeed = stepSpeed / 1.3f;
                InvokeRepeating("CallFootsteps", 0, stepSpeed);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isShiftPressed = false;
                CancelInvoke("CallFootsteps");
                stepSpeed = initialStepSpeed;
                InvokeRepeating("CallFootsteps", 0, stepSpeed);
            }
        }

        void OnDisable()
        {
            playerismoving = false;
        }
    }
}