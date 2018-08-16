//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionAttack : ActionNPC
    {
        private Character character;
        private Fighter parentFighter;

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
            parent.GetComponent<Animator>().SetBool("IsAttacking", false);
            parent.GetComponent<Animator>().SetBool("IsInFight", false);
            if (executeEnd)
            {
                parent.Reacting = false;
            }
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            parent.GetComponent<Animator>().SetBool("IsAttacking", true);
            parentFighter = parent as Fighter;
            base.StartAction(parent, other);
            if (other != null)
                character = other.GetComponent<Character>();
        }

        public override void Update(float deltaTime)
        {
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
                        parent.Attack(character);

                        if (parent.Attack(character))
                            parent.GetComponent<Animator>().SetBool("IsInFight", true);
                    }
                }
            }
            else
            {
                parent.SetCurrentAction(new ActionIdle(), null, true);
                parent.ResetTarget();
            }
        }
    }
}
