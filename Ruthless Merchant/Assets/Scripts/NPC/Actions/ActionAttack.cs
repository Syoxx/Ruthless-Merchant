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
        private float attackAnimationDuration = 0;

        /// <summary>
        /// Action which tries to attack an given enemy 
        /// </summary>
        /// <param name="priority">Priority of the action</param>
        public ActionAttack(ActionPriority priority) : base(priority)
        {
        }

        /// <summary>
        /// Action which tries to attack an given enemy 
        /// </summary>
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

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public override void EndAction(bool executeEnd = true)
        {
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            AbortAnimations();
            parent.StartCoroutine(AbortAnimationsDelayed());
            base.EndAction();
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            parentFighter = parent as Fighter;
            animator = parent.gameObject.GetComponent<Animator>();
            base.StartAction(parent, other);
            if (other != null)
                character = other.GetComponent<Character>();

            attackAnimationDuration = GetAnimationDuration("meeleAttack");
            if (attackAnimationDuration <= 0)
                attackAnimationDuration = GetAnimationDuration("monsterattack") * Random.Range(0.85f, 0.95f);
            else
                attackAnimationDuration *= Random.Range(1.0f, 1.05f);

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsInFight", true);
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
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
                        if (parent.Attack(character, attackAnimationDuration))
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

        /// <summary>
        /// Starts attack action
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateAttack()
        {
            animator.SetBool("IsInFight", true);
            animator.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(0);
            animator.SetBool("IsAttacking", false);
        }

        /// <summary>
        /// Aborts attack action
        /// </summary>
        private void AbortAnimations()
        {
            animator.SetBool("IsInFight", false);
            animator.SetBool("IsAttacking", false);
        }

        /// <summary>
        /// Aborts attack action delayed
        /// </summary>
        /// <returns></returns>
        private IEnumerator AbortAnimationsDelayed()
        {
            yield return new WaitForSeconds(0);
            animator.SetBool("IsInFight", false);
            animator.SetBool("IsAttacking", false);
        }

        /// <summary>
        /// Gets the animation duration from a given clip name
        /// </summary>
        /// <param name="clipName">Clip name</param>
        /// <returns>Returns the clip duration</returns>
        private float GetAnimationDuration(string clipName)
        {
            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            for (int i = 0; i < controller.animationClips.Length; i++)
            {
                if (controller.animationClips[i].name == clipName)
                    return controller.animationClips[i].length;
            }

            return 0.0f;
        }
    }
}
