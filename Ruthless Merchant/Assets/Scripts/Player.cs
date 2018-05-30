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

        [SerializeField]
        private float walkSpeed;

        [SerializeField]
        private float runSpeed;
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