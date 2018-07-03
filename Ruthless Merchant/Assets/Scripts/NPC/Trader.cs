// Alberto Lladó

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Trader : Civilian
    {
        #region Editor Separation

        [Space(3)]
        [Header("_____________________________")]
        [Space(5)]

        #endregion

        [SerializeField]
        GameObject tradePrefab;

        #region Influence Properties

        [Header("Influence Variables")]

        [SerializeField, Range(0, 1), Tooltip("Ruf: Fraktion")]
        float influenceFraction;

        [SerializeField, Range(0, 1), Tooltip("Ruf: Individuell")]
        float influenceIndividual;

        [SerializeField, Range(0, 1), Tooltip("Situation: Krieg")]
        float influenceWar;

        [SerializeField, Range(0, 1), Tooltip("Situation: Nachbarn")]
        float influenceNeighbours;

        #endregion

        #region Deviation Constants

        [SerializeField, Range(0, 1), Tooltip("Obergrenze %")]
        float upperLimitPerCent = 0.25f;

        [SerializeField, Range(0, 1), Tooltip("Untergrenze %")]
        float underLimitPerCent = 0.35f;

        [SerializeField, Range(0, 1), Tooltip("Feilschfaktor Obergrenze")]
        float upperLimitBargainPerCent = 0.5f;

        [SerializeField, Tooltip("(Obergrenze %) summiert")]
        float upperLimitPercentTotal;

        [SerializeField, Tooltip("(Untergrenze %) summiert")]
        float underLimitPercentTotal;

        #endregion

        #region Price Variables

        [Header("Price Variables")]

        [SerializeField, Tooltip("Wunschwerte")]
        List<float> wished;

        [SerializeField, Tooltip("(Obergrenze) absolut von Realwert")]
        float upperLimitReal;

        [SerializeField, Tooltip("(Obergrenze) Schmerzgrenze Feilschen")]
        float upperLimitBargain;

        [SerializeField, Tooltip("(Untergrenze) absolut von Realwert")]
        float underLimitReal;

        [SerializeField, Tooltip("Faktoren zu Realwert")]
        List<float> realAndOfferedRatio;

        [SerializeField, Tooltip("Faktoren zu Wunschwert")]
        List<float> wishedAndOfferedRatio;

        #endregion

        #region Psychological Variables

        [Header("Psychological Variables")]

        [SerializeField, Range(0, 100)]
        public float SkepticismTotal;

        [SerializeField, Range(0, 100)]
        public float SkepticismLimit;

        [SerializeField, Range(0, 100)]
        float skepticismStart;

        [SerializeField, Range(0, 100)]
        float skepticismStartLimit;

        [SerializeField]
        float skepticismDelta;

        [SerializeField, Range(0, 100), Tooltip("Skepsis plus Grundwert")]
        float skepticismDeltaModifier;

        [SerializeField, Range(0, 100)]
        public float IrritationTotal;

        [SerializeField, Range(0, 100)]
        public float IrritationLimit;

        [SerializeField, Range(0, 100)]
        float irritationStart;

        [SerializeField, Range(0, 100)]
        float irritationStartLimit;

        [SerializeField]
        float irritationDelta;

        [SerializeField, Range(0, 100), Tooltip("Genervt plus Grundwert")]
        float irritationDeltaModifier;

        #endregion

        #region MonoBehaviour cycle

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        #endregion

        /// <summary>
        /// Sets the initial trading variables.
        /// </summary>
        public void SetInitialVariables()
        {
            wished                = new List<float>();
            realAndOfferedRatio   = new List<float>();
            wishedAndOfferedRatio = new List<float>();

            Trade trade = Trade.Singleton;

            ItemValue[] itemValue = trade.Item.ItemValue;

            IrritationTotal = irritationStart;
            SkepticismTotal = skepticismStart;

            if (IrritationTotal > 100 || SkepticismTotal > 100)
            {
                trade.Abort();
            }
            else
            {
                if (itemValue.Length == 1 && itemValue[0].Item.itemType == ItemType.Other)
                {
                    float realPrice = itemValue[0].Count;

                    upperLimitPercentTotal = upperLimitPerCent + ((upperLimitPerCent * influenceFraction) + (upperLimitPerCent * influenceIndividual) + (upperLimitPerCent * influenceWar) + (upperLimitPerCent * influenceNeighbours)) / 4;
                    underLimitPercentTotal = underLimitPerCent + (-(underLimitPerCent * influenceFraction) - (underLimitPerCent * influenceIndividual) + (underLimitPerCent * influenceWar) + (underLimitPerCent * influenceNeighbours)) / 4;

                    upperLimitReal = (float)Math.Ceiling(realPrice * (1 + upperLimitPercentTotal));
                    upperLimitBargain   = (float)Math.Ceiling(upperLimitReal * (1 + upperLimitBargainPerCent));
                    underLimitReal      = (float)Math.Floor(realPrice * (1 - underLimitPercentTotal));
                    wished.Add((float)Math.Floor((upperLimitReal + underLimitReal) / 2));
                }
                else
                {
                    Debug.LogError("Item Value not supported yet.");
                }
            }
        }

        public void HandlePlayerOffer()
        {
            Trade trade = Trade.Singleton;

            realAndOfferedRatio.Add(lastItem(trade.PlayerOffers) / trade.Item.ItemValue[0].Count);
            wishedAndOfferedRatio.Add(lastItem(trade.PlayerOffers) / lastItem(wished));
            wished.Add((float)Math.Ceiling(upperLimitReal - ((upperLimitReal - lastItem(wished)) / lastItem(wishedAndOfferedRatio))));

            if (UpdatePsychology())
            {
                if (trade.TotalPlayerOffers == 1)
                {
                    HandleFirstPlayerOffer();
                    trade.AcceptButton.interactable = true;
                }
                else
                {
                    HandleLaterPlayerOffers();
                }
            }
        }

        /// <summary>
        /// Handles the first offer given by the player.
        /// </summary>
        /// <returns>True if the Trade</returns>
        void HandleFirstPlayerOffer()
        {
            Trade trade = Trade.Singleton;

            if (trade.PlayerOffers[0] > upperLimitBargain)
            {
                // Trader refuses and doesn't want to bargain with the offered price.
                trade.TraderOffers.Add(0);
            }
            else
            {
                if(trade.PlayerOffers[0] < wished[0])
                {
                    // Trader accepts.
                    trade.TraderOffers.Add(trade.PlayerOffers[0]);
                }
                else
                {
                    // Trader makes first counteroffer.
                    trade.TraderOffers.Add(underLimitReal + (wished[0] - underLimitReal) / wishedAndOfferedRatio[0]);
                }
            }
        }

        void HandleLaterPlayerOffers()
        {
            Trade trade = Trade.Singleton;

            if (lastItem(trade.PlayerOffers) <= lastItem(wished, 1))
            {
                trade.TraderOffers.Add(lastItem(trade.PlayerOffers));
            }
            else
            {
                trade.TraderOffers.Add(lastItem(trade.TraderOffers, 0, "DOWN") + (lastItem(wished,1) - lastItem(trade.TraderOffers, 0, "DOWN")) / lastItem(wishedAndOfferedRatio));
            }
        }

        float lastItem(List<float> list, int beforeLast = 0, string round = "")
        {
            float result = list[list.Count - (1 + beforeLast)];

            switch(round)
            {
                case "UP":
                    return (float)Math.Ceiling(result);

                case "DOWN":
                    return (float)Math.Floor(result);

                default:
                    return result;
            }            
        }

        /// <summary>
        /// Updates the irritation and skepticism levels.
        /// </summary>
        bool UpdatePsychology()
        {
            Trade trade = Trade.Singleton;

            if (realAndOfferedRatio[realAndOfferedRatio.Count - 1] < 1)
            {
                irritationDelta = irritationDeltaModifier;
            }
            else
            {
                irritationDelta = irritationDeltaModifier * realAndOfferedRatio[realAndOfferedRatio.Count - 1];
            }

            if (trade.Item.ItemValue[0].Count < underLimitReal)
            {
                skepticismDelta = 100;
            }
            else
            {
                skepticismDelta = skepticismDeltaModifier / realAndOfferedRatio[realAndOfferedRatio.Count - 1];
            }

            IrritationTotal += irritationDelta;
            SkepticismTotal += skepticismDelta;

            if (IrritationTotal > 100 || SkepticismTotal > 100)
            {
                trade.Abort();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the Trading Scene.
        /// </summary>
        /// <param name="caller"></param>
        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Trader");

            Trade trade = Instantiate(tradePrefab, transform).GetComponent<Trade>();
            trade.Trader = this;
            trade.InitializeTrade();
        }
    }
}