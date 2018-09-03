using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class KillGoal : Goal {

        //[SerializeField]
        //private int EnemyID;
        //[SerializeField]
        //private bool inProgress;

        Hero hero;
        public int RequiredEliminations;
        private GameObject[] Targets;
        private QuestButton questButton;

        public KillGoal(/*int enemyID, */string description, bool completed, int currentAmount, int requiredAmount, List<Transform> waypoints)
        {
            //this.EnemyID = enemyID;
            this.Description = description;
            this.Completed = completed;
            this.CurrentAmount = currentAmount;
            this.RequiredAmount = requiredAmount;
        }

        public override void Initialize()
        {
            base.Initialize();

        }

        private void Start()
        {
            RequiredEliminations = RequiredAmount;
            hero = GetComponent<Hero>();
        }

        private void Update()
        {
            //cheat for testing
            if (Input.GetKeyDown(KeyCode.F10))
                EnemyKilled();

        }

        void EnemyKilled()
        {
            CurrentAmount++;
            Evaluate();
        }

        public void SetTargetList(GameObject button)
        {
            Completed = false;
            questButton = button.GetComponentInChildren<QuestButton>();

            questButton.InProgressButton();
            InProgress = true;
        }

        public void CalcNextWayPoint()
        {
            GameObject target = null;
            float distance = float.MaxValue;

            // detect monsters
            GameObject[] Monsters = GameObject.FindGameObjectsWithTag("NPC");

            for (int i = 0; i < Monsters.Length; i++)
            {
                if (Monsters[i].gameObject.name == "NPC_Monster(Clone)")
                {
                    float newDistance = Vector3.Distance(gameObject.transform.position, Monsters[i].transform.position);

                    if (newDistance < distance)
                    {
                        target = Monsters[i];
                        distance = newDistance;
                    }
                }
            }

            if (target != null && hero != null)
            {
                if (!(hero.CurrentAction is ActionAttack))
                {
                    hero.SetCurrentAction(new ActionHunt(ActionNPC.ActionPriority.Medium), target);
                }
            }
        }
    }

}