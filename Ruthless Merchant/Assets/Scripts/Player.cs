using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Player : Character
    {
        #region Private Fields
        private UISystem uiSystem;
        private QuestManager questManager;
        private bool isCursorLocked = true;
        private int maxInteractDistance;
        private float moveSpeed;
        private float mouseXSensitivity = 1.8f;
        private float mouseYSensitivity = 1.8f;
        
        private Camera playerAttachedCamera;
        private Quaternion playerLookAngle;
        private Quaternion cameraPitchAngle;
        private Vector3 MoveVector = Vector3.zero;
        private Vector2 InputVector = Vector2.zero;

        [SerializeField]
        private float jumpSpeed = 10;
        #endregion

        public UISystem UISystem
        {
            get
            {
                return uiSystem;
            }
            set
            {
                uiSystem = value;
            }
        }

        public RuthlessMerchant.QuestManager QuestManager
        {
            get
            {
                return questManager;
            }
            set
            {
                questManager = value;
            }
        }

        public override void Start()
        {
            base.Start();
            maxInteractDistance = 4;

            playerLookAngle = transform.localRotation;

            // try to get the first person camera
            playerAttachedCamera = GetComponentInChildren<Camera>();
            
            if (playerAttachedCamera != null)
            {
                cameraPitchAngle = playerAttachedCamera.transform.localRotation;
            }
            else
            {
                Debug.Log("Player object does not have a first person camera.");
                isCursorLocked = false;
            }
        }

        public override void Update()
        {
            LookRotation();
            HandleInput();
        }

        /// <summary>
        /// Rotates view using mouse movement.
        /// </summary>
        private void LookRotation()
        {
            float yRot = Input.GetAxis("Mouse X") * mouseXSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * mouseYSensitivity;

            playerLookAngle *= Quaternion.Euler(0f, yRot, 0f);
            cameraPitchAngle *= Quaternion.Euler(-xRot, 0f, 0f);

            transform.localRotation = playerLookAngle;

            if (playerAttachedCamera != null)
            {
                playerAttachedCamera.transform.localRotation = cameraPitchAngle;
            }

            FocusCursor();
        }

        private void FocusCursor()
        {
            // Pressing escape makes cursor visible + unlocks it
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                isCursorLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isCursorLocked = true;
            }

            if(isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void ShowInventory()
        {
            throw new System.NotImplementedException();
        }

        public void ShowMap()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Checks for input to control the player character.
        /// </summary>
        public void HandleInput()
        {
            bool isWalking = false;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SendInteraction();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                base.Jump(jumpSpeed);
            }

            moveSpeed = isWalking ? walkSpeed : runSpeed;
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            InputVector = new Vector2(horizontal, vertical);

            if (InputVector.sqrMagnitude > 1)
            {
                InputVector.Normalize();
            }

            MoveVector = new Vector3(InputVector.x, 0.0f, InputVector.y);
            base.Move(MoveVector, moveSpeed);
        }

        public void SendInteraction()
        {
            if (playerAttachedCamera != null)
            {
                Ray clickRay = playerAttachedCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(clickRay, out hit, maxInteractDistance))
                {
                    Debug.Log(hit.collider.name + " " + hit.point + " clicked.");

                    InteractiveObject target = hit.collider.gameObject.GetComponent<InteractiveObject>();

                    if (target != null)
                    {
                        target.Interact(this.gameObject);
                    }
                }
            }
        }

        public override void Interact(GameObject caller)
        {
            throw new NotImplementedException();
        }

        public void Crouch()
        {
            throw new System.NotImplementedException();
        }

        public void Craft()
        {
            throw new System.NotImplementedException();
        }

        public void GetLoudness()
        {
            throw new System.NotImplementedException();
        }

        public void Sneak()
        {
            throw new System.NotImplementedException();
        }
    }
}