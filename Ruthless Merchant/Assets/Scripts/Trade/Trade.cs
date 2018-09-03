using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Trade : TradeAbstract
    {
        #region Public Fields

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public List<float> PlayerOffers;

        [SerializeField]
        Item coinPrefab;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public List<float> TraderOffers;

        public static event EventHandler<TradeArgs> ItemsSold;

        public class TradeArgs : EventArgs
        {
            public List<InventoryItem> Items;
            public Trader Trader;

            public TradeArgs(List<InventoryItem> items, Trader trader)
            {
                Items = items;
                Trader = trader;
            }
        }

        #endregion

        #region MonoBehaviour Life Cycle

        protected override void Awake()
        {
            base.Awake();
            PlayerOffers = new List<float>();
            TraderOffers = new List<float>();
            weightsPlayer = new List<List<GameObject>>();
            weightsTrader = new List<List<GameObject>>();
        }

        private void Start()
        {
            weightsPlayer = GetPresentWeights(PlayerZone);
            weightsTrader = GetPresentWeights(TraderZone);

            for (int x = 0; x < weightsPlayer.Count; x++)
            {
                for (int y = 0; y < weightsPlayer[x].Count; y++)
                {
                    weightsPlayer[x][y].SetActive(false);
                    weightsTrader[x][y].SetActive(false);
                }
            }

            TradeObjectsParent.transform.position = Trader.CurrentTrader.gameObject.transform.position;
            NeutralPositionY = PlayerZone.transform.position.y;

            neutralConnectorRotation = new Vector3(-90, -90, 90);

            Vector3 prevRotation = TradeObjectsParent.transform.rotation.eulerAngles;
            TradeObjectsParent.transform.LookAt(Player.Singleton.transform);

            Vector3 rotation = TradeObjectsParent.transform.rotation.eulerAngles;
            TradeObjectsParent.transform.Rotate(-rotation.x, 0, -rotation.z);

            UpdateUI();

            initialized = false;
        }

        void Update()
        {
            if (Exit)
            {
                exitTimer += Time.deltaTime;

                if (exitTimer > 3)
                {
                    Cursor.visible = false;
                    Singleton = null;
                    Player.Singleton.AllowTradingMovement();

                    if (Tutorial.Singleton != null)
                    {
                        Tutorial.Singleton.TradeIsDone = true;
                        Debug.Log("Trade is done");
                    }

                    Main_SceneManager.UnLoadScene("TradeScene");
                    Trader.CurrentTrader.Scale.SetActive(true);
                    Trader.CurrentTrader = null;
                }
            }
            else
            {
                if (initialized)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") != 0)
                    {
                        ModifyOffer();
                    }

                    else if (!Player.Singleton.bookCanvas.activeInHierarchy && Input.GetMouseButtonDown(0))
                    {
                        HandlePlayerOffer();
                    }

                    else if (Input.GetKeyDown(KeyCode.E) && TraderOffers.Count > 0)
                    {
                        Accept();
                    }
                }

                #if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Q) || Vector3.Distance(TradeObjectsParent.transform.position, Player.Singleton.transform.position) > 5)
                {
                    Quit();
                }
                #else
                if (Input.GetKeyDown(KeyCode.Escape) || Vector3.Distance(TradeObjectsParent.transform.position, Player.Singleton.transform.position) > 5)
                {
                    Quit();
                }
                #endif
            }
        }

        #endregion

        /// <summary>
        /// Initializes the trade.
        /// </summary>
        /// <param name="realValue">The RealValue of the trade.</param>
        public override void Initialize(int realValue)
        {
            SetItemValue(realValue);
            Trader.CurrentTrader.Initialize(this);

            nextPlayerOffer = RealValue; ;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();
            nextPlayerOfferText.fontStyle = FontStyle.Italic;

            UpdateWeights(weightsPlayer, nextPlayerOffer);
            UpdateWeights(weightsTrader, realValue);

            Player.Singleton.RestrictBookUsage = false;

            initialized = true;
        }

        /// <summary>
        /// Sets the RealValue of the trade.
        /// </summary>
        void SetItemValue(int realValue = 0)
        {
            if (realValue == 0)
                realValue = defaultValue;

            RealValue = realValue;
            valueText.text = realValue.ToString();
        }

        public override float GetCurrentPlayerOffer()
        {
            return lastItem(PlayerOffers);
        }

        public override int GetPlayerOffersCount()
        {
            return PlayerOffers.Count;
        }

        public override float GetCurrentTraderOffer()
        {
            return lastItem(TraderOffers);
        }

        public override void UpdateCurrentTraderOffer(float offer)
        {
            TraderOffers.Add(offer);
        }

        /// <summary>
        /// Modifies nextPlayerOffer and its representation by weights in the scene.
        /// </summary>
        public override void ModifyOffer()
        {
            float wheelAxis = Input.GetAxis("Mouse ScrollWheel");

            int multiplier = 10;

            if (Input.GetKey(KeyCode.LeftShift))
                multiplier = 100;

            if (PlayerOffers.Count != 0 && nextPlayerOffer + (int)(wheelAxis * multiplier) >= GetCurrentPlayerOffer())
            {
                if(GetCurrentPlayerOffer() > GetCurrentTraderOffer())
                    nextPlayerOffer = (int)Math.Floor(GetCurrentPlayerOffer()) - 1;
                else
                    nextPlayerOffer = (int)Math.Floor(GetCurrentPlayerOffer());
            }
            else
            {
                nextPlayerOffer += (int)(wheelAxis * multiplier);

                if (nextPlayerOffer > RealValue * 5)
                    nextPlayerOffer = RealValue * 5;

                else if (nextPlayerOffer < 1)
                    nextPlayerOffer = 1;

                else if (nextPlayerOffer < (int)GetCurrentTraderOffer())
                    nextPlayerOffer = (int)GetCurrentTraderOffer();

                else if (nextPlayerOffer > 400)
                    nextPlayerOffer = 400;
            }

            nextPlayerOfferText.fontStyle = FontStyle.Italic;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();
            UpdateWeights(weightsPlayer, nextPlayerOffer);
        }

        /// <summary>
        /// Handles the player's offer.
        /// </summary>
        void HandlePlayerOffer()
        {
            if(TraderOffers.Count > 0 && nextPlayerOffer == (int)TraderOffers[TraderOffers.Count - 1])
            {
                Accept();
                return;
            }

            int lastPlayerOffer = -1;

            if (PlayerOffers.Count > 0)
            {
                lastPlayerOffer = (int)Math.Floor(PlayerOffers[PlayerOffers.Count - 1]);
            }

            if (nextPlayerOffer < 1 || lastPlayerOffer != -1 && lastPlayerOffer < 2)
            {
                nextPlayerOffer = 1;
            }

            else if (lastPlayerOffer != -1 && nextPlayerOffer >= lastPlayerOffer)
            {
                nextPlayerOffer = lastPlayerOffer - 1;
            }

            PlayerOffers.Add(nextPlayerOffer);

            TradeDialogue.text = "";
            Trader.CurrentTrader.ReactToPlayerOffer();

            if (TraderOffers.Count > 0)
                UpdateWeights(weightsTrader, (int)TraderOffers[TraderOffers.Count -1]);

            if(nextPlayerOffer > GetCurrentTraderOffer())
                nextPlayerOffer -= 1;

            nextPlayerOfferText.fontStyle = FontStyle.Normal;
            UpdateWeights(weightsPlayer, nextPlayerOffer);
            UpdateUI();
        }

        /// <summary>
        /// Updates the UI with numerous trading variables. This method will be deleted in the future.
        /// </summary>
        void UpdateUI()
        {
            currentPlayerOfferText.text = nextPlayerOfferText.text;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();

            if (TraderOffers.Count > 0)
            {
                traderOffer.text = Math.Floor(TraderOffers[TraderOffers.Count - 1]).ToString();
            }

            sliderIrritation.value = Trader.CurrentTrader.IrritationTotal;
            sliderSkepticism.value = Trader.CurrentTrader.SkepticismTotal;

            sliderIrritationNumber.text = Trader.CurrentTrader.IrritationTotal.ToString();
            sliderSkepticismNumber.text = Trader.CurrentTrader.SkepticismTotal.ToString();
        }

        /// <summary>
        /// Called when the player accepts the trader's offer.
        /// </summary>
        protected override void Accept()
        {
            Inventory.Singleton.Add(coinPrefab, (int)GetCurrentTraderOffer(), true);

            if (Achievements.Singleton.switchIndex == 3)
                Achievements.AddToCounter(null, false);

            if (ItemsSold != null)
                ItemsSold.Invoke(this, new TradeArgs(ItemsToSell, Trader.CurrentTrader));
            
            Trader.CurrentTrader.IncreaseReputation();
            Exit = true;

            TradeDialogue.text = "You and Dormammu have a blood-sealing pact. He wishes you a good day and rides off into the sunset.";

            if (GetCurrentTraderOffer() >= RealValue)
                Tutorial.Monolog(5);
            else
                Tutorial.Monolog(6);

        }

        /// <summary>
        /// Called when the trader doesn't want to trade anymore.
        /// </summary>
        public override void Abort()
        {
            TradeDialogue.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            Exit = true;

            foreach (InventoryItem item in ItemsToSell)
            {
                Inventory.Singleton.Add(item.Slot.ItemInfo, int.Parse(item.ItemQuantity.text.Replace("x", "")), true);
            }
        }

        /// <summary>
        /// Called when the player quits the trade.
        /// </summary>
        public override void Quit()
        {
            TradeDialogue.text = "U quitted coz u a lil chicken.";
            Player.RestrictCamera = false;
            Cursor.visible = false;
            Exit = true;

            foreach(InventoryItem item in ItemsToSell)
            {
                Inventory.Singleton.Add(item.Slot.ItemInfo, int.Parse(item.ItemQuantity.text.Replace("x","")), true);
            }
        }

        /// <summary>
        /// Gets the last Item of a List<float>.
        /// </summary>
        float lastItem(List<float> list, int beforeLast = 0)
        {
            if (list.Count == 0)
                return -1;

            return list[list.Count - (1 + beforeLast)];
        }
    }
}