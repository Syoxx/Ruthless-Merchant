using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Trade : MonoBehaviour
    {
        public static Trade Singleton;

        // TODO: Delete this
        public static string PreviousScene;

        public Trader Trader;

        public Item Item;

        public float currentPlayerOffer;

        float currentTraderOffer;
        public float CurrentTraderOffer
        {
            get
            {
                return currentTraderOffer;
            }
            set
            {
                currentTraderOffer = (int)value;
            }
        }

        float abortTimer = 0;
        float abortTimerLimit = 2;
        bool abort = false;

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

        void Awake()
        {
            Singleton = this;
        }

        void Update()
        {
            if (abort)
            {
                abortTimer += Time.deltaTime;

                if(abortTimer > abortTimerLimit)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(PreviousScene);
                }
            }
        }

        public void UpdateUI(bool exits)
        {
            sliderIrritation.value = Trader.irritationTotal;
            sliderSkepticism.value = Trader.skepticismTotal;

            if (!exits)
            {
                playerOfferInputField.text = currentPlayerOffer.ToString();
                traderOffer.text = currentTraderOffer.ToString();
            }
        }

        public void Accept()
        {
            bargainEventsText.text = "Dormammu accepts your offer.";
        }

        public void Abort()
        {
            bargainEventsText.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            abort = true;
        }
    }
}