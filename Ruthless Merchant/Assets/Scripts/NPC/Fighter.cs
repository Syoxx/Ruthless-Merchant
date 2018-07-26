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
        [Range(0, 100)]
        protected float huntDistance = 5;

        [SerializeField, Tooltip("If the distance to a character is smaller then the attacking distance, the npc attacks the character")]
        [Range(1, 100)]
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

        public override void Start()
        {
            base.Start();
            HealthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        public override void Update()
        {
            base.Update();
            SetCurrentAction(new ActionIdle(ActionNPC.ActionPriority.None), null);
        }

        private void HealthSystem_OnHealthChanged(object sender, DamageAbleObject.HealthArgs e)
        {
            if (CurrentAction == null || CurrentAction is ActionIdle)
            {
                if (e.ChangedValue < 0 && e.Sender != null && HealthSystem.Health > 0)
                {
                    SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.Medium), e.Sender.gameObject, false, true);
                }
            }
        }

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
                else
                {
                    if (CurrentAction == null || !(CurrentAction is ActionHunt))
                        SetCurrentAction(new ActionHunt(), character.gameObject);
                }
            }
        }

        public override void React(Item item)
        {
            //TODO: Pickup item
            Reacting = true;
        }
    }
}