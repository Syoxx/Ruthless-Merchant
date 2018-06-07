using UnityEngine;

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
        private Vector3 playerSize;
        private Vector3 boxSize;

        private Rigidbody rb;
        private bool isPlayer;

        public void Start()
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

            playerSize = GetComponent<BoxCollider>().size;
            boxSize = new Vector3(playerSize.x, groundedSkin, playerSize.z);
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

        public void Update()
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
            }
        }

        public void FixedUpdate()
        {

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

        public void Grounding(LayerMask layer)
        {
            Vector3 boxCenter = (Vector3)transform.position + Vector3.down * (playerSize.y + boxSize.y) * 0.5f; 
           // grounded = (Physics.OverlapBox(boxCenter, boxSize, Quaternion.identity, layer) != null);
            grounded = Physics.CheckBox(boxCenter, boxSize, Quaternion.identity, layer);
        }
    }
}