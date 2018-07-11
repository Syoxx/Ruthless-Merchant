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
        private Fighter fighter;

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
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            parent.Waypoints.Clear();
            fighter = parent as Fighter;
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
                    if (distance <= fighter.AttackDistance)
                    {
                        parent.Reacting = true;
                        parent.Attack(character);
                    }
                }
            }
            else
            {
                parent.SetCurrentAction(new ActionIdle(), null, true);
            }
        }
    }
}
