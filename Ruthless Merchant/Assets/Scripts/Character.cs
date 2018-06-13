﻿using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Character : InteractiveObject
    {
        private DamageAbleObject healthSystem;
        private Vector3 velocity;
        private int stamina;
        private int maxStamina;
        private int staminaRegeneration;
        private GearSystem gearSystem;

        private StatValue[] characterStats;
        private StaminaController staminaController;
        private float maxJumpHeight;

        private Rigidbody rb;
        private bool isPlayer;

        private float attackDelay = 2f;
        private float elapsedAttackTime = 2f;

        public bool IsPlayer
        {
            get { return isPlayer; }
        }

        [SerializeField]
        [Range(0, 1000)]
        protected float walkSpeed = 2;
 
        [SerializeField]
        [Range(0, 1000)]
        protected float runSpeed = 4;

        public float WalkSpeed
        {
            get
            {
                return walkSpeed;
            }
        }

        public float RunSpeed
        {
            get
            {
                return runSpeed;
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

        public DamageAbleObject HealthSystem
        {
            get
            {
                if (healthSystem == null)
                {
                    healthSystem = GetComponent<DamageAbleObject>();
                    healthSystem.OnDeath += HealthSystem_OnDeath;
                }

                return healthSystem;
            }
        }

        public override void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            if (CompareTag("Player"))
            {
                isPlayer = true;
            }
            else
            {
                isPlayer = false;
            }

            healthSystem = GetComponent<DamageAbleObject>();
            healthSystem.OnDeath += HealthSystem_OnDeath;
        }

        private void HealthSystem_OnDeath(object sender, System.EventArgs e)
        {
            DestroyInteractivObject();
        }

        public void Attack(DamageAbleObject dmg)
        {
            if (elapsedAttackTime >= attackDelay)
            {
                elapsedAttackTime = 0f;
                dmg.ChangeHealth(-13, this);
            }
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
            elapsedAttackTime += Time.deltaTime;
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
            rb.velocity = Vector3.up * JumpVelocity;
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
    }
}