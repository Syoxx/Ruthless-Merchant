// Alberto Lladó

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Trader : Civilian
    {
        public static Trader CurrentTrader;

        #region Editor Separation

        [Space(3)]
        [Header("_____________________________")]
        [Space(5)]

        #endregion

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

        [SerializeField, Tooltip("Feilschfaktor Obergrenze")]
        float upperLimitBargainPerCent;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("(Obergrenze %) summiert")]
        float upperLimitPercentTotal;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("(Untergrenze %) summiert")]
        float underLimitPercentTotal;

        #endregion

        #region Price Variables

        [Header("Price Variables")]

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("Wunschwerte")]
        List<float> wished;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("(Obergrenze) absolut von Realwert")]
        float upperLimitReal;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("(Obergrenze) Schmerzgrenze Feilschen")]
        float upperLimitBargain;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("(Untergrenze) absolut von Realwert")]
        float underLimitReal;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("Faktoren zu Realwert")]
        List<float> realAndOfferedRatio;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("Faktoren zu Wunschwert")]
        List<float> wishedAndOfferedRatio;

        #endregion

        #region Psychological Variables

        [Header("Psychological Variables")]

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField]
        bool continueTrade = true;

        [SerializeField, Range(0, 100)]
        public float SkepticismTotal;

        [SerializeField, Range(0, 100)]
        public float SkepticismLimit = 100;

        //[SerializeField, Range(0, 100)]
        //float skepticismStart;

        [SerializeField, Range(0, 100)]
        float skepticismStartLimit = 80;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField]
        float skepticismDelta;

        [SerializeField, Range(0, 100), Tooltip("Skepsis plus Grundwert")]
        float skepticismDeltaModifier;

        [SerializeField, Range(0, 100)]
        public float IrritationTotal;

        [SerializeField, Range(0, 100)]
        public float IrritationLimit = 100;

        //[SerializeField, Range(0, 100)]
        //float irritationStart;

        [SerializeField, Range(0, 100)]
        float irritationStartLimit = 80;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField]
        float irritationDelta;

        [SerializeField, Range(0, 100), Tooltip("Genervt plus Grundwert")]
        float irritationDeltaModifier;

        [SerializeField, Range(0, 2), Tooltip("Abbau pro Sekunde (Skepsis / Genervtheit)")]
        float psychoCooldown = 1;

        #endregion

        float timer = 0;

        float timerLimit = 1;

        #region MonoBehaviour cycle

        public override void Start()
        {
        }

        public override void Update()
        {
            if (CurrentTrader != this)
            {
                timer += Time.deltaTime;

                if (timer >= timerLimit)
                {
                    timer = 0;

                    if (SkepticismTotal > 0)
                        SkepticismTotal -= 1;

                    if (SkepticismTotal < 0)
                        SkepticismTotal = 0;

                    if (IrritationTotal > 0)
                        IrritationTotal -= 1;

                    if (IrritationTotal < 0)
                        IrritationTotal = 0;
                }
            }
        }

        #endregion

        /// <summary>
        /// Sets the initial trading variables.
        /// </summary>
        public void Initialize(TradeAbstract trade)
        {
            wished                = new List<float>();
            realAndOfferedRatio   = new List<float>();
            wishedAndOfferedRatio = new List<float>();

            if (IrritationTotal >= irritationStartLimit || SkepticismTotal >= skepticismStartLimit)
            {
                trade.Abort();
            }
            else
            {
                float realPrice = trade.RealValue;

                upperLimitPercentTotal = upperLimitPerCent + ((upperLimitPerCent * influenceFraction) + (upperLimitPerCent * influenceIndividual) + (upperLimitPerCent * influenceWar) + (upperLimitPerCent * influenceNeighbours)) / 4;
                underLimitPercentTotal = underLimitPerCent + (-(underLimitPerCent * influenceFraction) - (underLimitPerCent * influenceIndividual) + (underLimitPerCent * influenceWar) + (underLimitPerCent * influenceNeighbours)) / 4;

                upperLimitReal = (float)Math.Ceiling(realPrice * (1 + upperLimitPercentTotal));
                upperLimitBargain   = (float)Math.Ceiling(upperLimitReal * (1 + upperLimitBargainPerCent));
                underLimitReal      = (float)Math.Floor(realPrice * (1 - underLimitPercentTotal));
                wished.Add((float)Math.Floor((upperLimitReal + underLimitReal) / 2));
            }
        }

        /// <summary>
        /// Main method of the Trader. Called every time the player has made an offer.
        /// </summary>
        public void ReactToPlayerOffer()
        {
            TradeAbstract trade = TradeAbstract.Singleton;

            realAndOfferedRatio.Add(trade.GetCurrentPlayerOffer() / trade.RealValue);
            wishedAndOfferedRatio.Add(trade.GetCurrentPlayerOffer() / lastItem(wished));
            wished.Add((float)Math.Ceiling(upperLimitReal - ((upperLimitReal - lastItem(wished)) / lastItem(wishedAndOfferedRatio))));

            if (UpdatePsychology())
            {
                if (trade.GetCurrentPlayerOffer() == 1)
                {
                    HandleFirstPlayerOffer();
                }
                else
                {
                    HandleLaterPlayerOffers();
                }
            }

            if (IrritationTotal >= irritationStartLimit || SkepticismTotal >= skepticismStartLimit)
            {
                continueTrade = false;
            }
        }

        /// <summary>
        /// Handles the first offer made by the player.
        /// </summary>
        void HandleFirstPlayerOffer()
        {
            TradeAbstract trade = TradeAbstract.Singleton;

            float currentPlayerOffer = trade.GetCurrentPlayerOffer();

            if (currentPlayerOffer > upperLimitBargain)
            {
                // Trader refuses and doesn't want to bargain with the offered price.
                trade.UpdateCurrentTraderOffer(0);
            }
            else
            {
                if(currentPlayerOffer < wished[0])
                {
                    // Trader accepts
                    trade.UpdateCurrentTraderOffer(currentPlayerOffer);
                }
                else
                {
                    // Trader makes first counteroffer.
                    trade.UpdateCurrentTraderOffer(underLimitReal + (wished[0] - underLimitReal) / wishedAndOfferedRatio[0]);
                }
            }
        }

        /// <summary>
        /// Handles all offers made by the player except the first one.
        /// </summary>
        void HandleLaterPlayerOffers()
        {
            TradeAbstract trade = TradeAbstract.Singleton;

            float currentPlayerOffer = trade.GetCurrentPlayerOffer();
            float currentTraderOffer = trade.GetCurrentTraderOffer();

            if (!continueTrade)
            {
                trade.Abort();
                return;
            }

            if (currentPlayerOffer <= lastItem(wished, 1))
            {
                trade.UpdateCurrentTraderOffer(currentPlayerOffer);
            }
            else
            {
                trade.UpdateCurrentTraderOffer((float)(Math.Floor(currentTraderOffer) + (lastItem(wished, 1) - Math.Floor(currentTraderOffer)) / lastItem(wishedAndOfferedRatio)));
            }
        }

        /// <summary>
        /// Updates the irritation and skepticism levels.
        /// </summary>
        bool UpdatePsychology()
        {
            TradeAbstract trade = TradeAbstract.Singleton;

            if (realAndOfferedRatio[realAndOfferedRatio.Count - 1] < 1)
            {
                irritationDelta = irritationDeltaModifier;
            }
            else
            {
                irritationDelta = irritationDeltaModifier * realAndOfferedRatio[realAndOfferedRatio.Count - 1];
            }

            if (trade.RealValue < underLimitReal)
            {
                skepticismDelta = 100;
            }
            else
            {
                skepticismDelta = skepticismDeltaModifier / realAndOfferedRatio[realAndOfferedRatio.Count - 1];
            }

            IrritationTotal += irritationDelta;
            SkepticismTotal += skepticismDelta;

            if (IrritationTotal >= IrritationLimit || SkepticismTotal >= SkepticismLimit)
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
            if (TradeAbstract.Singleton == null)
            {
                CurrentTrader = this;
                Position = gameObject.transform.position;
                InventoryItem.Behaviour = InventoryItem.ItemBehaviour.Move;
                Main_SceneManager.LoadSceneAdditively("TradeScene");
                Player.Singleton.EnterTrading();
            }
        }

        /// <summary>
        /// Gets the last Item of a List<float>.
        /// </summary>
        float lastItem(List<float> list, int beforeLast = 0)
        {
            return list[list.Count - (1 + beforeLast)];
        }
    }
}