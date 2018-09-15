//---------------------------------------------------------------
// Authors: Peter Ehmler, Richard Brönnimann, 
//---------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Character : InteractiveObject
    {
        private DamageAbleObject healthSystem;
        private Vector3 moveVector;
        private CharacterController charController;
        private int stamina;
        private int maxStamina;
        private int staminaRegeneration;

        private static float globalGravityScale = -9.81f;
        private float groundedSkin = 0.05f;
        private bool grounded;
        private bool justJumped;
        protected bool isDying = false;
        private RaycastHit rayHit;
        bool is_climbable_left;
        bool is_climbable_right;
        bool is_climbable_front;
        bool is_climbable_back;

        private float playerRadius;
        private Vector3 boxSize;

        private Rigidbody rb;
        private CapsuleCollider charCollider;
        private bool isPlayer;
        private bool preventClimbing = false;
        private Vector3 previousPosition;
        private float yPositionOffset;
        private float elapsedSecs;
        private float terrainCheckRadius;

        [Header("Character Attack Settings")]
        [SerializeField, Range(0, 1000), Tooltip("Base damage per attack")]
        protected int baseDamagePerAtk = 75;

        [SerializeField, Range(0.0f, 10.0f), Tooltip("Base time between attacks")]
        protected float baseAttackDelay = 2;

        [SerializeField, Range(0, 1000), Tooltip("Base defense value")]
        protected int baseDefense = 10;

        [SerializeField, Range(0, 50.0f), Tooltip("Health Regneration per second")]
        private float healthRegPerSec = 0;
        private float healthRegValue = 0.0f;

        private float elapsedAttackTime = 2;
        protected Weapon weapon;
        protected Weapon shield;
        protected Potion potion;

        protected Animator animator;

        public Weapon Weapon
        {
            get
            {
                return weapon;
            }
        }

        public Weapon Shield
        {
            get
            {
                return shield;
            }
        }

        public Potion Potion
        {
            get
            {
                return potion;
            }

            set
            {
                potion = value;
                if (potion != null)
                {
                    if (HealthSystem != null)
                    {
                        int diff = Convert.ToInt32(HealthSystem.MaxHealth * potion.Health) - HealthSystem.Health;
                        HealthSystem.MaxHealth = Convert.ToInt32(HealthSystem.MaxHealth * potion.Health);
                        HealthSystem.ChangeHealth(diff, this);
                    }
                    healthRegPerSec += potion.Regeneration;
                }
            }
        }

        public bool IsPlayer
        {
            get { return isPlayer; }
        }

        public bool IsDying
        {
            get
            {
                return isDying;
            }
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
        [Range(0, 5)]
        [Tooltip("Length of raycast to check if player is grounded")]
        private float groundCheckDistance = 0.25f;

        [SerializeField]
        [Tooltip("Layermask that is detected as ground.")]
        private LayerMask layerMask;

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

        public bool JustJumped
        {
            get { return justJumped; }
        }

        public bool IsGrounded
        {
            get
            {
                CheckCharGrounded(new RaycastHit());
                return grounded;
            }
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
                    if(healthSystem != null)
                        healthSystem.OnDeath += HealthSystem_OnDeath;
                }

                return healthSystem;
            }
        }

        public override void Start()
        {
            elapsedAttackTime = baseAttackDelay;

            if (isPlayer)
            {
                previousPosition = transform.position;
            }

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            if (charCollider == null)
            {
                charCollider = GetComponent<CapsuleCollider>();
                terrainCheckRadius = charCollider.radius / 4;
                yPositionOffset = (charCollider.height / 2) * transform.localScale.y;
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
            animator = gameObject.GetComponent<Animator>();
        }

        private void HealthSystem_OnDeath(object sender, System.EventArgs e)
        {
            if (!isPlayer)
            {
                isDying = true;
                if (animator != null)
                {
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsInFight", false);
                    animator.SetBool("IsAttacking", false);
                    animator.SetBool("IsDying", true);
                }
            }
            DestroyInteractiveObject(5.0f);
        }

        public bool Attack(Character character, float delay = 0.0f)
        {
            if (elapsedAttackTime >= GetAttackDelay())
            {
                elapsedAttackTime = 0f;

                int damage = GetDamage() - character.GetDefense();
                if (damage <= 0)
                    damage = 1;

                //FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Minions/NPC swords", this.gameObject.transform.position);
                
                if (delay > 0)
                    StartCoroutine(DoDamage(character, damage, delay));
                else
                    character.HealthSystem.ChangeHealth(-damage, this);

                return true;
            }

            return false;
        }

        private IEnumerator DoDamage(Character character, int damage, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!isDying && character != null && !character.isDying)
            {
                character.HealthSystem.ChangeHealth(-damage, this);
            }
        }

        public void Move(Vector2 velocity, float speed)
        {
            if(velocity != Vector2.zero)
            {
                Vector3 velocityVector = rb.velocity;

                velocityVector += transform.forward * velocity.y * speed;
                velocityVector += new Vector3(transform.forward.z, 0, -transform.forward.x) * velocity.x * speed;

                Vector2 rbspeed = new Vector2(velocityVector.x, velocityVector.z);
                float length = rbspeed.sqrMagnitude;
                if (length > Mathf.Pow(speed, 2))
                {
                    rbspeed = rbspeed.normalized * speed;
                    velocityVector = new Vector3(rbspeed.x, velocityVector.y, rbspeed.y);
                }
                rb.velocity = velocityVector;
            }
            else
            {
                rb.velocity = Vector3.right * rb.velocity.x / 2 + Vector3.up * rb.velocity.y + Vector3.forward * rb.velocity.z / 2;
            }
        }

        public void Rotate()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            elapsedAttackTime += Time.deltaTime;      

            if (elapsedSecs > 0)
            {
                elapsedSecs -= Time.deltaTime;
            }
            else 
            {
                justJumped = false;
            }
        }

        protected void Regeneration()
        {
            if (healthRegPerSec != 0)
            {
                healthRegValue += healthRegPerSec * Time.deltaTime;
                if (Math.Abs(healthRegValue) > 1.0f)
                {
                    int addValue = (int)Math.Floor(healthRegValue);
                    healthRegValue -= addValue;
                    HealthSystem.ChangeHealth(addValue, this);
                }
            }
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
            rayHit = CheckCharGrounded(rayHit);

            if (grounded)
            {
                grounded = false;
                justJumped = true;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * 0.1f * maxJumpHeight, ForceMode.VelocityChange);
                elapsedSecs = 0.4f;
            }
        }

        protected virtual void FixedUpdate()
        {

        }

        public override void DestroyInteractiveObject(float delay = 0)
        {
            base.DestroyInteractiveObject(delay);
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

            Physics.Raycast(ray, out hitInfo, 0.4f);

            if (hitInfo.collider != null)
            {
                float slopeAngle = Vector3.Angle(hitInfo.normal, Vector3.up);

                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    if (slopeAngle > maxSlopeAngle)
                    {
                        isClimbableAngle = false;
                    }
                }
            }

            return isClimbableAngle;
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

        /// <summary>
        /// Used to check if character is on the ground
        /// </summary>
        /// <returns>
        /// returns true if detects collision with relevant layer within the CheckDistance 
        /// (layerMask defines which layers are detected)
        /// </returns>
        private RaycastHit CheckCharGrounded(RaycastHit hitInfo)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, out hitInfo, groundCheckDistance);
            return hitInfo;
        }

        public float GetAttackDelay()
        {
            if (weapon != null && potion != null)
                return baseAttackDelay / (weapon.AttackSpeed * potion.AttackSpeed);
            else if (weapon != null)
                return baseAttackDelay / weapon.AttackSpeed;
            else if(potion != null)
                return baseAttackDelay / potion.AttackSpeed;

            return baseAttackDelay;
        }

        public int GetDamage()
        {
            int damage = baseDamagePerAtk;

            if (weapon != null)
                damage += weapon.Damage;

            if (shield != null)
                damage += shield.Damage;

            return damage;
        }

        public int GetDefense()
        {
            int defense = baseDefense;

            if (weapon != null)
                defense += weapon.DefencePower;

            if (shield != null)
                defense += shield.DefencePower;

            if (potion != null)
                defense += potion.DefenseValue;

            return defense;
        }
    }
}