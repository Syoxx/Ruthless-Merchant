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

        #endregion

        #region Serialized Fields

        [SerializeField, Range(1, 50)]
        float weightsDeltaModifier;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField]
        int nextPlayerOffer;

        [SerializeField]
        Text TradeDialogue;

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
                // TODO: Uncomment this
                //exitTimer += Time.deltaTime;

                //if (exitTimer > 3)
                //{
                //    Cursor.visible = false;
                //    Destroy(gameObject);
                //}
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

                else if (Input.GetKeyDown(KeyCode.E) && TraderOffers.Count > 0)
                {
                    Accept();
                }

                #if UNITY_EDITOR
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    Quit();
                }
                #else
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Quit();
                }
                #endif
            }

            // TODO: Delete this
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }

        #endregion

        /// <summary>
        /// Modifies nextPlayerOffer and its representation by weights in the scene.
        /// </summary>
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

                else if (nextPlayerOffer < 1)
                    nextPlayerOffer = 1;
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
            Trader.CurrentTrader.ReactToPlayerOffer(this);

            if (TraderOffers.Count > 0)
                UpdateWeights(weightsTrader, (int)TraderOffers[TraderOffers.Count -1]);

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
        void Accept()
        {
            TradeDialogue.text = "You and Dormammu have a blood-sealing pact. He wishes you a good day and rides off into the sunset.";
            exit = true;
        }

        /// <summary>
        /// Called when the trader doesn't want to trade anymore.
        /// </summary>
        public void Abort()
        {
            TradeDialogue.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            exit = true;
        }

        /// <summary>
        /// Called when the player quits the trade.
        /// </summary>
        public void Quit()
        {
            TradeDialogue.text = "U quitted coz u a lil chicken.";
            exit = true;
        }

        /// <summary>
        /// Updates the weight representation in the scene.
        /// </summary>
        /// <param name="weights">The weight group to be updated (either weightsPlayer or weightsTrader).</param>
        /// <param name="offer">The offer to be represented.</param>
        void UpdateWeights(List<List<GameObject>> weights, int offer)
        {
            int[] targetWeights = GetTargetWeights(offer);

            UpdateWeight(weights, targetWeights, 0);
            UpdateWeight(weights, targetWeights, 1);
            UpdateWeight(weights, targetWeights, 2);
            UpdateWeight(weights, targetWeights, 3);

            if (TraderOffers.Count > 0)
            {
                float playerTraderOfferDelta = ((float)nextPlayerOffer / (int)(float)TraderOffers[TraderOffers.Count - 1] - 1) / weightsDeltaModifier;

                if (playerTraderOfferDelta > 0.75f)
                    playerTraderOfferDelta = 0.75f;

                else if (playerTraderOfferDelta < -0.75f)
                    playerTraderOfferDelta = -0.75f;

                weightsPlayerParent.position += new Vector3(0, -weightsPlayerParent.position.y - playerTraderOfferDelta, 0);
                weightsTraderParent.position += new Vector3(0, -weightsTraderParent.position.y + playerTraderOfferDelta, 0);
            }
        }

        /// <summary>
        /// Updates a single weight group in the scene.
        /// </summary>
        /// <param name="weights">The weight group to be updated.</param>
        /// <param name="targetWeights">The array of all target weight groups.</param>
        /// <param name="weightIndex">The array index that specifies the weight group</param>
        void UpdateWeight(List<List<GameObject>> weights, int[] targetWeights, int weightIndex)
        {
            for (int x = weights[weightIndex].Count - 1; x >= targetWeights[weightIndex]; x--)
            {
                weights[weightIndex][x].SetActive(false);
            }

            for (int x = 0; x < targetWeights[weightIndex]; x++)
            {
                weights[weightIndex][x].SetActive(true);
            }
        }

        /// <summary>
        /// Gets the weight children of a specified weightParent in the scene.
        /// </summary>
        /// <param name="weightsParent">The weightsParent in which to look for weights.</param>
        /// <returns>A list with four GameObject Lists. Each List includes a weight group.</returns>
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

        /// <summary>
        /// Calculates how an offer should be represented in weights.
        /// </summary>
        /// <param name="offer">The amount and type of weights the offer would be represented with.</param>
        /// <returns>An array with four integers, every one of them describes how many weights in their weight group should be present.</returns>
        int[] GetTargetWeights(int offer)
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