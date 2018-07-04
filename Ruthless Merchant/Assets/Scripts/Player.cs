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
        private static bool restrictCamera = false;

        #region Private Fields
        private UISystem uiSystem;
        private QuestManager questManager;
        private bool restrictMovement = false;
        private bool hasJumped;
        private bool isCrouching;
        private bool isCtrlPressed;
        private bool wasCrouching;
        private bool isGameFocused;
        private int maxInteractDistance;
        private float moveSpeed;
        private float mouseXSensitivity = 2f;
        private float mouseYSensitivity = 2f;

        enum ControlMode
        {
            Move = 0, Smith = 1, Workbench = 2, AlchemySlot = 3
        }
        
        private Camera playerAttachedCamera;
        private Quaternion playerLookAngle;
        private Quaternion cameraPitchAngle;
        private Vector3 MoveVector = Vector3.zero;
        private Vector2 InputVector = Vector2.zero;
        private GameObject inventoryCanvas;
        private GameObject itemsContainer;
        private ControlMode controlMode = ControlMode.Move;

        int currenRecipe;
        GameObject smithCanvas;
        Smith localSmith;

        AlchemySlot localAlchemist;
        GameObject alchemyCanvas;

        Canvas workbenchCanvas;
        Workbench localWorkbench;
        Item breakableItem;
        int itemSlot;

        private float crouchDelta;
        private float playerHeight;


        

        [SerializeField]
        private Texture2D aimpointTexture;

        [SerializeField]
        [Tooltip("Tip: If this value matches the rigidbody's height, crouching doesn't affect player height")]
        private float CrouchHeight;

        [SerializeField]
        private GameObject ItemsParent;

        [SerializeField]
        private GameObject ItemUIPrefab;

        [SerializeField, Tooltip("This is the UI Prefab that appears for each Item when accessing an Alchemyslot")]
        GameObject alchemyUiPrefab;

        [SerializeField]
        private GameObject workshopUIPrefab;

        [SerializeField, Tooltip("The UI Prefab that appears for each recipe when accessing the Smith")]
        GameObject recipeUiPrefab;

        [SerializeField]
        private GameObject mapObject;

        [SerializeField, Tooltip("This is the Recipe Component placed on this Object")]
        private Recipes recipes;

        [Header("Book")]
        [SerializeField] [Tooltip("Drag a book canvas there / Daniil Masliy")]
        private GameObject _bookCanvas;

        [SerializeField] private int MaxItemsPerPage;
        private Transform lastGroundedPosition;

        #endregion

        #region MonoBehaviour Life Cycle

        private void Awake()
        {
            Singleton = this;
        }

        #endregion


        public static bool RestrictCamera
        {
            get { return restrictCamera; }
            set { restrictCamera = value; }
        }

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

            smithCanvas = GameObject.Find("SmithCanvas");
            alchemyCanvas = GameObject.Find("AlchemyCanvas");
            if(smithCanvas)
            {
                smithCanvas.SetActive(false);
            }
            if (alchemyCanvas)
            {
                alchemyCanvas.SetActive(false);
            }

            if (!recipes)
            {
                recipes = FindObjectOfType<Recipes>();
            }

            if (ItemsParent != null)
            {
                itemsContainer = ItemsParent.transform.parent.gameObject;
            }

            if (itemsContainer != null)
            {
                inventoryCanvas = itemsContainer.transform.parent.gameObject;
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
                isGameFocused = true;
            }
            else
            {
                Debug.Log("Player object does not have a first person camera.");
                isGameFocused = false;
            }

            inventory.InventoryChanged.AddListener(PopulateInventoryPanel);
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
                if (Input.GetKeyUp(KeyCode.LeftControl) || restrictMovement == true)
                {
                    isCtrlPressed = false;

                    if (crouchDelta == 0)
                    {
                        isCrouching = false;
                    }
                }
            }

            Crouch();

            base.FixedUpdate();
        }

        public override void Update()
        {
            LookRotation();
            HandleInput();
            if(controlMode == ControlMode.Move)
                FocusCursor();
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        /// <summary>
        /// Rotates view using mouse movement.
        /// </summary>
        private void LookRotation()
        {
            if (!restrictCamera)
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
            }

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
            // Pressing escape in a menu switches back to game (no cursor)
            if (Input.GetKeyUp(KeyCode.Escape) && restrictCamera)
            {
                isGameFocused = true;
                restrictCamera = false;
            }
            else if ((!restrictCamera) && restrictMovement)
            {
                // This prevents movement being disabled while restrictCamera is not on
                restrictMovement = false;
            }

            if (!restrictCamera && isGameFocused)
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                // Currently using None instead of Limited
                if (Cursor.lockState != CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        public void ShowInventory()
        {  
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    bool isUI_Inactive = (inventoryCanvas.activeSelf == false);
            //    if (mapObject.activeSelf)
            //    {
            //        mapObject.SetActive(false);
            //    }

            //    inventoryCanvas.SetActive(isUI_Inactive);
            //    restrictMovement = isUI_Inactive;
            //}
        }
        private void PopulateWorkbenchPanel()
        {
            /*
            if (inventory.inventorySlots.Length == 0)
            {
                return;
            }
            else
            {
                foreach (Transform child in ItemsParent.transform)
                {
                    Destroy(child.gameObject);
                }

            }
            for (int itemIndex = 0; itemIndex < inventory.inventorySlots.Length; itemIndex++)
            {
                if (inventory.inventorySlots[itemIndex].Item == null)
                {
                    continue;
                }
                else if (inventory.inventorySlots[itemIndex].Item.itemType == ItemType.Weapon)
                {                  
                    GameObject InventoryItem = Instantiate(ItemUIPrefab) as GameObject;
                    InventoryItem.transform.SetParent(ItemsParent.transform, false);
                    InventoryDisplayedData itemInfos = InventoryItem.GetComponent<InventoryDisplayedData>();
                    itemInfos.itemName.text = inventory.inventorySlots[itemIndex].Item.itemName + " x" + inventory.inventorySlots[itemIndex].Count;
                    itemInfos.itemWeight.text = inventory.inventorySlots[itemIndex].Item.itemWeight + " kg";
                    itemInfos.itemDescription.text = inventory.inventorySlots[itemIndex].Item.itemLore;
                    itemInfos.itemRarity.text = inventory.inventorySlots[itemIndex].Item.itemRarity.ToString();
                    itemInfos.itemPrice.text = inventory.inventorySlots[itemIndex].Item.itemPrice + "G";

                    if (inventory.inventorySlots[itemIndex].Item.itemSprite != null)
                    {
                        itemInfos.ItemImage.sprite = inventory.inventorySlots[itemIndex].Item.itemSprite;
                    }
                    GameObject workshopButton = Instantiate(workshopUIPrefab) as GameObject;
                    workshopButton.transform.SetParent(InventoryItem.transform, false);
                    if (workshopButton.GetComponent<Button>() != null)
                    {
                        itemSlot = itemIndex;
                        workshopButton.GetComponent<Button>().onClick.AddListener(() => OnWorkbenchButton(itemSlot));
                    }
                    
                    //Set Button-Width and Height
                    workshopButton.GetComponent<RectTransform>().offsetMin = new Vector2(-400, -65);
                    workshopButton.GetComponent<RectTransform>().offsetMax = new Vector2(400, 65);


                    //workshopButton.transform.position = inventory.inventorySlots[itemIndex].Item.transform.position;
                    //InventoryItem.GetComponent<RectTransform>().rect.x, InventoryItem.GetComponent<RectTransform>().rect.y, InventoryItem.GetComponent<RectTransform>().rect.width, InventoryItem.GetComponent<RectTransform>().rect.height
                    workshopButton.SetActive(true);
                }
                else continue;
            }
            */
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
      
               GameObject inventoryItem = Instantiate(ItemUIPrefab) as GameObject;
               inventoryItem.transform.SetParent(ItemsParent.transform, false);
               InventoryDisplayedData itemInfos = inventoryItem.GetComponent<InventoryDisplayedData>();
               itemInfos.itemName.text = inventory.inventorySlots[itemIndex].Count + "x " + inventory.inventorySlots[itemIndex].Item.itemName + " (" + inventory.inventorySlots[itemIndex].Item.itemRarity + ")";
               //itemInfos.itemWeight.text = inventory.inventorySlots[itemIndex].Item.itemWeight + " kg";
               itemInfos.itemDescription.text = inventory.inventorySlots[itemIndex].Item.itemLore;
               //itemInfos.itemRarity.text = inventory.inventorySlots[itemIndex].Item.itemRarity.ToString();
               itemInfos.itemPrice.text = inventory.inventorySlots[itemIndex].Item.itemPrice + "G";
      
               if (inventory.inventorySlots[itemIndex].Item.itemSprite != null)
               {
                   itemInfos.ItemImage.sprite = inventory.inventorySlots[itemIndex].Item.itemSprite;
               }
           }
       }

        private void UpdateCanvas(int currentRecipe)
        {
            Transform canv = smithCanvas.transform.GetChild(0);
            foreach (Transform child in canv.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < recipes.GetRecipes()[currenRecipe].ListOfMaterials.Count; i++)
            {
                GameObject newPanel = Instantiate(recipeUiPrefab, canv);
                newPanel.GetComponentInChildren<Text>().text = recipes.GetRecipes()[currenRecipe].ListOfMaterials[i].Item.itemName + "\n" + recipes.GetRecipes()[currenRecipe].ListOfMaterials[i].Count;
            }
        }

        public void ShowMap()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                bool isUI_Inactive = (mapObject.activeSelf == false);

                if (_bookCanvas.activeSelf)
                {
                    _bookCanvas.SetActive(false);
                }

                mapObject.SetActive(isUI_Inactive);
                restrictMovement = isUI_Inactive;
                restrictCamera = isUI_Inactive;
            }
        }

        /// <summary>
        /// Checks for input to control the player character.
        /// </summary>
        public void HandleInput()
        {
            switch(controlMode)
            {
                case ControlMode.Move:
                    ControleModeMove();
                    break;
                case ControlMode.Smith:
                    ControlModeSmith();
                    break;
                case ControlMode.Workbench:
                    ControlModeWorkbench();
                    break;
                case ControlMode.AlchemySlot:
                    ControlModeAlchemist();
                    break;
            }
        }

        private void ControleModeMove()
        {
            bool isWalking = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }
            else if (!isCrouching)
            {
                isWalking = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!restrictMovement && !restrictCamera)
                {
                    hasJumped = true;
                }
            }

            //TODO: If toggle_crouch, toggle a switch instead of checking for sneak every update
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!restrictMovement && !restrictCamera)
                {
                    isCtrlPressed = true;
                }
            }


            moveSpeed = isWalking ? walkSpeed : runSpeed;

            float horizontal = 0f;
            float vertical = 0f;

            if (!restrictMovement && !restrictCamera)
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            
            if (horizontal != 0 || vertical != 0)
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
            OpenBook();
        }

        private void ControlModeSmith()
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                controlMode = ControlMode.Move;
                restrictMovement = false;
                smithCanvas.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {

                currenRecipe++;
                if (currenRecipe >= recipes.GetRecipes().Count)
                {
                    currenRecipe = 0;
                }

                while(!recipes.GetRecipes()[currenRecipe].Unlocked)
                {
                    currenRecipe++;
                    if (currenRecipe >= recipes.GetRecipes().Count)
                    {
                        currenRecipe = 0;
                    }
                }

                UpdateCanvas(currenRecipe);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                currenRecipe--;
                if (currenRecipe < 0)
                {
                    currenRecipe = recipes.GetRecipes().Count - 1;
                }

                while (!recipes.GetRecipes()[currenRecipe].Unlocked)
                {
                    currenRecipe--;
                    if (currenRecipe < 0)
                    {
                        currenRecipe = recipes.GetRecipes().Count - 1;
                    }
                }

                UpdateCanvas(currenRecipe);
                
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                localSmith.TryCraft(inventory, currenRecipe);
                PopulateInventoryPanel();
            }
        }

        void ControlModeAlchemist()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                alchemyCanvas.SetActive(false);
                controlMode = ControlMode.Move;
            }
        }

        public void OnAlchemyButton(int itemSlot)
        {
            localAlchemist.AddItem((Ingredient)Inventory.inventorySlots[itemSlot].Item);
            Inventory.Remove(itemSlot, 1, true);
            alchemyCanvas.SetActive(false);
            controlMode = ControlMode.Move;
        }


        private void ControlModeInventoryBook()
        {

        }
        public void OnWorkbenchButton(int itemslot)
        {
            localWorkbench.BreakdownItem(inventory.inventorySlots[itemSlot].Item, Inventory);
            PopulateWorkbenchPanel();
        }

        private void ControlModeWorkbench()
        {
            //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    PopulateInventoryPanel();
            //    restrictMovement = false;
            //    inventoryCanvas.SetActive(false);
            //    controlMode = ControlMode.Move;
            //}
            //if(Input.GetKeyDown(KeyCode.E))
            //{
            //    localWorkbench.BreakdownItem(inventory.inventorySlots[0].Item, Inventory);
            //    PopulateWorkbenchPanel();

            //}

            /* REMOVE ONCE WORKBENCH IS FIXED:  */
            restrictMovement = false;
            restrictCamera = false;
            controlMode = ControlMode.Move;
        }
        private void OnCollisionStay(Collision collision)
        {
            
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                base.Grounding(true);
                lastGroundedPosition = gameObject.transform;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                base.Grounding(false);
            }
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
                                   PopulateInventoryPanel();
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
                                PopulateInventoryPanel();
                           }
                           else if(target !=null )
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

            //if (crouchDelta != 0)
            //{
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

        /// <summary>
        /// A simple function to open a book
        /// </summary>
        private void OpenBook()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (mapObject.activeSelf == true)
                {
                    mapObject.SetActive(false);
                }

                _bookCanvas.SetActive(_bookCanvas.activeSelf == false);
                restrictMovement = !(_bookCanvas.activeSelf == false);
                restrictCamera = !(_bookCanvas.activeSelf == false);
            }
        }

        public void EnterSmith(Smith smith)
        {
            localSmith = smith;
            controlMode = ControlMode.Smith;
            currenRecipe = 0;

            smithCanvas.SetActive(true);
            restrictMovement = true;
            restrictCamera = true;
            UpdateCanvas(currenRecipe);
        }

        public void EnterAlchemist(AlchemySlot alchemySlot)
        {
            bool hasIngridients = false;
            for(int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                if (inventory.inventorySlots[i].Item.GetType() == typeof(Ingredient))
                    hasIngridients = true;
            }
            localAlchemist = alchemySlot;
            controlMode = ControlMode.AlchemySlot;
        }

        public void EnterWorkbench(Workbench workbench)
        {
            //Might be excessiv Populating
            PopulateWorkbenchPanel();
            if (mapObject.activeSelf)
            {
                mapObject.SetActive(false);
            }

            inventoryCanvas.SetActive(true);
            restrictMovement = true;
            restrictCamera = true;
            localWorkbench = workbench;
            controlMode = ControlMode.Workbench;
        }

        public void EnterAlchemySlot(AlchemySlot alchemySlot)
        {
            localAlchemist = alchemySlot;
            controlMode = ControlMode.AlchemySlot;

            alchemyCanvas.SetActive(true);
            CreateAlchemyCanvas();
        }

        void CreateAlchemyCanvas()
        {
            foreach(Transform item in alchemyCanvas.transform)
            {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                if (inventory.inventorySlots[i].Item)
                    if (inventory.inventorySlots[i].Item.itemType == ItemType.Ingredient)
                    {
                        Button newPanel = Instantiate(alchemyUiPrefab, alchemyCanvas.transform).GetComponent<Button>();

                        int panel = i;
                        newPanel.onClick.AddListener(delegate { OnAlchemyButton(panel); });

                        newPanel.GetComponentInChildren<Text>().text = inventory.inventorySlots[i].Item.itemName;
                    }
            }
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

        public void MakeOffer(string playerOfferString)
        {
            Trade trade = Trade.Singleton;
            trade.AugmentTotalPlayerOffers();

            float playerOfferParsed = float.Parse(playerOfferString);
            float playerOffer = (float)Math.Floor(playerOfferParsed);

            int lastPlayerOffer = -1;

            if (trade.PlayerOffers.Count > 0)
            {
                lastPlayerOffer = (int)Math.Floor(trade.PlayerOffers[trade.PlayerOffers.Count - 1]);
            }

            if (playerOffer < 1 || lastPlayerOffer != -1 && lastPlayerOffer < 2)
            {
                playerOffer = 1;
            }

            else if (lastPlayerOffer != -1 && playerOffer >= lastPlayerOffer)
            {
                playerOffer = lastPlayerOffer - 1;
            }

            trade.PlayerOffers.Add(playerOffer);
            trade.BargainEventsText.text = "";

            trade.UpdateTrading();
        }

        public void OnOutOfBound()
        {
            gameObject.transform.position = lastGroundedPosition.position;
        }

    }
}
       
        
     