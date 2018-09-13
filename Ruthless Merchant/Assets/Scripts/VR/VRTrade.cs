﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

namespace RuthlessMerchant
{
    public class VRTrade : TradeAbstract
    {
        [SerializeField]
        VRPlayerTradeZone vrTradePlayer;

        [SerializeField]
        Hand hand1;

        [SerializeField]
        Hand hand2;

        public Transform WeightContent;

        int totalPlayerOffers = 0;
        float currentPlayerOffer = -1;
        float currentTraderOffer = -1;

        protected override void Awake()
        {
            base.Awake();
            Trader.CurrentTrader = FindObjectOfType<Trader>();
        }

        void Start()
        {
            weightsTrader = GetPresentWeights(TraderZone);

            for (int x = 0; x < weightsTrader.Count; x++)
            {
                for (int y = 0; y < weightsTrader[x].Count; y++)
                {
                    weightsTrader[x][y].SetActive(false);
                }
            }

            NeutralPositionY = PlayerZone.transform.position.y;
        }

        public override void Initialize(int realValue = defaultValue)
        {
            Cursor.visible = true;
            SetItemValue(realValue);
            Trader trader = FindObjectOfType<Trader>();
            Trader.CurrentTrader = trader;
            trader.Initialize(this);

            nextPlayerOffer = RealValue; ;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();
            nextPlayerOfferText.fontStyle = FontStyle.Italic;

            UpdateWeights(weightsTrader, realValue);
        }

        private void Update()
        {
            if (VRSmithing.Singleton.smithingSteps == VRSmithing.SmithingSteps.Trading && (Input.GetKeyDown(KeyCode.E) || hand1 != null && hand1.controller.GetHairTriggerDown()))
            {
                HandlePlayerOffer();
            }
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

        public override void ModifyOffer()
        {
            nextPlayerOffer = vrTradePlayer.TotalWeight;

            nextPlayerOfferText.fontStyle = FontStyle.Italic;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();

            UpdateWeights(weightsTrader, (int)currentTraderOffer);
        }

        public override float GetCurrentPlayerOffer()
        {
            return currentPlayerOffer;
        }

        public override int GetPlayerOffersCount()
        {
            return totalPlayerOffers;
        }

        public override float GetCurrentTraderOffer()
        {
            return currentTraderOffer;
        }

        public override void UpdateCurrentTraderOffer(float offer)
        {
            currentTraderOffer = offer;
        }

        /// <summary>
        /// Called when the player accepts the trader's offer.
        /// </summary>
        protected override void Accept()
        {
            tradeDialogue.text = "You and Dormammu have a blood-sealing pact. He wishes you a good day and rides off into the sunset.";
            ExitTrade();
        }

        /// <summary>
        /// Called when the trader doesn't want to trade anymore.
        /// </summary>
        public override void Abort()
        {
            tradeDialogue.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            ExitTrade();
        }

        /// <summary>
        /// Called when the player quits the trade.
        /// </summary>
        public override void Quit()
        {
            tradeDialogue.text = "U quitted coz u a lil chicken.";
            ExitTrade();
        }

        /// <summary>
        /// Exits trading.
        /// </summary>
        void ExitTrade()
        {
            Exit = true;
            VRSmithing.Singleton.FinalSword.SetActive(false);
            VRSmithing.Singleton.FireworkEnd.SetActive(true);
            Invoke("LoadIslandScene", 3);
        }

        void LoadIslandScene()
        {
            SceneManager.LoadScene("VR_IslandScene");
        }

        /// <summary>
        /// Handles the player's offer.
        /// </summary>
        void HandlePlayerOffer()
        {
            if (GetCurrentTraderOffer() != -1 && nextPlayerOffer == GetCurrentTraderOffer())
            {
                Accept();
                return;
            }

            int lastPlayerOffer = -1;

            if (GetPlayerOffersCount() > 0)
            {
                lastPlayerOffer = (int)Math.Floor(GetCurrentPlayerOffer());
            }

            if (nextPlayerOffer < 1 || lastPlayerOffer != -1 && lastPlayerOffer < 2)
            {
                nextPlayerOffer = 1;
            }

            currentPlayerOffer = nextPlayerOffer;

            tradeDialogue.text = "";
            Trader.CurrentTrader.ReactToPlayerOffer();

            VRPlayerTradeZone.Singleton.UpdateWeight = true;

            nextPlayerOffer -= 1;
            nextPlayerOfferText.fontStyle = FontStyle.Normal;
            UpdateUI();
        }

        /// <summary>
        /// Updates the UI with numerous trading variables. This method will be deleted in the future.
        /// </summary>
        void UpdateUI()
        {
            currentPlayerOfferText.text = nextPlayerOfferText.text;
            nextPlayerOfferText.text = nextPlayerOffer.ToString();

            if (GetCurrentTraderOffer() != -1)
            {
                traderOffer.text = Math.Floor(GetCurrentTraderOffer()).ToString();
            }

            sliderIrritation.value = Trader.CurrentTrader.IrritationTotal;
            sliderSkepticism.value = Trader.CurrentTrader.SkepticismTotal;

            sliderIrritationNumber.text = Trader.CurrentTrader.IrritationTotal.ToString();
            sliderSkepticismNumber.text = Trader.CurrentTrader.SkepticismTotal.ToString();
        }
    }
}