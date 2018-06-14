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

        [SerializeField, ReadOnly]
        float upperLimitPerCent = 1.25f;

        [SerializeField, ReadOnly]
        float underLimitPerCent = 0.35f;

        [SerializeField, ReadOnly]
        float upperLimitBargainPerCent = 1.5f;

        #endregion

        #region Price Variables

        [Header("Price Variables")]

        [SerializeField, ReadOnly]
        List<float> wished;

        [SerializeField, ReadOnly]
        float upperLimitReal;

        [SerializeField, ReadOnly]
        float upperLimitBargain;

        [SerializeField, ReadOnly]
        float underLimitReal;

        [SerializeField, ReadOnly]
        List<float> realAndOfferedRatio;

        [SerializeField, ReadOnly]
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

        [SerializeField, ReadOnly]
        float skepticismDelta;

        [SerializeField, Range(0, 100)]
        float skepticismDeltaModifier;

        [SerializeField, Range(0, 100)]
        public float IrritationTotal;

        [SerializeField, Range(0, 100)]
        public float IrritationLimit;

        [SerializeField, Range(0, 100)]
        float irritationStart;

        [SerializeField, Range(0, 100)]
        float irritationStartLimit;

        [SerializeField, ReadOnly]
        float irritationDelta;

        [SerializeField, Range(0, 100)]
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
                if (itemValue.Length == 1 && itemValue[0].Item.Type == ItemType.Other)
                {
                    float realPrice = itemValue[0].Count;

                    upperLimitReal      = (float)Math.Ceiling(realPrice * (upperLimitPerCent));
                    upperLimitBargain   = (float)Math.Ceiling(upperLimitReal * (upperLimitBargainPerCent));
                    underLimitReal      = (float)Math.Floor(realPrice * (1 - underLimitPerCent));
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

            Trade.PreviousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene("BargainTestScene");
        }
    }
}