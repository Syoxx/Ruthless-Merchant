using UnityEngine;

namespace RuthlessMerchant
{
    public class Guard : Warrior
    {
        [SerializeField, Tooltip("Outpost which should be protected")]
        private CaptureTrigger outpost;

        // Use this for initialization
        public override void Start()
        {

        }

        // Update is called once per frame
        public override void Update()
        {
            if (CurrentAction is ActionIdle)
                SetCurrentAction(new ActionMove(), outpost.Target.gameObject, true, true);

            //Godmode
            HealthSystem.ChangeHealth(HealthSystem.MaxHealth - HealthSystem.Health, this);
        }
    }
}
