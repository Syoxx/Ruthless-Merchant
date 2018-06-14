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

        public Character Character
        {
            get
            {
                return character;
            }
        }

        public override void EndAction()
        {
            parent.Reacting = false;
            base.EndAction();
        }

        public override void StartAction(NPC parent, GameObject other)
        {
            fighter = parent as Fighter;
            base.StartAction(parent, other);
            if (other != null)
                character = other.GetComponent<Character>();
        }

        public override void Update(float deltaTime)
        {
            if(character != parent.CurrentReactTarget)
            {
                parent.SetCurrentAction(new ActionIdle(), null);
                return;
            }

            if (character != null && character.HealthSystem != null && character.HealthSystem.Health > 0)
            {
                parent.RotateToNextTarget(character.transform.position, false);
                float distance = Vector3.Distance(character.transform.position, parent.transform.position);
                if (distance <= fighter.AttackDistance)
                {
                    parent.Reacting = true;
                    parent.Attack(character.HealthSystem);
                }
            }
            else
            {
                parent.SetCurrentAction(new ActionIdle(), null);
            }
        }
    }
}
