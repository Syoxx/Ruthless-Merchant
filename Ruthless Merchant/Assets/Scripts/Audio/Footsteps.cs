//---------------------------------------------------------------
// Author: Fabian Subat
//
//---------------------------------------------------------------

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
        private bool isGrounded, justJumped;
        private bool isCtrlPressed;
        private bool isShiftPressed;
        private FMOD.Studio.EventInstance stepSound;
        private FMOD.Studio.ParameterInstance floorMaterial;
        private Character character;
        

        [SerializeField, Tooltip("Enter the Gameobject of this character")]
        public GameObject movingCharacter;
        private Rigidbody characterRb;


        /// <summary>
        /// Checks if the player is currently not airborne
        /// </summary>
        public bool IsGrounded
        {
            get { return isGrounded;  }
            set
            {
                isGrounded = value;
                if(isGrounded && !justJumped)
                {
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Steps/GroundLanding", GetComponent<Transform>().position);
                    justJumped = true;
                }
                if (!isGrounded)
                {
                    justJumped = false;
                }
            }
        }

        /// <summary>
        /// Called regularily to fire the event specified in the Editor
        /// </summary>
        void CallFootsteps()
        {
            if (playerismoving == true && isGrounded)
            {
                //Debug.Log("Stepsound playing");
                FMODUnity.RuntimeManager.PlayOneShot(Steps, movingCharacter.GetComponent<Transform>().position);
            }
        }

        void Start()
        {
            InvokeRepeating("CallFootsteps", 0, stepSpeed);
            stepSound = FMODUnity.RuntimeManager.CreateInstance(Steps);
            initialStepSpeed = stepSpeed;
            characterRb = movingCharacter.GetComponent<Rigidbody>();

        }

        /// <summary>
        /// Checks if player is airbourne and if he's moving
        /// </summary>
        void Update()
        {
            IsGrounded = !movingCharacter.GetComponent<Player>().JustJumped;

            if (gameObject.tag == "Player")
            {
                if (characterRb.velocity.x > 0.05f || characterRb.velocity.y > 0.05f || characterRb.velocity.z > 0.05f)
                {
                    playerismoving = true;
                }
                else if (characterRb.velocity.x <= 0.05f || characterRb.velocity.y <= 0.05f)
                {
                    playerismoving = false;
                }
                checkMovementStatus();
            }

        }

        /// <summary>
        /// Handles the rate at which Stepsounds are played depending on the state of movement
        /// </summary>
        private void checkMovementStatus()
        {
            if (Input.GetKey(KeyCode.LeftControl) && !isCtrlPressed)
            {
                //Debug.Log("Player crouches");
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
                //Debug.Log("Player sprints");
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

        private void OnTriggerEnter(Collider other)
        {
            if(other.name == "TradeTrigger" && Steps != "event:/Characters/Steps/StepsGrass")
            {
                Steps = "event:/Characters/Steps/StepsGrass";
                stepSound = FMODUnity.RuntimeManager.CreateInstance(Steps);
            }
        }
    }
}