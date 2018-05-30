using System;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Player : Character
    {
        #region Private Fields
        private UISystem uiSystem;
        private QuestManager questManager;
        private int maxInteractDistance;
        private float moveSpeed;
        
        private Camera camera;
        private Quaternion playerLookAngle;
        private Quaternion cameraPitchAngle;
        private Vector3 MoveVector = Vector3.zero;
        private Vector2 InputVector = Vector2.zero;

        [SerializeField]
        private float jumpSpeed = 10;

        [SerializeField]
        private float walkSpeed = 2;

        [SerializeField]
        private float runSpeed = 4;
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

        private void Start()
        {
            base.Start();

            playerLookAngle = transform.localRotation;            
            // try to get the first person camera
            try
            {
                camera = GetComponentInChildren<Camera>();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }

            cameraPitchAngle = camera.transform.localRotation;
        }

        private void Update()
        {
            LookRotation();
            HandleInput();
        }

        private void LookRotation()
        {
            // TODO: Multiply with sensitivity value
            float yRot = Input.GetAxis("Mouse X");
            float xRot = Input.GetAxis("Mouse Y");

            playerLookAngle *= Quaternion.Euler(0f, yRot, 0f);
            cameraPitchAngle *= Quaternion.Euler(-xRot, 0f, 0f);

            transform.localRotation = playerLookAngle;

            if (camera != null)
            {
                camera.transform.localRotation = cameraPitchAngle;
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

        public void HandleInput()
        {
            bool isWalking = false;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
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

        public override void Interact()
        {
            throw new System.NotImplementedException();
        }

        public void Run()
        {
            // GD can alter player speed in inspector
            throw new System.NotImplementedException();
        }

        public void Walk()
        {
            throw new System.NotImplementedException();
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