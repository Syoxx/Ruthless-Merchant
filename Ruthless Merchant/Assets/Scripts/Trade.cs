using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Trade : MonoBehaviour
    {
        public static Trade Singleton;

        #region Public Fields

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public Item Item;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public List<float> PlayerOffers;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public List<float> TraderOffers;

        public Text TradeDialogue;

        #endregion

        #region Serialized Fields

        [SerializeField]
        Slider sliderIrritation;

        [SerializeField]
        Text sliderIrritationNumber;

        [SerializeField]
        Slider sliderSkepticism;

        [SerializeField]
        Text sliderSkepticismNumber;

        [SerializeField]
        Text traderOffer;

        [SerializeField]
        Text nextPlayerOfferText;

        [SerializeField]
        Text currentPlayerOfferText;

        [SerializeField]
        Transform weightsPlayerParent;

        [SerializeField]
        Transform weightsTraderParent;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField]
        int nextPlayerOffer;

        [SerializeField, Range(1, 50)]
        float weightsDeltaModifier;

        #endregion

        #region Private Fields

        List<List<GameObject>> weightsPlayer;

        List<List<GameObject>> weightsTrader;

        float exitTimer = 0;

        bool exit = false;

        #endregion

        #region MonoBehaviour Life Cycle

        void Awake()
        {
            Singleton = this;
            PlayerOffers = new List<float>();
            TraderOffers = new List<float>();
            weightsPlayer = new List<List<GameObject>>();
            weightsTrader = new List<List<GameObject>>();
        }

        void Start()
        {
            Cursor.visible = true;
            GetComponent<ItemSetter>().SetTrade(this);
            Trader.CurrentTrader.Initialize(this);

            nextPlayerOffer = Item.ItemValue[0].Count;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();
            nextPlayerOfferText.fontStyle = FontStyle.Italic;
            weightsPlayer = GetPresentWeights(weightsPlayerParent);
            weightsTrader = GetPresentWeights(weightsTraderParent);

            for (int x = 0; x < weightsPlayer.Count; x++)
            {
                for(int y = 0; y < weightsPlayer[x].Count; y++)
                {
                    weightsPlayer[x][y].SetActive(false);
                    weightsTrader[x][y].SetActive(false);
                }
            }

            UpdateWeights(weightsPlayer, nextPlayerOffer);
        }

        void Update()
        {
            if (exit)
            {
                exitTimer += Time.deltaTime;

                if (exitTimer > 3)
                {
                    Cursor.visible = false;
                    Destroy(gameObject);
                }
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    ModifyOffer();
                }

                else if (Input.GetMouseButtonDown(0))
                {
                    HandlePlayerOffer();
                }

                else if (Input.GetKeyDown(KeyCode.E))
                {
                    Accept();
                }

                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    TradeDialogue.text = "U quitted coz u a lil chicken.";
                    exit = true;
                }
            }
        }

        #endregion

        void ModifyOffer()
        {
            float wheelAxis = Input.GetAxis("Mouse ScrollWheel");

            int multiplier = 10;

            if (Input.GetKey(KeyCode.LeftShift))
                multiplier = 100;

            if (PlayerOffers.Count != 0 && nextPlayerOffer + (int)(wheelAxis * multiplier) >= PlayerOffers[PlayerOffers.Count - 1])
            {
                nextPlayerOffer = (int)Math.Floor(PlayerOffers[PlayerOffers.Count - 1]) - 1;
            }
            else
            {
                nextPlayerOffer += (int)(wheelAxis * multiplier);

                if(nextPlayerOffer > 400)
                    nextPlayerOffer = 400;

                else if (nextPlayerOffer < 0)
                    nextPlayerOffer = 0;
            }

            nextPlayerOfferText.fontStyle = FontStyle.Italic;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();
            UpdateWeights(weightsPlayer, nextPlayerOffer);
        }

        void HandlePlayerOffer()
        {
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
            Trader.CurrentTrader.ReactToPlayerOffer(this);

            if (TraderOffers.Count > 0)
                UpdateWeights(weightsTrader, (int)TraderOffers[TraderOffers.Count -1]);

            nextPlayerOffer -= 1;
            nextPlayerOfferText.fontStyle = FontStyle.Normal;
            UpdateUI();
        }

        void UpdateUI()
        {
            currentPlayerOfferText.text = nextPlayerOfferText.text;

            if (TraderOffers.Count > 0)
            {
                traderOffer.text = Math.Floor(TraderOffers[TraderOffers.Count - 1]).ToString();
            }

            sliderIrritation.value = Trader.CurrentTrader.IrritationTotal;
            sliderSkepticism.value = Trader.CurrentTrader.SkepticismTotal;

            sliderIrritationNumber.text = Trader.CurrentTrader.IrritationTotal.ToString();
            sliderSkepticismNumber.text = Trader.CurrentTrader.SkepticismTotal.ToString();
        }

        void Accept()
        {
            TradeDialogue.text = "You and Dormammu have a blood-sealing pact. He wishes you a good day and rides off into the sunset.";
            exit = true;
        }

        public void Abort()
        {
            TradeDialogue.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            exit = true;
        }

        void UpdateWeights(List<List<GameObject>> weights, int offer)
        {
            int[] wantedWeights = GetWantedWeights(offer);

            HandleWeight(weights, wantedWeights, 0);
            HandleWeight(weights, wantedWeights, 1);
            HandleWeight(weights, wantedWeights, 2);
            HandleWeight(weights, wantedWeights, 3);

            if (TraderOffers.Count > 0)
            {
                float playerTraderOfferDelta = ((float)nextPlayerOffer / (float)TraderOffers[TraderOffers.Count - 1] - 1) / weightsDeltaModifier;

                if (playerTraderOfferDelta > 0.75f)
                    playerTraderOfferDelta = 0.75f;

                else if (playerTraderOfferDelta < -0.75f)
                    playerTraderOfferDelta = -0.75f;

                weightsPlayerParent.position += new Vector3(0, -weightsPlayerParent.position.y - playerTraderOfferDelta, 0);
                weightsTraderParent.position += new Vector3(0, -weightsTraderParent.position.y + playerTraderOfferDelta, 0);
            }
        }

        void HandleWeight(List<List<GameObject>> presentWeightsGaObj, int[] wantedWeights, int weightIndex)
        {
            for (int x = presentWeightsGaObj[weightIndex].Count - 1; x >= 0; x--)
            {
                presentWeightsGaObj[weightIndex][x].SetActive(false);
            }

            for (int x = 0; x < wantedWeights[weightIndex]; x++)
            {
                presentWeightsGaObj[weightIndex][x].SetActive(true);
            }
        }

        List<List<GameObject>> GetPresentWeights(Transform weightsParent)
        {
            List<GameObject> weights1 = new List<GameObject>();
            List<GameObject> weights5 = new List<GameObject>();
            List<GameObject> weights10 = new List<GameObject>();
            List<GameObject> weights50 = new List<GameObject>();

            UniqueIDGenerator[] weights = weightsParent.GetComponentsInChildren<UniqueIDGenerator>(true);

            foreach(UniqueIDGenerator weight in weights)
            {
                if (weight.name == "Weight1")
                {
                    weights1.Add(weight.gameObject);
                }

                else if (weight.name == "Weight5")
                {
                    weights5.Add(weight.gameObject);
                }

                else if (weight.name == "Weight10")
                {
                    weights10.Add(weight.gameObject);
                }

                else if (weight.name == "Weight50")
                {
                    weights50.Add(weight.gameObject);
                }
            }

            List<List<GameObject>> result = new List<List<GameObject>>
            {
                weights1,
                weights5,
                weights10,
                weights50
            };

            return result;
        }

        int[] GetWantedWeights(int offer)
        {
            int w50 = (int)((float)offer / 50f);
            int w10 = (int)((float)(offer - w50 * 50) / 10f);
            int w5 = (int)((float)(offer - w50 * 50 - w10 * 10) / 5f);
            int w1 = offer - w50 * 50 - w10 * 10 - w5 * 5;

            return new int[]
            {
                w1,
                w5,
                w10,
                w50
            };
        }
    }
}