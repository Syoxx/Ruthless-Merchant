<<<<<<< HEAD:Ruthless Merchant/Assets/Scripts/Trader.cs
﻿using System;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> 1b80879c31900804716bfbc5ecabe760b947ea67:Ruthless Merchant/Assets/Scripts/NPC/Trader.cs

namespace RuthlessMerchant
{
    public class Trader : Civilian
    {
        #region Editor Separation

        [Space(3)]
        [Header("_____________________________")]
        [Space(5)]

        #endregion

        #region Influence Properties

        [Header("Influence Variables")]

        #region Reputation

        [SerializeField, Range(0, 1)]
        float influenceFraction;

        [SerializeField, Range(0, 1)]
        float influenceIndividual;

        #endregion

        #region Situation

        [SerializeField, Range(0, 1)]
        float influenceWar;

        [SerializeField, Range(0, 1)]
        float influenceNeighbours;

        #endregion

        #endregion

        #region Deviation Constants

        const float upperLimitPerCent = 0.25f;

        const float underLimitPerCent = 0.35f;

        const float upperLimitBargainPerCent = 0.5f;

        #endregion

        #region Price Variables

        [Header("Price Variables")]

        [SerializeField, ReadOnly]
        float wished;

        [SerializeField, ReadOnly]
        float upperLimitReal;

        [SerializeField, ReadOnly]
        float upperLimitBargain;

        [SerializeField, ReadOnly]
        float underLimitReal;

        [SerializeField, ReadOnly]
        float realAndOfferedRatio;

        #endregion

        #region Psychological Variables

        [Header("Psychological Variables")]

        [SerializeField, Range(0, 100)]
        public float skepticismTotal;

        [SerializeField, ReadOnly]
        float skepticismDelta;

        [SerializeField, Range(0, 100)]
        float skepticismDeltaModifier;

        [SerializeField, Range(0, 100)]
        public float irritationTotal;

        [SerializeField, ReadOnly]
        float irritationDelta;

        [SerializeField, Range(0, 100)]
        float irritationDeltaModifier;

        #endregion

        #region MonoBehaviour cycle

        public override void Start()
        {
            Trade.Singleton.Trader = this;
            SetInitialVariables();
        }

        public override void Update()
        {
        }

        #endregion

        public void SetInitialVariables()
        {
            Trade trade = Trade.Singleton;

            ItemValue[] itemValue = trade.Item.ItemValue;

            if (itemValue.Length == 1 && itemValue[0].Item.Type == ItemType.Other)
            {
                float realPrice = itemValue[0].Count;

                upperLimitReal      = Mathf.RoundToInt(realPrice * (1 + upperLimitPerCent));
                upperLimitBargain   = Mathf.RoundToInt(upperLimitReal * (1 + upperLimitBargainPerCent));
                underLimitReal      = Mathf.RoundToInt(realPrice * (1 - underLimitPerCent));
                wished              = Mathf.RoundToInt((upperLimitReal + underLimitReal) / 2);
            }
            else
            {
                Debug.LogError("Item Value not supported yet.");
            }
        }

        public void HandlePlayerOffer()
        {
            Trade trade = Trade.Singleton;

            bool tradeExit = false;

            realAndOfferedRatio = trade.currentPlayerOffer / trade.Item.ItemValue[0].Count;

            if (trade.currentPlayerOffer > upperLimitBargain)
            {
                // Trader refuses and doesn't want to bargain with the offered price.
                trade.CurrentTraderOffer = 0;
            }
            else
            {
                if(trade.currentPlayerOffer < wished)
                {
                    // Trader accepts.
                    tradeExit = true;
                    trade.CurrentTraderOffer = trade.currentPlayerOffer;
                    trade.Accept();
                }
                else
                {
                    // Trader makes first counteroffer.
                    trade.CurrentTraderOffer = underLimitReal + (wished - underLimitReal) / realAndOfferedRatio;
                }
            }

            UpdatePsychology();
            trade.UpdateUI(tradeExit);
        }

        void UpdatePsychology()
        {
            Trade trade = Trade.Singleton;

            if (realAndOfferedRatio < 1)
                irritationDelta = irritationDeltaModifier;
            else
                irritationDelta = irritationDeltaModifier * realAndOfferedRatio;

            if (trade.Item.ItemValue[0].Count < underLimitReal)
                skepticismDelta = 100;
            else
                skepticismDelta = skepticismDeltaModifier / realAndOfferedRatio;

            // TODO: Delete this

            irritationTotal = irritationDelta;
            skepticismTotal = skepticismDelta;

            if (irritationTotal > 100 || skepticismTotal > 100)
                trade.Abort();
        }

        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Trader");
        }
    }
}