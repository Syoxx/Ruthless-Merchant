//---------------------------------------------------------------
// Authors: Daniil Masliy, Richard Brönnimann, Peter Ehmler
//---------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Player : Character
    {
        public static Player Singleton;
        public bool RestrictBookUsage = false;
        private static bool restrictCamera = false;

        #region Private Fields
        private bool[] unlockedTravelPoints;
        private UISystem uiSystem;
        private QuestManager questManager;
        private bool restrictMovement = false;
        private bool isOutpostDialogActive;
        private bool hasJumped;
        private bool isCrouching;
        private bool isCtrlPressed;
        private bool wasCrouching;
        private bool isGameFocused;
        private int outpostToUpgrade = 0;
        [SerializeField, Tooltip("Max. interaction and glow range"), Range(0.0f, 5.0f)]
        private float maxInteractDistance;
        private float moveSpeed;
        private float mouseXSensitivity = 2f;
        private float mouseYSensitivity = 2f;

        enum ControlMode
        {
            Move = 0, AlchemySlot = 1
        }

        private Camera playerAttachedCamera;
        public NavMeshObstacle NavMeshObstacle;
        public Quaternion PlayerLookAngle;
        private Quaternion cameraPitchAngle;
        private Vector3 MoveVector = Vector3.zero;
        private Vector2 InputVector = Vector2.zero;
        private GameObject inventoryCanvas;
        private GameObject itemsContainer;
        private ControlMode controlMode = ControlMode.Move;
        private Reputation reputation;
        private MapSystem mapLogic;
        int currenRecipe;
        private Recipes recipes;
        GameObject smithCanvas;
        Smith localSmith;

        AlchemySlot localAlchemist;
        GameObject alchemyCanvas;
        respawnLogic respawn;

        Canvas workbenchCanvas;
        Workbench localWorkbench;
        Item breakableItem;
        int itemSlot;

        private float crouchDelta;
        private float playerHeight;

        [SerializeField, Tooltip("Tip: If this value matches the rigidbody's height, crouching doesn't affect player height")]
        [Range(0, 5)]
        private float CrouchHeight;

        [Space(10)]

        [Header("Texture")]
        [SerializeField, Tooltip("2D Texture for aimpoint.")]
        private Texture2D aimpointTexture;

        [Header("UI Prefabs")]
        [SerializeField, Tooltip("This is the UI Prefab that appears for each Item when accessing an Alchemyslot")]
        GameObject alchemyUiPrefab;

        [SerializeField, Tooltip("The UI Prefab that appears for each recipe when accessing the Smith")]
        GameObject recipeUiPrefab;

        [SerializeField, Tooltip("'TempUpgradeDialogue' in /Prefabs/TradingPoint/")]
        private GameObject outpostUpgradeDialogue;

        [SerializeField, Tooltip("'FailedUpgradeMessage' in /Prefabs/TradingPoint/")]
        private GameObject failedUpgradeDialogue;

        [Space(15)]

        [SerializeField, Tooltip("Drag Map_Canvas object here.")]
        private GameObject mapObject;
        

        [Space(10)]

        [Header("Book")]
        [SerializeField, Tooltip("Drag a book canvas there / Daniil Masliy")]
        public GameObject bookCanvas;

        [SerializeField, Tooltip("Drag 'InventoryItem' Prefab here.")]
        private GameObject itemInventory;

        [SerializeField, Tooltip("The Booklogic attached to the Book-Object")]
        private PageLogic bookLogic;
        private KeyCode currentBookSection;
        #endregion

        #region Public Fields
        [HideInInspector]
        public static KeyCode lastKeyPressed;
        [Space(8)]
        [SerializeField, Tooltip("Set the maximum amount of items per page.")]
        [Range(0, 8)]
        public int MaxItemsPerPage = 4;
        #endregion
        #region MonoBehaviour Life Cycle

        private void Awake()
        {
            Singleton = this;

            NavMeshObstacle = GetComponent<NavMeshObstacle>();

            if (NavMeshObstacle != null)
                NavMeshObstacle.enabled = false;
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

        public Reputation Reputation
        {
            get
            {
                if (reputation == null)
                {
                    reputation = GetComponent<Reputation>();
                }
                return reputation;
            }
        }
        
        public bool HasJumped
        {
            get { return hasJumped; }
            set { hasJumped = value; }
        }

        public override void Start()
        {
            base.Start();
            CheckForMissingObjects();
            unlockedTravelPoints = new bool[17];
            smithCanvas = GameObject.Find("SmithCanvas");
            alchemyCanvas = GameObject.Find("AlchemyCanvas");
            reputation = GetComponent<Reputation>();
            respawn = GetComponent<respawnLogic>();
            if (smithCanvas)
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

            if (!inventory)
            {
                inventory = FindObjectOfType<Inventory>();
            }

            if (itemsContainer != null)
            {
                inventoryCanvas = itemsContainer.transform.parent.gameObject;
            }

            playerHeight = GetComponent<CapsuleCollider>().height;
            crouchDelta = playerHeight - CrouchHeight;

            //BookLogic instantiate
            if(!bookLogic)
            {
                bookLogic = GameObject.Find("Book").GetComponent<PageLogic>();
            }
            bookLogic.GeneratePages();
            inventory.BookLogic = bookLogic;

            mapLogic = mapObject.GetComponent<MapSystem>();
            mapLogic.Start();

            inventory.ItemUIPrefab = itemInventory;

            PlayerLookAngle = transform.localRotation;

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

            Physics.IgnoreLayerCollision(9, 13);

            OpenBook(KeyCode.N);
            //inventory.InventoryChanged.AddListener(PopulateInventoryPanel);
        }

        /// <summary>
        /// Use this method to check if your Prefab/GameObject etc. is correctly configured in Inspector 
        /// </summary>
        private void CheckForMissingObjects()
        {
            if (itemInventory == null)
            {
                throw new Exception("Item Inventory Prefab is missing in Player Inspector. This prefab if placed in (../Prefabs/Book/Item Inventory)");
            }

            if (bookCanvas == null)
            {
                throw new Exception("BookCanvas is missing in Player Inspector - just drag book canvas from the Scene");
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

        public override void Update()
        {     
            LookRotation();
            ControleModeMove();
            if (controlMode == ControlMode.Move)
                FocusCursor();
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            GlowObject();
            base.Update();
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

                PlayerLookAngle *= Quaternion.Euler(0f, yRot, 0f);

                transform.localRotation = PlayerLookAngle;

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
            if ((!restrictCamera) && restrictMovement && TradeAbstract.Singleton == null)
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
        

        #region Book and map

        public void ShowMap()
        {
            if (Input.GetKeyDown(KeyCode.M) && !isOutpostDialogActive)
            {
                bool isUI_Inactive = (mapObject.activeSelf == false);

                if (isUI_Inactive)
                    gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", true);
                else
                    gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", false);
                if (bookCanvas.activeSelf)
                {
                    CloseBook();
                }

                //Sound
                if (mapObject.activeSelf == false)
                {
                    //Sound - Open Book
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Book/Open Book", GameObject.FindGameObjectWithTag("Player").transform.position);
                }
                else
                {
                    //Sound - Close Book
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Book/CloseBook", GameObject.FindGameObjectWithTag("Player").transform.position);
                }

                //TODO: check which posts player has unlocked / bought
                // pass an array with ids of trading posts that should be displayed
                mapLogic.RefreshMapCanvas(unlockedTravelPoints);

                mapObject.SetActive(isUI_Inactive);
                restrictMovement = isUI_Inactive || TradeAbstract.Singleton != null;
                restrictCamera = isUI_Inactive;
            }
        }

        /// <summary>
        /// A simple function to open a book
        /// </summary>
        private void BookControls()
        {
            if (!RestrictBookUsage)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    OpenBook(KeyCode.J);
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    OpenBook(KeyCode.N);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OpenBook(KeyCode.Escape);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    OpenBook(KeyCode.I);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    OpenBook(KeyCode.R);
                }
            }
        }

        private void OpenBook(KeyCode key)
        {
            if (mapObject.activeSelf)
            {
                mapObject.SetActive(false);
                gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", false);
            }

            if (currentBookSection == key || (bookCanvas.activeSelf && key == KeyCode.Escape))
            {
                CloseBook();

                //Sound - Close Book
                FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Book/CloseBook", GameObject.FindGameObjectWithTag("Player").transform.position);
            }
            else if (!isOutpostDialogActive)
            {
                gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", true);

                //Sound - Open Book
                FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Book/Open Book", GameObject.FindGameObjectWithTag("Player").transform.position);

                bookCanvas.SetActive(true);
                restrictMovement = bookCanvas.activeSelf;
                restrictCamera = bookCanvas.activeSelf;
                currentBookSection = key;
                bookLogic.GoToPage(key);
            }
        }

        private void CloseBook()
        {
            if (mapObject.activeSelf)
            {
                mapObject.SetActive(false);
                gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", false);
            }
            gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", false);
            currentBookSection = KeyCode.None;
            bookCanvas.SetActive(bookCanvas.activeSelf == false);
            //lastKeyPressed = KeyCode.Escape;
            restrictMovement = bookCanvas.activeSelf || TradeAbstract.Singleton != null;
            restrictCamera = bookCanvas.activeSelf;
            if (!bookCanvas.activeSelf && recipes != null)
            {
                for (int i = 0; i < recipes.Panels.Count; i++)
                {
                    recipes.Panels[i].Button.onClick.RemoveAllListeners();
                }
                for (int i = 0; i < inventory.InventorySlots.Length; i++)
                {
                    if (inventory.inventorySlots[i].DisplayData)
                        inventory.inventorySlots[i].DisplayData.ItemButton.onClick.RemoveAllListeners();
                }
            }
        }
        #endregion


        #region Interaction and control modes

        private void ControleModeMove()
        {
            if (InputVector != new Vector2(0, 0))
                gameObject.GetComponentInChildren<Animator>().SetBool("IsWalking", true);
            else
                gameObject.GetComponentInChildren<Animator>().SetBool("IsWalking", false);
            bool isWalking = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = true;
            }
            else
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

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!restrictMovement && !restrictCamera)
                {
                    isCtrlPressed = true;
                }
            }

            if (!isCrouching)
            {
                moveSpeed = isWalking ? walkSpeed : runSpeed;
            }
            else
            {
                moveSpeed = sneakSpeed;
            }

            float horizontal = 0f;
            float vertical = 0f;

            if (!restrictMovement && !restrictCamera)
            {
                if (gameObject.GetComponent<Rigidbody>().freezeRotation == true || gameObject.GetComponent<Rigidbody>().useGravity == false)
                {
                    gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                }

                if (Input.GetKey(KeyCode.W))
                    vertical = 1;
                else if (Input.GetKey(KeyCode.S))
                    vertical = -1;
                if (Input.GetKey(KeyCode.D))
                    horizontal = 1;
                else if (Input.GetKey(KeyCode.A))
                    horizontal = -1;
                //horizontal = Input.GetAxis("Horizontal");
                //vertical = Input.GetAxis("Vertical");
            }
            else if (IsGrounded)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            }
            //else{}

            InputVector = new Vector2(horizontal, vertical);

            if (InputVector.sqrMagnitude > 1)
            {
                InputVector.Normalize();
            }

            base.Move(InputVector, moveSpeed);


            SendInteraction();
            ShowMap();

            if (isOutpostDialogActive)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && TradeAbstract.Singleton == null)
                {
                    isOutpostDialogActive = false;
                    restrictCamera = false;
                    restrictMovement = false;
                    if (outpostUpgradeDialogue.activeSelf)
                    {
                        outpostUpgradeDialogue.SetActive(false);
                    }
                    else
                    {
                        failedUpgradeDialogue.SetActive(false);
                    }
                    controlMode = ControlMode.Move;
                }
            }
            else
            {
                BookControls();
            }

            if (Input.GetKey(KeyCode.F6))
            {
                Debug.Log("Gold: " + inventory.PlayerMoney);
            }
        }

        public void SendInteraction()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerAttachedCamera != null)
                {
                    Ray clickRay = playerAttachedCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                    RaycastHit hit;
                    LayerMask mask = ~(1 << 16);

                    if (Physics.Raycast(clickRay, out hit, maxInteractDistance, mask));
                    {
                        Debug.Log(hit.collider.name + " " + hit.point + " clicked.");

                        InteractiveObject target = hit.collider.gameObject.GetComponent<InteractiveObject>();

                        // Treat interaction target like an item                    
                        Item targetItem = target as Item;

                        if (targetItem != null)
                        {
                            // Picking up items and gear
                            if (targetItem.ItemInfo.ItemType == ItemType.Weapon || targetItem.ItemInfo.ItemType == ItemType.Ingredient || targetItem.ItemInfo.ItemType == ItemType.CraftingMaterial || targetItem.ItemInfo.ItemType == ItemType.ConsumAble || targetItem.ItemInfo.ItemType == ItemType.Other)
                            {
                                Item clonedItem = targetItem.DeepCopy();

                                //Sound - Item pickup
                                if (targetItem.ItemInfo.ItemType == ItemType.Weapon)
                                {
                                    FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Player/Sheathing sword", this.transform.position);
                                }
                                else
                                {
                                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ItemPickup", this.transform.position);
                                }

                                // Returns 0 if item was added to inventory
                                int UnsuccessfulPickup = inventory.Add(clonedItem, 1, true);

                                if (UnsuccessfulPickup != 0)
                                {
                                    Debug.Log("Returned " + UnsuccessfulPickup + ", failed to collect item.");
                                }
                                else
                                {
                                    targetItem.DestroyInteractiveObject();
                                    //PopulateInventoryPanel();
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
                                //PopulateInventoryPanel();
                            }
                            else if (target != null)
                            {
                                target.Interact(this.gameObject);
                            }
                        }
                    }
                }
            }
        }

        public void EnterSmith(Smith smith)
        {
            localSmith = smith;
            for (int i = 0; i < recipes.Panels.Count; i++)
            {
                int num = i;
                    recipes.Panels[num].Button.onClick.RemoveAllListeners();
                recipes.Panels[num].Button.onClick.AddListener(delegate { localSmith.TryCraft(inventory, recipes.Panels[num].Recipe, recipes); });
            }
            {
                bookCanvas.SetActive(bookCanvas.activeSelf == false);
                lastKeyPressed = KeyCode.R;
                restrictMovement = !(bookCanvas.activeSelf == false);
                restrictCamera = !(bookCanvas.activeSelf == false);
                bookLogic.GoToPage(KeyCode.R);
            }
        }

        public void EnterAlchemySlot(AlchemySlot alchemySlot)
        {
            localAlchemist = alchemySlot;
            lastKeyPressed = KeyCode.I;
            if (localAlchemist.Ingredient == null)
            {
                gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", true);
                BookControls();
                bookCanvas.SetActive(true);
                restrictMovement = !(bookCanvas.activeSelf == false);
                restrictCamera = !(bookCanvas.activeSelf == false);
                bookLogic.GoToPage(KeyCode.I);

                SetAlchemyItemButtons();
            }
            else
            {
                localAlchemist.RemoveItem(inventory);
            }
        }

        void SetAlchemyItemButtons()
        {
            for (int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                if (inventory.inventorySlots[i].DisplayData)
                {
                    if (inventory.inventorySlots[i].ItemInfo.ItemType == ItemType.Ingredient)
                    {
                        int value = i;
                        inventory.inventorySlots[i].DisplayData.ItemButton.onClick.AddListener(delegate { OnAlchemyButton(value); });
                    }
                }
            }
        }

        public void EnterWorkbench(Workbench workbench)
        {
            PopulateWorkbenchPanel();
            if (mapObject.activeSelf)
            {
                gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", false);
                mapObject.SetActive(false);
            }

            gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", true);
            bookCanvas.SetActive(true);
            lastKeyPressed = KeyCode.I;
            restrictMovement = true;
            restrictCamera = true;
            localWorkbench = workbench;
            bookLogic.GoToPage(KeyCode.I);
        }

        public void EnterTrading()
        {
            restrictMovement = true;
            restrictCamera = true;
            bookCanvas.SetActive(true);
            gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", true);

            if (Trader.CurrentTrader.startTradeImmediately)
                bookLogic.GoToPage(KeyCode.N);
            else
                bookLogic.GoToPage(KeyCode.I);
        }

        public void AllowTradingMovement()
        {
            restrictCamera = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bookCanvas.SetActive(false);
            gameObject.GetComponentInChildren<Animator>().SetBool("IsReading", false);
        }

        void CreateAlchemyCanvas()
        {
            foreach (Transform item in alchemyCanvas.transform)
            {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                if (inventory.inventorySlots[i].Item)
                    if (inventory.inventorySlots[i].ItemInfo.ItemType == ItemType.Ingredient)
                    {
                        Button newPanel = Instantiate(alchemyUiPrefab, alchemyCanvas.transform).GetComponent<Button>();

                        int panel = i;
                        newPanel.onClick.AddListener(delegate { OnAlchemyButton(panel); });

                        newPanel.GetComponentInChildren<Text>().text = inventory.inventorySlots[i].ItemInfo.ItemName;
                    }
            }
        }

        public void OutpostInteraction(int OutpostIndex)
        {
            outpostToUpgrade = OutpostIndex;

            // TODO: option to unlock fast travel point
            // requires: outpost receives interaction
            // if this outpost has no trade point yet:
            // show confirmation dialogue, restrict movement until OnClick or Esc pressed
            if (unlockedTravelPoints[outpostToUpgrade] == false)
            {
                isOutpostDialogActive = true;
                outpostUpgradeDialogue.SetActive(true);
                restrictCamera = true;
                restrictMovement = true;
            }
            else
            {
                // TODO: Allow item storage in trading point
                Debug.Log("Interacted with trading point");
            }
        }

        public override void Interact(GameObject caller)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region OnClick Handlers

        public void OnAlchemyButton(int itemSlot)
        {
            if(Inventory.inventorySlots[itemSlot].ItemInfo.ItemType == ItemType.Ingredient)
            {
                localAlchemist.AddItem((Ingredient)Inventory.inventorySlots[itemSlot].Item);
                Inventory.Remove(itemSlot, 1, true);
                CloseBook();
            }
        }


        public void OnWorkbenchButton(int itemslot)
        {
            localWorkbench.BreakdownItem(inventory.inventorySlots[itemslot].Item, Inventory, recipes);
            PopulateWorkbenchPanel();
        }

        private void PopulateWorkbenchPanel()
        {
            for (int itemIndex = 0; itemIndex < inventory.inventorySlots.Length; itemIndex++)
            {
                if (inventory.inventorySlots[itemIndex].Item == null)
                {
                    continue;
                }
                else if (inventory.inventorySlots[itemIndex].ItemInfo.ItemType == ItemType.Weapon)
                {
                    int item = itemIndex;
                    inventory.inventorySlots[itemIndex].DisplayData.ItemButton.onClick.RemoveAllListeners();
                    inventory.inventorySlots[itemIndex].DisplayData.ItemButton.onClick.AddListener(delegate{ OnWorkbenchButton(item); });
                }
            }
        }

        public void BuyTradingPoint()
        {
            if (inventory.RemoveGold(50))
            {
                unlockedTravelPoints[outpostToUpgrade] = true;      // update array of unlocked travel points

                Ray cameraRay = playerAttachedCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                if (Physics.Raycast(cameraRay, out hit, maxInteractDistance))
                {
                    TradepointUnlocker tradepoint = hit.collider.gameObject.GetComponent<TradepointUnlocker>();

                    if (tradepoint != null)
                    {
                        tradepoint.SetActiveTradepoint();
                    }
                }

                CloseTradingPointDialog();
            }
            else
            {
                CloseTradingPointDialog();
                isOutpostDialogActive = true;
                restrictCamera = true;
                restrictMovement = true;
                failedUpgradeDialogue.SetActive(true);
            }
            
        }

        private Outline lastOutline;
        public void GlowObject()
        {
            Ray cameraRay = playerAttachedCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            LayerMask mask = ~(1 << 16);
            if (Physics.Raycast(cameraRay, out hit, maxInteractDistance, mask))
            {
                Outline outline = hit.collider.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    ReplaceOutline(outline);
                }
                else
                {
                    ReplaceOutline(null);
                }
            }
            else
            {
                ReplaceOutline(null);
            }
        }

        private void ReplaceOutline(Outline outline)
        {
            if (outline != lastOutline)
            {
                if (lastOutline != null)
                    lastOutline.OutlineMode = Outline.Mode.None;

                if (outline != null)
                    outline.OutlineMode = Outline.Mode.OutlineVisible;

                lastOutline = outline;
            }
        }

        public void CloseTradingPointDialog()
        {
            if (outpostUpgradeDialogue.activeSelf && TradeAbstract.Singleton == null)
            {
                isOutpostDialogActive = false;
                restrictCamera = false;
                restrictMovement = false;
                outpostUpgradeDialogue.SetActive(false);
                controlMode = ControlMode.Move;
            }
        }

        public void CloseTradingPointMessage()
        {
            if (failedUpgradeDialogue.activeSelf && TradeAbstract.Singleton == null)
            {
                isOutpostDialogActive = false;
                restrictCamera = false;
                restrictMovement = false;
                failedUpgradeDialogue.SetActive(false);
                controlMode = ControlMode.Move;
            }
        }
        #endregion

        public override void DestroyInteractiveObject(float delay = 0)
        {
            respawn.InitiateRespawn();
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


