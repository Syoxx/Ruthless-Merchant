using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Player : Character
    {
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
        private float jumpSpeed = 10;

        [SerializeField]
        private GameObject ItemsParent;

        [SerializeField]
        private GameObject ItemUIPrefab;
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


            itemsContainer = ItemsParent.transform.parent.gameObject;
            uiCanvas = itemsContainer.transform.parent.gameObject;
            
            // Ensure hidden inventory
            if (uiCanvas.activeInHierarchy == true)
            {
                ShowInventory(false);
            }

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

        public override void Update()
        {
            LookRotation();
            HandleInput();
        }

        private void LookRotation()
        {
            // TODO: Set sensitivity values in menus
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

        public void ShowInventory(bool makeVisible)
        {
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
                itemInfos.itemWeight.text = "" + inventory.inventorySlots[itemIndex].Item.ItemWeight;
                // TODO: Item image, description, rarity, price
            }
        }

        public void ShowMap()
        {
            throw new System.NotImplementedException();
        }

        public void HandleInput()
        {
            bool isWalking = false;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SendInteraction();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                base.Jump(jumpSpeed);
            }

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

            moveSpeed = isWalking ? walkSpeed : runSpeed;
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            InputVector = new Vector2(horizontal, vertical);

            if (InputVector.sqrMagnitude > 1)
            {
                InputVector.Normalize();
            }

            MoveVector = new Vector3(InputVector.x, 0.0f, InputVector.y);
            base.Move(MoveVector, moveSpeed);
        }

        public void SendInteraction()
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
                        if (targetItem.Type == ItemType.Weapon || targetItem.Type == ItemType.Gear)
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

        public override void Interact(GameObject caller)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            // GD can alter player speed in inspector
            throw new System.NotImplementedException();
        }

        public void Walk()
        {
            throw new System.NotImplementedException();
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