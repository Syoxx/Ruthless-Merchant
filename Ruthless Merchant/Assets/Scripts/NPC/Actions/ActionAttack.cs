using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionAttack : ActionNPC
    {
        private Character character;
        private Fighter fighter;

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
            if (character != null && character.HealthSystem != null && character.HealthSystem.Health > 0)
            {
                float distance = Vector3.Distance(character.transform.position, parent.transform.position);
                if (distance <= fighter.AttackDistance)
                {
                    parent.Reacting = true;
                    parent.Attack(character.HealthSystem);
                }
            }
            else
            {
                parent.CurrentAction = null;
            }
        }
    }
}
