using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Trade : MonoBehaviour
    {
        public static Trade Singleton;

        public float TotalPlayerOffers { get; private set; }

        // TODO: Delete this
        public static string PreviousScene;

        public Trader Trader;

        public Item Item;

        public List<float> PlayerOffers;
        public List<float> TraderOffers;

        float exitTimer = 0;
        float exitTimerLimit = 2;
        bool exit = false;

        [SerializeField]
        Text traderOffer;

        [SerializeField]
        InputField playerOfferInputField;

        [SerializeField]
        public Text bargainEventsText;

        [SerializeField]
        Slider sliderIrritation;

        [SerializeField]
        Slider sliderSkepticism;

        #region MonoBehaviour Life Cycle

        public Trade(Trader trader)
        {
            Singleton = this;
            Trader = trader;
        }

        void Awake()
        {
            // TODO: Delete this
            Singleton = this;
        }

        void Start()
        {
            // TODO: Delete this
            ItemSetter.Singleton.SetTrade();

            Trader.SetInitialVariables();

            TotalPlayerOffers = 0;
            
            if(Trader.SkepticismTotal >= Trader.SkepticismLimit || Trader.IrritationTotal >= Trader.IrritationLimit)
            {
                Abort();
            }
        }

        void Update()
        {
            if (exit)
            {
                exitTimer += Time.deltaTime;

                if(exitTimer > exitTimerLimit)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(PreviousScene);
                }
            }
        }

        #endregion

        public void UpdateTrading()
        {
            Trader.HandlePlayerOffer();

            UpdateUI();
        }

        public void UpdateUI()
        {
            sliderIrritation.value = Trader.IrritationTotal;
            sliderSkepticism.value = Trader.SkepticismTotal;

            playerOfferInputField.text = PlayerOffers[PlayerOffers.Count - 1].ToString();
            traderOffer.text = TraderOffers[TraderOffers.Count - 1].ToString();
        }

        public void AugmentTotalPlayerOffers()
        {
            TotalPlayerOffers++;
        }

        public void Accept()
        {
            playerOfferInputField.interactable = false;
            bargainEventsText.text = "You and Dormammu have a blood-sealing pact. He wishes you a good day and rides off into the sunset.";
            exit = true;
        }

        public void Abort()
        {
            playerOfferInputField.interactable = false;
            bargainEventsText.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            exit = true;
        }
    }
}