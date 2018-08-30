﻿// Alberto Lladó

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
        float upperLimitBargainPerCent = 10000;

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

        [SerializeField]
        GameObject moodIconCanvas;

        [SerializeField]
        GameObject moodIconPrefab;

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
        float skepticismDeltaModifier = 15;

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
        float irritationDeltaModifier = 20;

        [SerializeField, Range(0, 2), Tooltip("Abbau pro Sekunde (Skepsis / Genervtheit)")]
        float psychoCooldown = 1;

        #endregion

        float timer = 0;
        float timerLimit = 1;

        public bool startTradeImmediately;

        bool moving = false;
        Vector3 targetPosition;

        #region MonoBehaviour cycle

        public override void Start()
        {
            if(Tutorial.Singleton != null && startTradeImmediately)
                Interact(null);
        }

        public override void Update()
        {
            UpdatePsychoCoolff();

            if (moving)
            {
                float remainingDistance = Player.Singleton.NavMeshAgent.remainingDistance;

                Player player = Player.Singleton;

                if (remainingDistance < 0.005f)
                {
                    moving = false;

                    player.transform.position = targetPosition;
                    player.transform.LookAt(transform);
                    player.transform.eulerAngles = new Vector3(0, Player.Singleton.transform.eulerAngles.y, 0);
                    player.PlayerLookAngle = Player.Singleton.transform.localRotation;

                    player.NavMeshAgent.enabled = false;

                    InventoryItem.Behaviour = InventoryItem.ItemBehaviour.Move;
                    Main_SceneManager.LoadSceneAdditively("TradeScene");
                    player.EnterTrading();
                    Tutorial.Monolog(1);
                }
                else
                {
                    player.transform.LookAt(transform);
                    player.transform.eulerAngles = new Vector3(0, Player.Singleton.transform.eulerAngles.y, 0);
                }
            }
        }

        #endregion

        void UpdatePsychoCoolff()
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

        /// <summary>
        /// Sets the initial trading variables.
        /// </summary>
        public void Initialize(TradeAbstract trade)
        {
            wished                = new List<float>();
            realAndOfferedRatio   = new List<float>();
            wishedAndOfferedRatio = new List<float>();

            if (WantsToStartTrading())
            {
                float realPrice = trade.RealValue;

                upperLimitPercentTotal = upperLimitPerCent + ((upperLimitPerCent * influenceFraction) + (upperLimitPerCent * influenceIndividual) + (upperLimitPerCent * influenceWar) + (upperLimitPerCent * influenceNeighbours)) / 4;
                underLimitPercentTotal = underLimitPerCent + (-(underLimitPerCent * influenceFraction) - (underLimitPerCent * influenceIndividual) + (underLimitPerCent * influenceWar) + (underLimitPerCent * influenceNeighbours)) / 4;

                upperLimitReal = (float)Math.Ceiling(realPrice * (1 + upperLimitPercentTotal));
                upperLimitBargain = (float)Math.Ceiling(upperLimitReal * (1 + upperLimitBargainPerCent));
                underLimitReal = (float)Math.Floor(realPrice * (1 - underLimitPercentTotal));
                wished.Add((float)Math.Floor((upperLimitReal + underLimitReal) / 2));
            }
            else
            {
                trade.Abort();
                Debug.Log("Trader does not want to start Trading!");
            }
        }

        public bool WantsToStartTrading()
        {
            if (IrritationTotal >= irritationStartLimit || SkepticismTotal >= skepticismStartLimit)
                return false;

            return true;
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
                if (trade.GetCurrentTraderOffer() == -1)
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
            Debug.LogWarning("HandleFirstPlayerOffer");

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

                if(currentPlayerOffer <= TradeAbstract.Singleton.RealValue)
                {
                    Tutorial.Monolog(4);
                }
                else
                {
                    Tutorial.Monolog(3);
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
                Debug.Log("Trader does not want to continue Trade!");
                trade.Abort();
                return;
            }

            if (currentPlayerOffer <= lastItem(wished, 1))
            {
                trade.UpdateCurrentTraderOffer(currentPlayerOffer);
            }
            else
            {
                float nextTraderOffer = (float)(Math.Floor(currentTraderOffer) + (lastItem(wished, 1) - Math.Floor(currentTraderOffer)) / lastItem(wishedAndOfferedRatio));

                Debug.Log("nextTraderOffer: " + nextTraderOffer + " currentPlayerOffer: " + currentPlayerOffer);

                if (nextTraderOffer < currentPlayerOffer)
                    trade.UpdateCurrentTraderOffer(nextTraderOffer);
                else
                    trade.UpdateCurrentTraderOffer(currentPlayerOffer);
            }
        }

        /// <summary>
        /// Updates the irritation and skepticism levels.
        /// </summary>
        bool UpdatePsychology()
        {
            TradeAbstract trade = TradeAbstract.Singleton;

            if (trade.GetCurrentPlayerOffer() < underLimitReal)
            {
                skepticismDelta = 100;
            }
            else
            {
                skepticismDelta = skepticismDeltaModifier / realAndOfferedRatio[realAndOfferedRatio.Count - 1];
            }

            irritationDelta = realAndOfferedRatio[realAndOfferedRatio.Count - 1] * irritationDeltaModifier;

            if (irritationDelta < irritationDeltaModifier)
            {
                irritationDelta = irritationDeltaModifier;
            }

            IrritationTotal += irritationDelta;
            SkepticismTotal += skepticismDelta;

            SpawnMoodIcon();

            if (IrritationTotal >= IrritationLimit)
            {
                IrritationTotal = IrritationLimit;
                trade.Abort();
                Debug.Log("Trader has surpassed his psycho limits.");
                Tutorial.Monolog(7);
                return false;
            }

            if (SkepticismTotal >= SkepticismLimit)
            {
                SkepticismTotal = SkepticismLimit;
                trade.Abort();
                Debug.Log("Trader has surpassed his psycho limits.");
                Tutorial.Monolog(8);
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
                if (WantsToStartTrading() && (Tutorial.Singleton == null || !Tutorial.Singleton.isTutorial || !Tutorial.Singleton.TradeIsDone))
                {
                    CurrentTrader = this;

                    Player player = Player.Singleton;

                    if (startTradeImmediately && !Tutorial.Singleton.TradeIsDone)
                    {
                        InventoryItem.Behaviour = InventoryItem.ItemBehaviour.Move;
                        Main_SceneManager.LoadSceneAdditively("TradeScene");
                        player.EnterTrading();
                        Tutorial.Monolog(1);
                    }
                    else
                    {
                        transform.localPosition += new Vector3(0, 0, 0.4f);
                        targetPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                        transform.localPosition -= new Vector3(0, 0, 0.4f);

                        Vector3 prevPosition = player.transform.position;
                        Quaternion prevRotation = player.transform.rotation;

                        player.transform.position = targetPosition;
                        player.transform.LookAt(transform);

                        player.transform.rotation = prevRotation;
                        player.transform.position = prevPosition;

                        player.NavMeshAgent.enabled = true;
                        player.NavMeshAgent.SetDestination(targetPosition);

                        player.RestrictBookUsage = true;
                        Player.RestrictCamera = true;

                        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                        moving = true;
                    }
                }
                else
                {
                    SpawnMoodIcon();
                    Debug.Log("Trader doesn't want to trade with you.");
                }
            }
        }

        public void SpawnMoodIcon()
        {
            TraderMoodIcon newTraderMoodIcon = Instantiate(moodIconPrefab, moodIconCanvas.transform).GetComponentInChildren<TraderMoodIcon>();
            newTraderMoodIcon.UpdateMood(this);
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