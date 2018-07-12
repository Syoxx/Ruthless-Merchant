//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class Hero : Warrior
    {
        private QuestItem Quest;

        [Header("Patrol settings")]
        [SerializeField]
        private bool patrolActive;
        public string[] PossiblePatrolPaths;
        public Waypoint[] PatrolPoints;

        public bool PaartolActive
        {
            get { return patrolActive; }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }


    }
}