﻿using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Character : InteractiveObject
    {
        private Vector3 velocity;
        private int stamina;
        private int maxStamina;
        private int staminaRegeneration;
        private GearSystem gearSystem;

        private StatValue[] characterStats;
        private StaminaController staminaController;
        private float maxJumpHeight;

        private static float globalGravityScale = -9.81f;
        private float groundedSkin = 0.05f;
        private bool grounded;
        //private Vector3 playerSize;
        private float playerRadius;
        private Vector3 boxSize;

        private Rigidbody rb;
        private bool isPlayer;

        [SerializeField]
        [Range(0, 1000)]
        protected float walkSpeed = 2;
 
        [SerializeField]
        [Range(0, 1000)]
        protected float runSpeed = 4;

        private float elapsedSecs;

        public override void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                rb.useGravity = false;
                
            }

            if (CompareTag("Player"))
            {
                isPlayer = true;
            }
            else
            {
                isPlayer = false;
            }

        }

        public StaminaController StaminaController
        {
            get
            {
                return staminaController;
            }
            set
            {
                staminaController = value;
            }
        }

        public GearSystem GearSystem
        {
            get
            {
                return gearSystem;
            }
            set
            {
                gearSystem = value;
            }
        }

        public StatValue[] CharacterStats
        {
            get
            {
                return characterStats;
            }
            set
            {
                characterStats = value;
            }
        }

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void Move(Vector3 velocity, float speed)
        {           
           if(velocity != Vector3.zero && !isPlayer)
                transform.rotation = Quaternion.LookRotation(velocity);

            transform.Translate(velocity * speed * Time.deltaTime, Space.Self);
        }

        public void Rotate()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            
        }

        public void Consume()
        {
            throw new System.NotImplementedException();
        }

        public void TrySteal()
        {
            throw new System.NotImplementedException();
        }

        public void Dodge()
        {
            throw new System.NotImplementedException();
        }

        public void Block()
        {
            throw new System.NotImplementedException();
        }

        public void Jump(float JumpVelocity)
        {
            if (grounded)
            {
                rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
                grounded = false;
                elapsedSecs = .5f;
            }
        }

        public void FixedUpdate()
        {
            if(elapsedSecs >= 0)
              elapsedSecs -= Time.deltaTime;
        }

        public void CalculateVelocity()
        {
            throw new System.NotImplementedException();
        }

        public void ChangeGear()
        {
            throw new System.NotImplementedException();
        }

        public void CalculateJumpHeight()
        {
            throw new System.NotImplementedException();
        }

        public void UseGravity(float gravityScale)
        {
            Vector3 gravity = globalGravityScale * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        public void Grounding(bool _grounded)
        {
            //if(Physics.CheckCapsule(GetComponent<Collider>().bounds.center, new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.min.y - 0.1f, GetComponent<Collider>().bounds.center.z), 0.5f) /*&& elapsedSecs <= 0*/)
            //{
            //    grounded = true;
            //}
            //else
            //    grounded = false;
            grounded = _grounded;
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                grounded = true;
                Debug.Log("true");
            }
            else
            {
                grounded = false;
                Debug.Log("false");
            }
        }
    }
}