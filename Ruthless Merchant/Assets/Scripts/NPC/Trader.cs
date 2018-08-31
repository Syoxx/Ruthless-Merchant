// Alberto Lladó

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("TRADER VARIABLES")]

        public bool startTradeImmediately;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public CaptureTrigger AsignedOutpost;

        [SerializeField]
        Transform directionPointer;

        #region Influence Properties

        [Header("Reputation Variables")]

        [SerializeField, Range(0, 1), Tooltip("Ruf: Individuell")]
        float influenceIndividual;

        #if UNITY_EDITOR
        [ReadOnly, Tooltip("Ruf Individuell +")]
        #endif
        [SerializeField]
        float influenceIndividualDelta;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("Situation: Krieg")]
        float influenceWar;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField, Tooltip("Situation: Nachbarn")]
        float influenceNeighbours;

        #if UNITY_EDITOR
        [ReadOnly, Tooltip("Ruf Fraktion")]
        #endif
        [SerializeField]
        float influenceFaction;

        #if UNITY_EDITOR
        [ReadOnly, Tooltip("Ruf Fraktion +")]
        #endif
        [SerializeField]
        float influenceFactionDelta;

        [SerializeField, Range(0, 0.001f), Tooltip("Grundwert Ruf Fraktion je 1 Realwert")]
        float valueFactionPerRealValue = 0.00005f;

        [SerializeField, Range(0, 1f), Tooltip("Faktor Bonus durch individuellen Ruf")]
        float influenceIndividualBonus = 0.1f;

        [SerializeField, Range(0, 0.1f), Tooltip("Grundwert Ruf Individuum je Handel")]
        float valueIndividualPerTrade = 0.02f;

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

        [SerializeField, Range(0, 100)]
        float irritationStartLimit = 80;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField]
        float irritationDelta;

        [SerializeField, Range(0, 100), Tooltip("Genervt plus Grundwert")]
        float irritationDeltaModifier = 20;

        [SerializeField, Range(0, 2), Tooltip("Abbau pro Sekunde (Skepsis & Genervtheit)")]
        float psychoCooldown = 1;

        #endregion

        float timer = 0;
        float timerLimit = 1;

        float startTimer = 0;
        bool fading;

        bool moving;
        Vector3 targetPosition;

        static Image fadeImage;

        #region MonoBehaviour cycle

        private void Awake()
        {
            if (fadeImage == null)
                fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        }

        public override void Start()
        {
            if(Tutorial.Singleton != null && startTradeImmediately)
                Interact(null);
        }

        public override void Update()
        {
            UpdatePsychoCoolff();

            if (fading)
            {
                startTimer += Time.deltaTime;

                if (startTimer > 1)
                {
                    fadeImage.color -= new Color(0, 0, 0, Time.deltaTime);

                    if(fadeImage.color.a <= 0)
                    {
                        fading = false;
                        startTimer = 0;
                        fadeImage.raycastTarget = false;
                        Cursor.visible = true;
                    }
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
                if (faction == Faction.Freidenker)
                {
                    influenceFaction = Player.Singleton.Reputation.FreidenkerStanding;
                    influenceWar = CaptureTrigger.OwnerStatistics[Faction.Freidenker] / 6;

                    if(AsignedOutpost != null)
                        influenceNeighbours = (AsignedOutpost.OutpostsToFreidenker.Length + AsignedOutpost.OutpostsToImperialist.Length - 1) / AsignedOutpost.OutpostsToFreidenker.Length;
                }

                else if (faction == Faction.Imperialisten)
                {
                    influenceFaction = Player.Singleton.Reputation.ImperalistenStanding;
                    influenceWar = CaptureTrigger.OwnerStatistics[Faction.Imperialisten] / 6;

                    if (AsignedOutpost != null)
                        influenceNeighbours = (AsignedOutpost.OutpostsToFreidenker.Length + AsignedOutpost.OutpostsToImperialist.Length - 1) / AsignedOutpost.OutpostsToImperialist.Length;
                }

                else
                {
                    Debug.LogError("Trader has no valid Faction!");
                    return;
                }

                if (AsignedOutpost != null)
                    influenceNeighbours = CorrectAndConvertToPercent(influenceNeighbours);

                influenceWar = CorrectAndConvertToPercent(influenceWar);

                upperLimitPercentTotal = upperLimitPerCent + ((upperLimitPerCent * influenceFaction) + (upperLimitPerCent * influenceIndividual) + (upperLimitPerCent * influenceWar) + (upperLimitPerCent * influenceNeighbours)) / 4;
                underLimitPercentTotal = underLimitPerCent + (-(underLimitPerCent * influenceFaction) - (underLimitPerCent * influenceIndividual) + (underLimitPerCent * influenceWar) + (underLimitPerCent * influenceNeighbours)) / 4;

                float realPrice = trade.RealValue;

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

        float CorrectAndConvertToPercent(float data)
        {
            if (data > 100)
                data = 100;

            else if (data < 0)
                data = 0;

            return data / 100;
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
                if (WantsToStartTrading() && (!Tutorial.Singleton.TradeIsDone || !startTradeImmediately))
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
                        fading = true;
                        fadeImage.color = new Color(0, 0, 0, 1);
                        fadeImage.raycastTarget = true;

                        Vector3 direction = (directionPointer.position - transform.position).normalized * 0.66f;

                        transform.localPosition += direction;
                        player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                        transform.localPosition -= direction;

                        player.transform.LookAt(transform);
                        player.transform.eulerAngles = new Vector3(0, Player.Singleton.transform.eulerAngles.y, 0);
                        player.PlayerLookAngle = Player.Singleton.transform.localRotation;

                        InventoryItem.Behaviour = InventoryItem.ItemBehaviour.Move;
                        Main_SceneManager.LoadSceneAdditively("TradeScene");
                        player.EnterTrading();
                        Tutorial.Monolog(1);

                        player.RestrictBookUsage = true;

                        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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

        public void IncreaseReputation()
        {
            TradeAbstract trade = TradeAbstract.Singleton;

            influenceIndividualDelta = valueIndividualPerTrade - valueIndividualPerTrade * ((IrritationTotal / IrritationLimit + SkepticismTotal / SkepticismLimit) / 2);
            influenceIndividual += influenceIndividualDelta;

            float multiplier = trade.RealValue / trade.GetCurrentTraderOffer();

            if (multiplier <= 1)
                multiplier = 1;

            influenceFactionDelta = (trade.RealValue * valueFactionPerRealValue) * multiplier + influenceIndividualDelta * influenceIndividualBonus;

            if (faction == Faction.Freidenker)
                Player.Singleton.Reputation.FreidenkerStanding += influenceFactionDelta;

            else if (faction == Faction.Imperialisten)
                Player.Singleton.Reputation.ImperalistenStanding += influenceFactionDelta;
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