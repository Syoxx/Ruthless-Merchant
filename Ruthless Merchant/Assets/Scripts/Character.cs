//---------------------------------------------------------------
// Authors: Peter Ehmler, Richard Brönnimann, 
//---------------------------------------------------------------
using System;
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
        private CapsuleCollider charCollider;
        private bool isPlayer;
        private bool preventClimbing = false;
        private float elapsedSecs;
        private float terrainCheckRadius;
        private float colliderHeight;

        [Header("Character Attack Settings")]
        [SerializeField, Range(0, 1000), Tooltip("Base damage per attack")]
        protected int baseDamagePerAtk = 75;

        [SerializeField, Range(0, 10), Tooltip("Base time between attacks")]
        protected float baseAttackDelay = 2;

        [SerializeField, Range(0, 1000), Tooltip("Base defense value")]
        protected int baseDefense = 10;

        private float elapsedAttackTime = 2;

        public bool IsPlayer
        {
            get { return isPlayer; }
        }
        [SerializeField]
        [Range(0, 1000)]
        [Tooltip("Player speed while holding LCtrl.")]
        protected float sneakSpeed = 2;

        [Header("Character Movement Settings")]
        [SerializeField]
        [Range(0, 1000)]
        [Tooltip("Player speed only using WASD.")]
        protected float walkSpeed = 3;

        [SerializeField]
        [Range(0, 1000)]
        [Tooltip("Player speed while holding shift.")]
        protected float runSpeed = 6;

        [SerializeField]
        [Range(0, 1000)]
        [Tooltip("Constant downward force, used instead of gravity when on the ground.")]
        private float stickToGroundValue;

        [SerializeField]
        [Range(0, 1000)]
        [Tooltip("Affects how high player can jump.")]
        private float maxJumpHeight;

        [SerializeField]
        [Range(0, 90)]
        [Tooltip("Limits the terrain angle player can climb.")]
        private float maxSlopeAngle;

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
            elapsedAttackTime = baseAttackDelay;

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                rb.useGravity = false;
            }

            if (charCollider == null)
            {
                charCollider = GetComponent<CapsuleCollider>();
                terrainCheckRadius = charCollider.radius;
                colliderHeight = charCollider.height;
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

            gearSystem = new GearSystem(isPlayer);
        }

        private void HealthSystem_OnDeath(object sender, System.EventArgs e)
        {
            DestroyInteractivObject();
        }

        public void Attack(Character character)
        {
            if (elapsedAttackTime >= baseAttackDelay)
            {
                elapsedAttackTime = 0f;

                int damage = baseDamagePerAtk - baseDefense;
                if (damage <= 0)
                    damage = 1;

                character.HealthSystem.ChangeHealth(-damage, this);
            }
        }

        public void Move(Vector2 velocity, float speed)
        {
            if (velocity != Vector2.zero && !isPlayer)
            { transform.rotation = Quaternion.LookRotation(velocity); }

            moveVector = Vector3.zero;
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            moveVector.y = 0;
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
            if (grounded && !preventClimbing)
            {
                if (elapsedSecs <= 0)
                {
                    grounded = false;
                    gravity = Vector3.zero;
                    rb.AddForce(Vector3.up * Mathf.Sqrt(maxJumpHeight), ForceMode.VelocityChange);
                    elapsedSecs = 1f;
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (elapsedSecs >= 0)
            {
                elapsedSecs -= Time.deltaTime;
            }

            if (isPlayer)
            {
                preventClimbing = false;
                                
                Ray rayLeft     = new Ray(new Vector3(transform.localPosition.x - terrainCheckRadius, transform.localPosition.y + 0.1f, transform.localPosition.z), Vector3.down);
                Ray rayRight    = new Ray(new Vector3(transform.localPosition.x + terrainCheckRadius, transform.localPosition.y + 0.1f, transform.localPosition.z), Vector3.down);
                Ray rayFront    = new Ray(new Vector3(transform.localPosition.x, transform.localPosition.y + 0.1f, transform.localPosition.z + terrainCheckRadius), Vector3.down);
                Ray rayBack     = new Ray(new Vector3(transform.localPosition.x, transform.localPosition.y + 0.1f, transform.localPosition.z - terrainCheckRadius), Vector3.down);
                
                bool is_climbable_left = CheckGroundAngle(rayLeft);
                bool is_climbable_right = CheckGroundAngle(rayRight);
                bool is_climbable_front = CheckGroundAngle(rayFront);
                bool is_climbable_back = CheckGroundAngle(rayBack);

                if (!is_climbable_left || !is_climbable_right || !is_climbable_front || !is_climbable_back)
                {
                    preventClimbing = true;
                }
            }
            


            if (grounded && !preventClimbing)
            {if (rb != null)
                {
                    gravity = Vector3.zero;
                    gravity.y = -stickToGroundValue;
                    ApplyGravity(gravity);
                }
            }
            else
            {
                if (rb != null)
                {
                    if (preventClimbing)
                    {
                        gravity.y -= 20;
                    }

                    gravity += globalGravityScale * Vector3.up * Time.deltaTime * 2f;
                    ApplyGravity(gravity);
                }
            }

        }

        /// <summary>
        /// Use raycast to check angle of ground beneath character.
        /// </summary>
        /// <returns>
        /// Returns true if angle is less than maxSlopeAngle.
        /// </returns>
        private bool CheckGroundAngle(Ray ray)
        {
            RaycastHit hitInfo;
            bool isClimbableAngle = true;

            Physics.Raycast(ray, out hitInfo, 4f);

            if (hitInfo.collider != null)
            {
                float slopeAngle = Vector3.Angle(hitInfo.normal, Vector3.up);

                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    if (slopeAngle > maxSlopeAngle)
                    {
                        isClimbableAngle = false;
                    }
                }
            }          

            return isClimbableAngle;
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
            if (gravity.y > 0)
            {
                gravity.y = 0;
            }
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
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                grounded = false;
            }
        }
    }
}