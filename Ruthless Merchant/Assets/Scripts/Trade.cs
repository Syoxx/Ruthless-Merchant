﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Trade : MonoBehaviour
    {
        public static Trade Singleton;

        public float TotalPlayerOffers { get; private set; }

        public Trader Trader;

        public Item Item;

        public List<float> PlayerOffers;
        public List<float> TraderOffers;

        float exitTimer = 0;
        bool exit = false;

        [SerializeField]
        Text traderOffer;

        [SerializeField]
        InputField playerOfferInputField;

        public Text BargainEventsText;

        public Button AcceptButton;

        [SerializeField]
        Slider sliderIrritation;

        [SerializeField]
        Text sliderIrritationNumber;

        [SerializeField]
        Slider sliderSkepticism;

        [SerializeField]
        Text sliderSkepticismNumber;

        #region MonoBehaviour Life Cycle

        public Trade(Trader trader)
        {
            Singleton = this;
        }

        void Awake()
        {
            Singleton = this;
        }

        public void InitializeTrade()
        {
            GetComponent<ItemSetter>().SetTrade(this);

            Trader.SetInitialVariables();

            TotalPlayerOffers = 0;

            playerOfferInputField.onEndEdit.AddListener(Player.Singleton.MakeOffer);
            AcceptButton.onClick.AddListener(Accept);
            
            if(Trader.SkepticismTotal >= Trader.SkepticismLimit || Trader.IrritationTotal >= Trader.IrritationLimit)
            {
                Abort();
            }

            Cursor.visible = true;
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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Cursor.visible = false;
                Destroy(gameObject);
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

            sliderIrritationNumber.text = Trader.IrritationTotal.ToString();
            sliderSkepticismNumber.text = Trader.SkepticismTotal.ToString();

            playerOfferInputField.text = PlayerOffers[PlayerOffers.Count - 1].ToString();
            traderOffer.text = Math.Floor(TraderOffers[TraderOffers.Count - 1]).ToString() + " (" + TraderOffers[TraderOffers.Count - 1].ToString() + ")";
        }

        public void AugmentTotalPlayerOffers()
        {
            TotalPlayerOffers++;
        }

        public void Accept()
        {
            playerOfferInputField.interactable = false;
            AcceptButton.interactable = false;
            BargainEventsText.text = "You and Dormammu have a blood-sealing pact. He wishes you a good day and rides off into the sunset.";
            exit = true;
        }

        public void Abort()
        {
            playerOfferInputField.interactable = false;
            BargainEventsText.text = "Dormammu tells you to fuck off and rides off with his galaxy-eating unicorn.";
            exit = true;
        }
    }
}