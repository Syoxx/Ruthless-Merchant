//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class Civilian : NPC
    {
        [SerializeField]
        [Range(0, 1000)]
        private float fleeDistance = 5.0f;

        public override void Start()
        {
            base.Start();

            HealthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        public override void Update()
        {
            base.Update();
        }

        private void HealthSystem_OnHealthChanged(object sender, DamageAbleObject.HealthArgs e)
        {
            if (e.ChangedValue < 0 && e.Sender != null)
                SetCurrentAction(new ActionFlee(), e.Sender.gameObject);
        }

        public override void React(Character character, bool isThreat)
        {
            if(isThreat)
            {
                float distance = Vector3.Distance(character.transform.position, transform.position);
                if (distance <= fleeDistance)
                {
                    Reacting = true;
                    SetCurrentAction(new ActionFlee(), character.gameObject);
                }
            }
            else
            {
                //TODO: Stare?
            }
        }

        public override void React(Item item)
        {
            //TODO: try pickup item?
        }

        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Civilian");
        }
    }
}