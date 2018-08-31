//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionAttack : ActionNPC
    {
        private Character character;
        private Fighter parentFighter;
        private Animator animator;

        public ActionAttack(ActionPriority priority) : base(priority)
        {
        }

        public ActionAttack() : base(ActionPriority.High)
        {

        }

        public Character Character
        {
            get
            {
                return character;
            }
        }

        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            AbortAnimations();
            parent.StartCoroutine(AbortAnimationsDelayed);
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            parentFighter = parent as Fighter;
            animator = parent.gameObject.GetComponent<Animator>();
            base.StartAction(parent, other);
            if (other != null)
                character = other.GetComponent<Character>();

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsInFight", true);
        }

        public override void Update(float deltaTime)
        {
            if (!animator.GetBool("IsInFight"))
                animator.SetBool("IsInFight", true);

            if (character != null && character.HealthSystem != null && character.HealthSystem.Health > 0)
            {
                if (character != parent.CurrentReactTarget)
                {
                    parent.SetCurrentAction(new ActionIdle(), null, true);    
                    return;                 
                }
                else
                {
                    parent.RotateToNextTarget(character.transform.position, false);
                    float distance = Vector3.Distance(character.transform.position, parent.transform.position);
                    if (distance <= parentFighter.AttackDistance)
                    { 
                        parent.Reacting = true;
                        if (parent.Attack(character))
                        {
                            if (character.HealthSystem.Health <= 0)
                            {
                                parent.ResetTarget();
                            }
                            parent.StartCoroutine(AnimateAttack());
                        }
                    }
                }
            }
            else
            {
                parent.SetCurrentAction(new ActionIdle(), null, true);
                parent.ResetTarget();
            }
        }

        private IEnumerator AnimateAttack()
        {
            animator.SetBool("IsInFight", true);
            animator.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(0);
            animator.SetBool("IsAttacking", false);
        }

        private void AbortAnimations()
        {
            animator.SetBool("IsInFight", false);
            animator.SetBool("IsAttacking", false);
        }

        private IEnumerator AbortAnimationsDelayed()
        {
            yield return new WaitForSeconds(0);
            animator.SetBool("IsInFight", false);
            animator.SetBool("IsAttacking", false);
        }
    }
}
