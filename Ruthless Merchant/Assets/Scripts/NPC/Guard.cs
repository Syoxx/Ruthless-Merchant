using UnityEngine;

namespace RuthlessMerchant
{
    public class Guard : Warrior
    {
        [SerializeField, Tooltip("Outpost which should be protected")]
        private CaptureTrigger outpost;

        [SerializeField, Tooltip("Indicates if the guard can die or not")]
        private bool hasGodMode = true;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            if (faction != Faction.TutorialGuard)
            {
                if (CurrentAction is ActionIdle)
                    SetCurrentAction(new ActionMove(), outpost.Target.gameObject, true, true);
            }
            //Godmode
            if(hasGodMode)
                HealthSystem.ChangeHealth(HealthSystem.MaxHealth - HealthSystem.Health, this);

            base.Update();
        }
    }
}
