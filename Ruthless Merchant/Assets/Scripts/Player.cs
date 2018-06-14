using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Player : Character
    {
        public static Player Singleton;

        #region Private Fields
        private UISystem uiSystem;
        private QuestManager questManager;
        private bool isCursorLocked = true;
        private bool showingInventory = false;
        private int maxInteractDistance;
        private float moveSpeed;
        private float mouseXSensitivity = 2f;
        private float mouseYSensitivity = 2f;
        
        private Camera playerAttachedCamera;
        private Quaternion playerLookAngle;
        private Quaternion cameraPitchAngle;
        private Vector3 MoveVector = Vector3.zero;
        private Vector2 InputVector = Vector2.zero;
        private GameObject uiCanvas;
        private GameObject itemsContainer;

        [SerializeField]
        private float jumpSpeed = 10.0f;

        [SerializeField]
        private GameObject ItemsParent;

        [SerializeField]
        private GameObject ItemUIPrefab;
        #endregion

        [SerializeField] private GameObject mapObject;
        private Transform Teleport1;
        [SerializeField]
        private Transform Teleport2;
        [SerializeField]
        private Transform Teleport3;
        [SerializeField]
        private Transform Teleport4;


        [SerializeField]
        private float gravityScale = 1.0f;
        [SerializeField]
        private LayerMask layermask;

        [SerializeField]
        private Transform teleportTarget;

        private bool hasJumped;
        
        #region MonoBehaviour Life Cycle

        private void Awake()
        {
            Singleton = this;
        }

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

        public override void Start()
        {
            base.Start();

            if(ItemsParent != null)
            itemsContainer = ItemsParent.transform.parent.gameObject;
            if (itemsContainer != null)
            {
                uiCanvas = itemsContainer.transform.parent.gameObject;

                // Ensure hidden inventory

            /*if (uiCanvas.activeInHierarchy == true)
                {
                    ShowInventory(false);
                }
            */
            maxInteractDistance = 3;

            this.inventory = new Inventory();

            playerLookAngle = transform.localRotation;

            // try to get the first person camera
            playerAttachedCamera = GetComponentInChildren<Camera>();
            
            if (playerAttachedCamera != null)
            {
                cameraPitchAngle = playerAttachedCamera.transform.localRotation;
            }
            else
            {
                Debug.Log("Player object does not have a first person camera.");
                isCursorLocked = false;
            }
        }

        private void FixedUpdate()
        {
            if (hasJumped)
            {
                base.Jump(jumpSpeed);
                hasJumped = false;
            }
            else
                base.Grounding(layermask);
            base.UseGravity(gravityScale);
            base.FixedUpdate();


        }
        public override void Update()
        {
            LookRotation();
            HandleInput();
            
        }

        /// <summary>
        /// Rotates view using mouse movement.
        /// </summary>
        private void LookRotation()
        {
            float yRot = Input.GetAxis("Mouse X") * mouseXSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * mouseYSensitivity;

            playerLookAngle *= Quaternion.Euler(0f, yRot, 0f);
            cameraPitchAngle *= Quaternion.Euler(-xRot, 0f, 0f);

            transform.localRotation = playerLookAngle;

            if (playerAttachedCamera != null)
            {
                playerAttachedCamera.transform.localRotation = cameraPitchAngle;
            }

            FocusCursor();
        }

        private void FocusCursor()
        {
            // Pressing escape makes cursor visible + unlocks it
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                isCursorLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isCursorLocked = true;
            }

            if(isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void ShowInventory()
        {   //Pre-Refactored Code. Daniel/Richard
            /* 
            if (makeVisible)
            {
                PopulateInventoryPanel();
                uiCanvas.SetActive(true);
                showingInventory = true;
            }
            else
            {
                uiCanvas.SetActive(false);
                showingInventory = false;
            }
            */

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!uiCanvas.activeSelf)
                {                   
                    uiCanvas.SetActive(true);
                    PopulateInventoryPanel();
                }
                else
                {
                    uiCanvas.SetActive(false);
                }

            }

        }

        private void PopulateInventoryPanel()
        {
            if (inventory.inventorySlots.Length == 0)
            {
                return;
            }
            else
            {
                // Delete all objects in inventory UI
                foreach (Transform child in ItemsParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            // Create inventory list objects
            for (int itemIndex = 0; itemIndex < inventory.inventorySlots.Length; itemIndex++)
            {
                if (inventory.inventorySlots[itemIndex].Item == null)
                {
                    continue;
                }

                GameObject InventoryItem = Instantiate(ItemUIPrefab) as GameObject;
                InventoryItem.transform.SetParent(ItemsParent.transform, false);
                InventoryDisplayedData itemInfos = InventoryItem.GetComponent<InventoryDisplayedData>();
                itemInfos.itemName.text = inventory.inventorySlots[itemIndex].Item.Name;
                itemInfos.itemWeight.text = inventory.inventorySlots[itemIndex].Item.ItemWeight + " kg";
                itemInfos.itemDescription.text = inventory.inventorySlots[itemIndex].Item.Description;
                itemInfos.itemRarity.text = inventory.inventorySlots[itemIndex].Item.Rarity.ToString();
                itemInfos.itemPrice.text = inventory.inventorySlots[itemIndex].Item.Price + "G";

                if (inventory.inventorySlots[itemIndex].Item.ItemSprite != null)
                {
                    itemInfos.ItemImage.sprite = inventory.inventorySlots[itemIndex].Item.ItemSprite;
                }
            }
        }

        public void ShowMap()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                mapObject.SetActive(mapObject.activeSelf == false);
            }
        }

        /// <summary>
        /// Checks for input to control the player character.
        /// </summary>
        public void HandleInput()
        {
            bool isWalking = false;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                hasJumped = true;
               // base.Jump(jumpSpeed);
            }
            
            //Pre-Refactored Code Daniel/Richard
            /*
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!showingInventory)
                {
                    ShowInventory(true);
                }
                else
                {
                    ShowInventory(false);
                }
            }
            */
            if (Input.GetKey(KeyCode.Alpha1))
            {
                teleportTarget = Teleport1;
                Debug.Log("Teleport1 - Blue");
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                teleportTarget = Teleport2;
                Debug.Log("Teleport2 - Purple");
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                teleportTarget = Teleport3;
                Debug.Log("Teleport3 - Green");
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                teleportTarget = Teleport4;
                Debug.Log("Teleport4 - Yellow");
            }
            if(Input.GetKey(KeyCode.T) && teleportTarget != null)
            {
                Teleport(teleportTarget.position + new Vector3(0, 1));
            }
            moveSpeed = isWalking ? walkSpeed : runSpeed;
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal == 0 && vertical == 0)
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            InputVector = new Vector2(horizontal, vertical);

            if (InputVector.sqrMagnitude > 1)
            {
                InputVector.Normalize();
            }

            MoveVector = new Vector3(InputVector.x, 0.0f, InputVector.y);
            base.Move(MoveVector, moveSpeed);

            SendInteraction();
            ShowInventory();
            ShowMap();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Teleport") && teleportTarget != null)
            {
                Teleport(teleportTarget.position + new Vector3 (0,1));
            }
        }
        
        private void Teleport(Vector3 targetPos)
        {
            transform.position = targetPos;
        }

        public void SendInteraction()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerAttachedCamera != null)
                {
                    Ray clickRay = playerAttachedCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(clickRay, out hit, maxInteractDistance))
                    {
                        Debug.Log(hit.collider.name + " " + hit.point + " clicked.");

                        InteractiveObject target = hit.collider.gameObject.GetComponent<InteractiveObject>();

                        // Treat interaction target like an item                    
                        Item targetItem = target as Item;

                        if (targetItem != null)
                        {
                            // Picking up items and gear
                            if (targetItem.Type == ItemType.Weapon || targetItem.Type == ItemType.Gear || targetItem.Type == ItemType.ConsumAble)
                            {
                                Item clonedItem = targetItem.DeepCopy();

                                // Returns 0 if item was added to inventory
                                int UnsuccessfulPickup = inventory.Add(clonedItem, 1);

                                if (UnsuccessfulPickup != 0)
                                {
                                    Debug.Log("Returned " + UnsuccessfulPickup + ", failed to collect item.");
                                }
                                else
                                {
                                    targetItem.Destroy();
                                }
                            }
                        }
                        else
                        {
                            // Treat interaction target like an NPC
                            NPC targetNPC = target as NPC;

                            if (targetNPC != null)
                            {
                                target.Interact(this.gameObject);
                            }
                        }
                    }
                }
            }
        }

        public override void Interact(GameObject caller)
        {
            throw new NotImplementedException();
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
       
        
     