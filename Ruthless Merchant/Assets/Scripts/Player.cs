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
        private Vector3 MoveVector = Vector3.zero;

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

        private void Update()
        {
            HandleInput();
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }

            moveSpeed = isWalking ? walkSpeed : runSpeed;

            if (Input.GetKey("w"))
            {
                Debug.Log("w pressed");
                MoveVector = Vector3.forward;
                
            }
            else if (Input.GetKey("s"))
            {
                
                MoveVector = -Vector3.forward;
            }
            else
            {
                MoveVector.z = 0;
            }

            if (Input.GetKey("a"))
            {
                    MoveVector = Vector3.left;
            }
            else if (Input.GetKey("d"))
            {
                    MoveVector = Vector3.right;
            }
            else
            {
                MoveVector.x = 0;
            }

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