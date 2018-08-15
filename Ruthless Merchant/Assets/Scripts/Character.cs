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
        private bool justJumped;
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

        [SerializeField, Range(0, 10), Tooltip("Base time between attacks")]
        protected float baseAttackDelay = 2;

        [SerializeField, Range(0, 1000), Tooltip("Base defense value")]
        protected int baseDefense = 10;

        [SerializeField, Range(0, 50.0f), Tooltip("Health Regneration per second")]
        private float healthRegPerSec = 0;
        private float healthRegValue = 0.0f;

        private float elapsedAttackTime = 2;
        private Weapon weapon;
        private Weapon shield;
        private Potion potion;

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

            if (isPlayer)
            {
                previousPosition = transform.position;
            }

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                //rb.useGravity = false;
                //rb.maxDepenetrationVelocity = 10f;
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

            gearSystem = new GearSystem(isPlayer);
        }

        private void HealthSystem_OnDeath(object sender, System.EventArgs e)
        {
            DestroyInteractivObject();
        }

        public void Attack(Character character)
        {
            if (elapsedAttackTime >= GetAttackDelay())
            {
                elapsedAttackTime = 0f;

                int damage = GetDamage() - character.GetDefense();
                if (damage <= 0)
                    damage = 1;

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

                //rb.velocity = new Vector3((rb.velocity.x < -walkSpeed) ? -walkSpeed : rb.velocity.x, rb.velocity.y, (rb.velocity.z < walkSpeed) ? -walkSpeed : rb.velocity.z);

            }
            else
            {
                rb.velocity = Vector3.right * rb.velocity.x / 2 + Vector3.up * rb.velocity.y + Vector3.forward * rb.velocity.z / 2;
            }

            //if (velocity != Vector2.zero && !isPlayer)
            //{ transform.rotation = Quaternion.LookRotation(velocity); }

            //rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            //moveVector.y = 0;
            //moveVector.x = velocity.x;
            //moveVector.z = velocity.y;

            //if (moveVector.sqrMagnitude > 1)
            //{
            //    moveVector.Normalize();
            //}

            //if (isPlayer) //Prevent penetration of terrain obstacles
            //{
            //    Ray forwardRay = new Ray(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z),
            //                    transform.TransformDirection(moveVector));
            //    RaycastHit forwardRayHit;

            //    if (Physics.Raycast(forwardRay, out forwardRayHit, 0.1f))
            //    {
            //        if (forwardRayHit.collider.gameObject.layer == 14)
            //        {
            //            // player does not move into obstacles of layer 14
            //        }
            //        else
            //        {
            //            transform.Translate(moveVector * speed * Time.deltaTime, Space.Self);
            //        }
            //    }
            //    else
            //    {
            //        transform.Translate(moveVector * speed * Time.deltaTime, Space.Self);
            //    }
            //}
            //else
            //{
            //    transform.Translate(moveVector * speed * Time.deltaTime, Space.Self);
            //}

            //moveVector = Vector3.zero;
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
            else /*if (justJumped == true && grounded)*/
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
                gravity = Vector3.zero;
                rb.AddForce(Vector3.up * 0.1f * maxJumpHeight, ForceMode.VelocityChange);
                elapsedSecs = 0.4f;
            }
        }

        protected virtual void FixedUpdate()
        {

            //rayHit = CheckCharGrounded(rayHit);

            //if (isPlayer)
            //{
            //    // check the terrain angle in four directions player could move
            //    preventClimbing = false;
            //    Vector3 rayOriginInPlayer = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);

            //    Ray rayLeft = new Ray(rayOriginInPlayer - transform.right * terrainCheckRadius, Vector3.down);
            //    Ray rayRight = new Ray(rayOriginInPlayer + transform.right * terrainCheckRadius, Vector3.down);
            //    Ray rayFront = new Ray(rayOriginInPlayer + transform.forward * terrainCheckRadius, Vector3.down);
            //    Ray rayBack = new Ray(rayOriginInPlayer - transform.forward * terrainCheckRadius, Vector3.down);

            //    is_climbable_left = CheckGroundAngle(rayLeft);
            //    is_climbable_right = CheckGroundAngle(rayRight);
            //    is_climbable_front = CheckGroundAngle(rayFront);
            //    is_climbable_back = CheckGroundAngle(rayBack);

            //    if (!is_climbable_left || !is_climbable_right || !is_climbable_front || !is_climbable_back)
            //    {
            //        preventClimbing = true;
            //    }
            //}
        }

        private void LateUpdate()
        {
            // Set gravity and prevent player climbing walls

            //if (isPlayer)
            //{
            //    if (preventClimbing)
            //    {
            //        if ((!is_climbable_front && moveVector.z > 0.5f) || (!is_climbable_back && moveVector.z < -0.5f)
            //                            || (!is_climbable_right && moveVector.x > 0.5f) || (!is_climbable_left && moveVector.x < -0.5f))
            //        {
            //            rb.transform.position = previousPosition;
            //        }
            //        else
            //        {
            //            previousPosition.x = transform.position.x;
            //            previousPosition.y = transform.position.y;
            //            previousPosition.z = transform.position.z;
            //        }
            //    }
            //    else
            //    {
            //        previousPosition.x = transform.position.x;
            //        previousPosition.y = transform.position.y;
            //        previousPosition.z = transform.position.z;
            //    }

            //}

            //// gravity for all characters
            //if (rb != null)
            //{
            //    if (grounded)
            //    {
            //        gravity = Vector3.zero;
            //    }
            //    else
            //    {
            //        gravity += globalGravityScale * Vector3.up * Time.deltaTime * 2f;
            //    }
            //    ApplyGravity(gravity);
            //}

            //// adjust y position
            //if (rayHit.point.y != 0)
            //{
            //    float offset = rayHit.point.y + yPositionOffset;

            //    if (!(justJumped) && grounded)
            //    {
            //        rb.transform.position = new Vector3(rb.transform.position.x, offset, rb.transform.position.z);
            //    }
            //}
            
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

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        //    {
        //        //grounded = true;
        //        gravity = Vector3.zero;
        //        Debug.Log("collision with terrain layer");
        //    }
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        //    {
        //        grounded = false;
        //    }
        //}

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