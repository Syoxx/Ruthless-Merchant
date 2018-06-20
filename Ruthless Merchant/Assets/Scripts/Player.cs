//---------------------------------------------------------------
// Authors: Daniel Masly, Richard Brönnimann, Peter Ehmler
//---------------------------------------------------------------

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
        private bool hasJumped;
        private bool isCrouching;
        private bool isCtrlPressed;
        private bool wasCrouching;
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
        private Transform teleportTarget;
        private float crouchDelta;
        private float playerHeight;

        [SerializeField]
        [Tooltip("Tip: CrouchHeight must be smaller than the player collider's height.")]
        private float CrouchHeight;

        [SerializeField]
        private GameObject ItemsParent;

        [SerializeField]
        private GameObject ItemUIPrefab;

        [SerializeField]
        private GameObject mapObject;
        [SerializeField]
        private Transform Teleport1;
        [SerializeField]
        private Transform Teleport2;
        [SerializeField]
        private Transform Teleport3;
        [SerializeField]
        private Transform Teleport4;
        #endregion

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

            if (ItemsParent != null)
            {
                itemsContainer = ItemsParent.transform.parent.gameObject;
            }

            if (itemsContainer != null)
            {
                uiCanvas = itemsContainer.transform.parent.gameObject;
            }
            maxInteractDistance = 3;
            
            playerHeight = GetComponent<CapsuleCollider>().height;
            crouchDelta = playerHeight - CrouchHeight;
            
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

        protected override void FixedUpdate()
        {
            if (hasJumped)
            {
                Jump();
                hasJumped = false;
            }

            if (isCtrlPressed)
            {
                isCrouching = true;
                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    isCtrlPressed = false;
                }
            }

            Crouch();

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

            transform.localRotation = playerLookAngle;

            if (playerAttachedCamera != null)
            {
                Vector3 camRotation = playerAttachedCamera.transform.rotation.eulerAngles + new Vector3(-xRot, 0f, 0f);
                camRotation.x = ClampAngle(camRotation.x, -90f, 90f);
                playerAttachedCamera.transform.eulerAngles = camRotation;
            }
            

            FocusCursor();
        }

        /// <summary>
        /// Limits camera angle on the x-axis
        /// </summary>
        /// <returns>
        /// Angle corrected to be within min and max values.
        /// </returns>
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < 0f)
            {
                angle = 360 + angle;
            }
            if (angle > 180f)
            {
                return Mathf.Max(angle, 360 + min);
            }
            return Mathf.Min(angle, max);
        }

        /// <summary>
        /// Hides the cursor if game window is focused.
        /// </summary>
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
        {  
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
               itemInfos.itemName.text = inventory.inventorySlots[itemIndex].Item.itemName;
               itemInfos.itemWeight.text = inventory.inventorySlots[itemIndex].Item.itemWeight + " kg";
               itemInfos.itemDescription.text = inventory.inventorySlots[itemIndex].Item.itemLore;
               itemInfos.itemRarity.text = inventory.inventorySlots[itemIndex].Item.itemRarity.ToString();
               itemInfos.itemPrice.text = inventory.inventorySlots[itemIndex].Item.itemPrice + "G";
      
               if (inventory.inventorySlots[itemIndex].Item.itemSprite != null)
               {
                   itemInfos.ItemImage.sprite = inventory.inventorySlots[itemIndex].Item.itemSprite;
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
            bool isWalking = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }
            else if(!isCrouching)
            {
                isWalking = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hasJumped = true;
               // base.Jump(jumpSpeed);
            }

            //TODO: If toggle_crouch, toggle a switch instead of checking for sneak every update
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isCtrlPressed = true;
            }
            
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
            
            base.Move(InputVector, moveSpeed);

            
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
            
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                base.Grounding(true);
                Debug.Log("true");
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                base.Grounding(false);
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
                           if (targetItem.itemType == ItemType.Weapon || targetItem.itemType == ItemType.Ingredient || targetItem.itemType == ItemType.CraftingMaterial|| targetItem.itemType == ItemType.ConsumAble)
                           {
                               Item clonedItem = targetItem.DeepCopy();
      
                               // Returns 0 if item was added to inventory
                               int UnsuccessfulPickup = inventory.Add(clonedItem, 1, true);
      
                               if (UnsuccessfulPickup != 0)
                               {
                                   Debug.Log("Returned " + UnsuccessfulPickup + ", failed to collect item.");
                               }
                               else
                               {
                                   targetItem.DestroyInteractivObject();
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
            CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();

            if (!isCtrlPressed && wasCrouching != isCtrlPressed)
            {
                if (playerCollider.height <= playerHeight)
                {
                    playerCollider.height += crouchDelta * 0.1f;
                    playerCollider.center -= new Vector3(0, crouchDelta * 0.05f, 0);
                }
                else
                {
                    isCrouching = false;
                    wasCrouching = isCrouching;
                }
            }

            if (!wasCrouching && isCrouching)
            {
                playerCollider.height -= crouchDelta;
                playerCollider.center += new Vector3(0, crouchDelta / 2, 0);
                wasCrouching = isCrouching;
            }
            //TODO: other sneak effects
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
       
        
     