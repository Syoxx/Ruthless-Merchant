//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public abstract class Fighter : NPC
    {
        [Header("NPC Fighter settings")]
        [SerializeField, Tooltip("If the distance to a character is smaller than the hunting distance, the NPC follows the character")]
        [Range(0.0f, 100.0f)]
        protected float huntDistance = 5;

        [SerializeField, Tooltip("If the distance to a character is smaller then the attacking distance, the npc attacks the character")]
        [Range(0.0f, 100.0f)]
        protected float attackDistance = 1.5f;

        public float HuntDistance
        {
            get
            {
                return huntDistance;
            }
        }

        public float AttackDistance
        {
            get
            {
                return attackDistance;
            }
        }

        /// <summary>
        /// Init fighter
        /// </summary>
        public override void Start()
        {
            base.Start();
            HealthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
            HealthSystem.OnDeath += HealthSystem_OnDeath;
        }

        private void HealthSystem_OnDeath(object sender, DamageAbleObject.HealthArgs e)
        {
            ParticleSystem fx = BloodManager.GetFreeFX();
            if (fx != null)
            {
                fx.transform.position = transform.position + (Vector3.up * 0.5f);
                fx.transform.rotation = transform.rotation;
                fx.Play();
            }
        }

        /// <summary>
        /// Update fighter
        /// </summary>
        public override void Update()
        {
            SetCurrentAction(new ActionIdle(ActionNPC.ActionPriority.None), null);
            base.Update();
        }

        /// <summary>
        /// Handles damage on fighter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HealthSystem_OnHealthChanged(object sender, DamageAbleObject.HealthArgs e)
        {
            if (e.ChangedValue < 0 && e.Sender != null && HealthSystem.Health > 0)
            {
                if (CurrentReactTarget == null || currentReactTarget.IsDying || !IsThreat(CurrentReactTarget.gameObject) || Vector3.Distance(CurrentReactTarget.transform.position, transform.position) > Vector3.Distance(e.Sender.transform.position, transform.position))
                {
                    currentReactTarget = e.Sender;
                    reactionState = TargetState.None;
                    reactionState = reactionState.SetFlag(TargetState.InView);
                    reactionState = reactionState.SetFlag(TargetState.IsThreat);
                    SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.Medium), e.Sender.gameObject, true, true);
                }
            }
        }

        /// <summary>
        /// Racts to other character
        /// </summary>
        /// <param name="character"></param>
        /// <param name="isThreat"></param>
        public override void React(Character character, bool isThreat)
        {
            if (isThreat)
            {
                float distance = Vector3.Distance(transform.position, character.transform.position);
                if (distance <= attackDistance)
                {
                    if (CurrentAction == null || !(CurrentAction is ActionAttack))
                        SetCurrentAction(new ActionAttack(), character.gameObject);
                }
                else if(distance < huntDistance)
                {
                    if (CurrentAction == null || !(CurrentAction is ActionHunt))
                        SetCurrentAction(new ActionHunt(), character.gameObject, true, true);
                }
            }
        }

        /// <summary>
        /// Tries to pickup equipment at outposts
        /// </summary>
        /// <param name="outpost"></param>
        protected void TryPickupEquipment(CaptureTrigger outpost)
        {
            foreach (KeyValuePair<string, ItemContainer> itemPair in outpost.AvailableItems)
            {
                if (weapon != null && shield != null && potion != null)
                    break;

                ItemContainer slot = itemPair.Value;
                if (slot.Count > 0)
                {
                    if (slot.Item is Weapon)
                    {
                        bool isShield = itemPair.Key.Contains("shield");
                        if (weapon == null && !isShield)
                        {
                            weapon = (Weapon)slot.Item;
                            slot.Count--;
                            EnableWeapon(weapon.name, transform);
                        }
                        else if (shield == null && isShield)
                        {
                            shield = (Weapon)slot.Item;
                            slot.Count--;
                        }
                    }
                    else if (slot.Item is Potion)
                    {
                        if (potion == null)
                        {
                            potion = (Potion)slot.Item;
                            slot.Count--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Enable pick weapon by weaponname
        /// </summary>
        /// <param name="name"></param>
        /// <param name="transform"></param>
        protected void EnableWeapon(string name, Transform transform = null)
        {
            foreach (Transform child in transform)
            {
                if (child == null)
                    continue;

                if (child.name.Contains("Weapon"))
                    child.gameObject.SetActive(child.name.StartsWith(name));

                EnableWeapon(name, child);
            }
        }

        public override void React(Item item)
        {
            //TODO: Pickup item
            Reacting = true;
        }
    }
}