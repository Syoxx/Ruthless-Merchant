//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CaptureTrigger : MonoBehaviour
    {
        public static Dictionary<Faction, int> OwnerStatistics = new Dictionary<Faction, int>();

        [SerializeField, Tooltip("Owner of outpost on gamestart")]
        protected Faction owner = Faction.None;
        private float captureValue = 0;

        private Hero hero;
        private Dictionary<Faction, int> capturingUnits;
        private List<NPC> capturingUnitsList;

        [SerializeField, Tooltip("Flag material for freemindes")]
        private UnityEngine.Material freidenkerFlagMaterial = null;
        [SerializeField, Tooltip("Flag material for imperialists")]
        private UnityEngine.Material imperialistFlagMaterial = null;

        private SkinnedMeshRenderer flagRenderer;
        [SerializeField, Tooltip("Text of mapmarker to make ownership visable on the map")]
        private TextMeshProUGUI mapMarkerRenderer;

        private float elapsedTime;
        [SerializeField, Tooltip("Indicates the time that is required for a hero to spawn.")]
        private float heroRespawnTime = 30.0f;

        [SerializeField, Tooltip("Indicates whether an army has to split between different lanes")]
        private bool isLaneSplitter = false;

        [SerializeField, Tooltip("Indicates the possible next outpost in the direction to the Freidenker city (first item is the default outpost, all other items are optional items for lane splitting or defending)")]
        protected CaptureTrigger[] outpostsToFreidenker;

        [SerializeField, Tooltip("Indicates the possible next outpost in the direction to the Imperialist city (first item is the default outpost, all other items are optional items for lane splitting or defending)")]
        protected CaptureTrigger[] outpostsToImperialist;

        [SerializeField, Tooltip("Hero prefab for imperialist faction that should be spawned at game start")]
        private GameObject imperialstHeroPrefab = null;
        [SerializeField, Tooltip("Hero prefab for freeminds faction that should be spawned at game start")]
        private GameObject freidenkerHeroPrefab = null;

        [SerializeField, Tooltip("Assign a Trader that was placed in the outpost")]
        private Trader assignedTrader = null;

        [SerializeField, Tooltip("Add all Prefabs of weapons, shields and potions here!")]
        private GameObject[] PrefabsUpgradeItems = null;

        private Dictionary<string, ItemContainer> availableItems = new Dictionary<string, ItemContainer>();

        private Transform target;
        private bool isHeroAway = false;
        private bool isCity = false;

        public event EventHandler OnHeroRemoved;
        public float CaptureValue
        {
            get
            {
                return captureValue;
            }
            set
            {
                captureValue = value;
                if (captureValue < -100)
                    captureValue = -100;
                else if (captureValue > 100)
                    captureValue = 100;
            }
        }

        public CaptureTrigger[] OutpostsToFreidenker
        {
            get
            {
                return outpostsToFreidenker;
            }
        }

        public CaptureTrigger[] OutpostsToImperialist
        {
            get
            {
                return outpostsToImperialist;
            }
        }

        /// <summary>
        /// Get or set the hero of the capture point and sets the owners faction to the faction of the hero
        /// </summary>
        public Hero Hero
        {
            get
            {

                return hero;

            }

            set
            {
                hero = value;
                if (hero != null)
                {
                    hero.Outpost = this;
                    hero.AddNewWaypoint(new Waypoint(Target, true, 0), true);
                    hero.SetCurrentAction(new ActionMove(ActionNPC.ActionPriority.Medium), null, true);
                }
            }
        }

        /// <summary>
        /// Sets the owner faction. If the new faction doesn't match the heros faction, the hero is set to null.
        /// </summary>
        public Faction Owner
        {
            get
            {
                return owner;
            }

            set
            {
                if(value != owner && !isCity)
                {
                    //Decrease old
                    if (OwnerStatistics.ContainsKey(owner))
                        OwnerStatistics[owner]--;

                    //Increase new
                    if (OwnerStatistics.ContainsKey(value))
                        OwnerStatistics[value]++;
                    else
                        OwnerStatistics.Add(value, 1);
                }

                owner = value;
                if (hero != null && hero.Faction != owner)
                    hero = null;

                if(flagRenderer != null)
                    flagRenderer.material.color = GetFactionColor();

                if (mapMarkerRenderer != null)
                    mapMarkerRenderer.color = GetFactionColor();
            }
        }

        public bool IsLaneSplitter
        {
            get
            {
                return isLaneSplitter;
            }
        }

        public bool IsHeroAway
        {
            get
            {
                return isHeroAway;
            }
        }

        public bool IsUnderAttack
        {
            get
            {
                if (capturingUnits != null)
                    return capturingUnits.Count > 1;
                else
                    return false;
            }
        }

        public Transform Target
        {
            get
            {
                if(target == null)
                {
                    target = transform.Find("Target");
                    if (target == null)
                        Debug.Log("Target NULL!");
                }
                return target;
            }
        }

        public Dictionary<string, ItemContainer> AvailableItems
        {
            get
            {
                return availableItems;
            }
        }

        void Awake()
        {
            if(assignedTrader != null)
                assignedTrader.AsignedOutpost = this;
        }

        // Use this for initialization
        protected virtual void Start()
        {
            isCity = name.Contains("City");
            capturingUnits = new Dictionary<Faction, int>();
            capturingUnitsList = new List<NPC>();
            if (owner == Faction.Freidenker)
                captureValue = -100;
            else if (owner == Faction.Imperialisten)
                captureValue = 100;

            target = transform.Find("Target");
            if (outpostsToFreidenker != null && outpostsToFreidenker.Length > 0 && !(outpostsToFreidenker[0] is CityTrigger))
            {
                outpostsToFreidenker[0].OnHeroRemoved += CaptureTriggerF_OnHeroRemoved;
                if (isLaneSplitter && outpostsToFreidenker.Length > 1 && outpostsToFreidenker[1] != null)
                    OutpostsToFreidenker[1].OnHeroRemoved += CaptureTriggerF_OnHeroRemoved;
            }

            if (outpostsToImperialist != null && outpostsToImperialist.Length > 0 && !(outpostsToImperialist[0] is CityTrigger))
            {
                outpostsToImperialist[0].OnHeroRemoved += CaptureTriggerI_OnHeroRemoved;
                if (isLaneSplitter && outpostsToImperialist.Length > 1 && outpostsToImperialist[1] != null)
                    outpostsToImperialist[1].OnHeroRemoved += CaptureTriggerI_OnHeroRemoved;
            }

            //Color Flag
            Transform parentFlag = transform.Find("Flag");
            if (parentFlag != null)
            {
                Transform obj = parentFlag.Find("Flag");

                if (obj != null && obj.CompareTag("Flag"))
                {
                    flagRenderer = obj.GetComponent<SkinnedMeshRenderer>();
                    ChangeFlagMaterial();
                }
            }

            if (mapMarkerRenderer != null)
                mapMarkerRenderer.color = GetFactionColor();

            Trade.ItemsSold += Trade_ItemsSold;

            //Spawn Hero
            SpawnHero();

            if (!isCity)
            {
                if (OwnerStatistics.ContainsKey(Owner))
                    OwnerStatistics[Owner]++;
                else
                    OwnerStatistics.Add(Owner, 1);
            }
        }

        private void Trade_ItemsSold(object sender, Trade.TradeArgs e)
        {
            Debug.Log("ItemsSlod event triggered");
            if (e.Trader == assignedTrader)
            {
                for (int i = 0; i < e.Items.Count; i++)
                {
                    InventorySlot slot = e.Items[i].Slot;

                    //Get item count
                    int count = 0;
                    if (!int.TryParse(e.Items[i].ItemQuantity.text.Replace("x", ""), out count))
                        count = 1;

                    if (slot.Item != null && slot.Item is Potion)
                    {
                        if (!availableItems.ContainsKey(slot.ItemInfo.ItemName))
                            availableItems.Add(slot.ItemInfo.ItemName, new ItemContainer(slot.Item, count));
                        else
                            availableItems[slot.ItemInfo.ItemName].Count += count;
                    }
                    else
                    {
                        if (slot.Item != null && slot.Item is Weapon)
                        {
                            if (!availableItems.ContainsKey(slot.ItemInfo.ItemName))
                                availableItems.Add(slot.ItemInfo.ItemName, new ItemContainer(slot.Item, count));
                            else
                                availableItems[slot.ItemInfo.ItemName].Count += count;
                        }
                        else
                        {
                            for (int j = 0; j < PrefabsUpgradeItems.Length; j++)
                            {
                                Weapon weapon = PrefabsUpgradeItems[j].GetComponent<Weapon>();
                                if (weapon.ItemInfo.ItemName == slot.ItemInfo.ItemName)
                                {
                                    if (!availableItems.ContainsKey(weapon.ItemInfo.ItemName))
                                        availableItems.Add(weapon.ItemInfo.ItemName, new ItemContainer(weapon, count));
                                    else
                                        availableItems[weapon.ItemInfo.ItemName].Count += count;
                                }
                            }
                        }
                    }
                }

                Debug.Log("New Items received");
            }
        }

        private void ChangeFlagMaterial()
        {
            if (flagRenderer != null)
            {
                if (owner == Faction.Freidenker)
                    flagRenderer.material = freidenkerFlagMaterial;
                else if (owner == Faction.Imperialisten)
                    flagRenderer.material = imperialistFlagMaterial;
                else
                    flagRenderer.material = null;
            }
        }

        private void SpawnHero()
        {
            if (hero == null)
            {
                if (owner == Faction.Freidenker)
                {
                    GameObject gobj = Instantiate(freidenkerHeroPrefab, Target.position, Quaternion.identity, transform);
                    Hero = gobj.GetComponent<Hero>();
                }
                else if (owner == Faction.Imperialisten)
                {
                    GameObject gobj = Instantiate(imperialstHeroPrefab, Target.position, Quaternion.identity, transform);
                    Hero = gobj.GetComponent<Hero>();
                }
            }
        }

        /// <summary>
        /// Outpost in direction to Freidenker lost it's hero
        /// </summary>
        /// <param name="sender">Outpost</param>
        /// <param name="e"></param>
        protected virtual void CaptureTriggerF_OnHeroRemoved(object sender, EventArgs e)
        {
            if (sender != null && sender is CaptureTrigger)
            {
                CaptureTrigger outpost = sender as CaptureTrigger;
                if (outpost.Owner == owner)
                {
                    if (owner == Faction.Imperialisten)
                    {
                        if (Hero != null && !IsHeroAway && outpost.Hero == null)
                        {
                            outpost.Hero = hero;
                            Hero = null;

                            if (OnHeroRemoved != null)
                                OnHeroRemoved.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Outpost in direction to Imperialists lost it's hero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CaptureTriggerI_OnHeroRemoved(object sender, EventArgs e)
        {
            if (sender != null && sender is CaptureTrigger)
            {
                CaptureTrigger outpost = sender as CaptureTrigger;
                if (outpost.Owner == owner)
                {
                    if (owner == Faction.Freidenker)
                    {
                        if (Hero != null && outpost.hero == null)
                        {
                            outpost.Hero = hero;
                            Hero = null;

                            if (OnHeroRemoved != null)
                                OnHeroRemoved.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Capture();
            HeroAllocTimer();
        }

        private void HeroAllocTimer()
        {
            if (hero == null)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= heroRespawnTime)
                {
                    if (OnHeroRemoved != null)
                        OnHeroRemoved.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                elapsedTime = 0.0f;
            }
        }

        private void Capture()
        {
            if ((hero == null || isHeroAway) && capturingUnits != null && !isCity)
            {
                //int freidenkerCount = capturingUnits.ContainsKey(Faction.Freidenker) ? capturingUnits[Faction.Freidenker] : 0;
                //int imperialistenCount = capturingUnits.ContainsKey(Faction.Imperialisten) ? capturingUnits[Faction.Imperialisten] : 0;
                int monsterCount = capturingUnits.ContainsKey(Faction.Monster) ? capturingUnits[Faction.Monster] : 0;

                float capValue = 0;
                for (int i = 0; i < capturingUnitsList.Count; i++)
                {
                    if (capturingUnitsList[i] != null)
                    {
                        if (capturingUnitsList[i].Faction == Faction.Freidenker)
                        {
                            capValue -= capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                        }
                        else if (capturingUnitsList[i].Faction == Faction.Imperialisten)
                        {
                            capValue += capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                        }
                        else if (capturingUnitsList[i].Faction == Faction.Monster)
                        {
                            if (captureValue < 0)
                                capValue += capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                            else if (captureValue > 0)
                                capValue -= capturingUnitsList[i].CapValuePerSecond * Time.deltaTime;
                        }
                    }
                }

                if (monsterCount <= 0 || System.Math.Abs(CaptureValue + capValue) < System.Math.Abs(CaptureValue))
                {
                    if (hero != null)
                    {
                        if (owner == Faction.Freidenker)
                        {
                            if (CaptureValue + capValue > -50)
                                CaptureValue = -50;

                        }
                        else if (owner == Faction.Imperialisten)
                        {
                            if (CaptureValue + capValue < 50)
                                CaptureValue = 50;
                        }
                    }
                    else
                    {
                        CaptureValue += capValue;
                    }
                }

                if (captureValue <= -50)
                    Owner = Faction.Freidenker;
                else if (captureValue >= 50)
                    Owner = Faction.Imperialisten;
                else
                    Owner = Faction.Neutral;
            }
        }
        
        private Color GetFactionColor()
        {
            if (owner == Faction.Freidenker)
                return new Color(1f, 215.0f / 255.0f, 0);
            else if (owner == Faction.Imperialisten)
                return new Color(175.0f / 255.0f, 33.0f / 255.0f, 32.0f / 255.0f);
            else
                return Color.white;
        }

        public Transform GetClosestAttacker(NPC npc)
        {
            float minDistance = float.MaxValue;
            Transform closestEnemey = null;
            for (int i = 0; i < capturingUnitsList.Count; i++)
            {
                if (capturingUnitsList[i] != null)
                {
                    if (capturingUnitsList[i].Faction != npc.Faction)
                    {
                        float tempDist = Vector3.Distance(capturingUnitsList[i].transform.position, transform.position);
                        if (tempDist < minDistance)
                        {
                            minDistance = tempDist;
                            closestEnemey = capturingUnitsList[i].transform;
                        }
                    }
                }
            }

            return transform;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                if (other.transform.GetComponent<Hero>() != null)
                {
                    if (hero != null && other.transform == hero.transform)
                    {
                        isHeroAway = false;
                    }
                }
                else if (capturingUnits != null)
                {
                    NPC npc = other.GetComponent<NPC>();
                    if (!capturingUnits.ContainsKey(npc.Faction))
                        capturingUnits.Add(npc.Faction, 0);

                    capturingUnitsList.Add(npc);
                    capturingUnits[npc.Faction]++;
                }
            }
            else if (other.CompareTag("Player") && this.gameObject.name == "OutpostF3")
            {
                if(Achievements.Singleton.switchIndex == 1)
                {
                    Achievements.AddToCounter();
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                if (hero == null || other.transform == hero.transform)
                {
                    isHeroAway = true;
                }

                if (hero == null || other.transform != hero.transform)
                {
                    if (capturingUnits != null)
                    {
                        NPC npc = other.GetComponent<NPC>();
                        if (capturingUnits.ContainsKey(npc.Faction))
                        {
                            capturingUnits[npc.Faction]--;
                            if (capturingUnits[npc.Faction] <= 0)
                                capturingUnits.Remove(npc.Faction);

                            capturingUnitsList.Remove(npc);
                        }
                    }
                }
            }
        }
    }
}
