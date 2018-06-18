using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Character : InteractiveObject
    {
        private DamageAbleObject healthSystem;
        //private Vector3 velocity;
        private Vector3 moveVector;
        private CharacterController charController;
        private int stamina;
        private int maxStamina;
        private int staminaRegeneration;
        private GearSystem gearSystem;

        private StatValue[] characterStats;
        private StaminaController staminaController;

        private static float globalGravityScale = -9.81f;
        private float groundedSkin = 0.05f;
        private bool grounded;

        private float playerRadius;
        private Vector3 boxSize;

        private Rigidbody rb;
        private bool isPlayer;
        private float elapsedSecs;

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

        [SerializeField]
        [Range(0, 1000)]
        private float stickToGroundValue;

        [SerializeField]
        [Range(0, 1000)]
        private float maxJumpHeight;

        private Vector3 gravity;

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

        public bool IsGrounded
        {
            get { return grounded; }
        }

        public CharacterController CharController
        {
            get { return charController; }
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

            gravity = Vector3.zero;

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

        public void Move(Vector2 velocity, float speed)
        {
            if (velocity != Vector2.zero && !isPlayer)
            { transform.rotation = Quaternion.LookRotation(velocity); }

            moveVector = Vector3.zero;
            moveVector.x = velocity.x;
            moveVector.z = velocity.y;

            if (moveVector.sqrMagnitude > 1)
            {
                moveVector.Normalize();
            }
            
            transform.Translate(moveVector * speed * Time.fixedDeltaTime, Space.Self);
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

        public void Jump()
        {
            if (grounded)
            {
                if (elapsedSecs <= 0)
                {
                    rb.AddForce(Vector3.up * Mathf.Sqrt(maxJumpHeight), ForceMode.VelocityChange);
                    grounded = false;
                    elapsedSecs = 2f;

                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if(elapsedSecs >= 0)
            {
                elapsedSecs -= Time.deltaTime;
            }
            
            if (grounded)
            {
                gravity = Vector3.zero;
                gravity.y = -stickToGroundValue * 0.1f;
                ApplyGravity(gravity);
            }
            else
            {
                gravity += globalGravityScale * Vector3.up * Time.deltaTime;
                ApplyGravity(gravity);
            }
            
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

        public void ApplyGravity(Vector3 gravity)
        {            
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        public void Grounding(bool _grounded)
        {
            grounded = _grounded;
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                grounded = true;
                Debug.Log("true");
            }
        }
    }
}