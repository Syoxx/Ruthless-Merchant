using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Character : InteractiveObject
    {
        private DamageAbleObject healthSystem;
        private Vector3 velocity;
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
        private bool isJumping;

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
        private CollisionFlags collisionFlags;

        [SerializeField]
        private float maxJumpHeight;

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
                //rb.useGravity = false;
            }

            if (CompareTag("Player"))
            {
                isPlayer = true;
            }
            else
            {
                isPlayer = false;
            }

            if (GetComponent<CharacterController>() != null)
            {
                charController = GetComponent<CharacterController>();
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

        public void Move(Vector2 velocity, float speed)
        {
            if (velocity != Vector2.zero && !isPlayer)
            { transform.rotation = Quaternion.LookRotation(velocity); }

            moveVector = transform.forward * velocity.y + transform.right * velocity.x;

            RaycastHit hitInfo;
            if (charController != null)
            {
                Physics.SphereCast(transform.position, charController.radius, Vector3.down, out hitInfo,
                               charController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

                moveVector = Vector3.ProjectOnPlane(moveVector, hitInfo.normal).normalized;

                if (isJumping)
                {
                    moveVector.y = maxJumpHeight;
                    isJumping = false;
                }

                collisionFlags = charController.Move(moveVector * speed * Time.deltaTime);
            } 
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
            //if (charController.isGrounded)
            {
                if (elapsedSecs <= 0)
                {
                    //rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
                    isJumping = true;
                    //grounded = false;
                    elapsedSecs = .5f;

                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if(elapsedSecs >= 0)
            {
                elapsedSecs -= Time.deltaTime;
            }

            if (CharController.isGrounded == false)
            {
                UseGravity();
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

        public void UseGravity(/*float gravityScale*/)
        {
            Vector3 gravity = 2 * globalGravityScale * Vector3.up * Time.deltaTime;
            //rb.AddForce(gravity, ForceMode.Acceleration);
            CharController.Move(gravity);
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
            else
            {
                grounded = false;
                Debug.Log("false");
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (collisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(charController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}