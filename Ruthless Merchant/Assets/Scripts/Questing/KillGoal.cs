using System;
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
        GameObject currentTarget;
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
            hero = GetComponent<Hero>();
        }

        private void Update()
        {
            //cheat for testing
            if (Input.GetKeyDown(KeyCode.F10))
                EnemyKilled();
            //if(InProgress)
            //    Debug.Log("current action: " + hero.CurrentAction);

            if (InProgress && hero.CurrentAction is ActionIdle)
            {
                // hero needs to see the monster to hunt properly, maybe use ActionMove
            }

            if (InProgress && currentTarget == null)
            {
                EnemyKilled();
            }
        }

        public void EnemyKilled()
        {
            CurrentAmount++;
            //Evaluate();
            Completed = EvaluateGoal();
        }

        private bool EvaluateGoal()
        {
            if (CurrentAmount < RequiredAmount)
            {
                CalcNextWayPoint();
                return false;
            }
            else
            {
                // reputation reward

                InProgress = false;

                if (questButton)
                {
                    questButton.CompleteButton();
                    questButton.DiscardQuestButton();
                }

                return true;
            }
        }

        public void SetTargetList(GameObject button, int monsterAmount, float reputationGain)
        {
            Completed = false;
            questButton = button.GetComponentInChildren<QuestButton>();

            questButton.InProgressButton();
            InProgress = true;

            RequiredAmount = monsterAmount;
        }

        public void CalcNextWayPoint()
        {
            currentTarget = null;
            float distance = float.MaxValue;

            // detect monsters
            GameObject[] Monsters = GameObject.FindGameObjectsWithTag("NPC");

            for (int i = 0; i < Monsters.Length; i++)
            {
                if (Monsters[i].gameObject.name.Contains("NPC_Monster")) 
                {
                    float newDistance = Vector3.Distance(gameObject.transform.position, Monsters[i].transform.position);

                    if (newDistance < distance)
                    {
                        currentTarget = Monsters[i];
                        distance = newDistance;
                    }
                }
            }

            if (currentTarget != null && hero != null)
            {
                if (!(hero.CurrentAction is ActionAttack))
                {
                    //Debug.Log(target.transform.position);     // target position checking
                    //hero.SetCurrentAction(new ActionHunt( ActionNPC.ActionPriority.Medium), target, true, true);
                    // TODO: Use an action that hunts monsters beyond the normal 'hunt distance' and calls EnemyKilled() when monster was killed
                    hero.SetCurrentAction(new ActionMonsterQuest(this, ActionNPC.ActionPriority.Medium), currentTarget, true, true);
                }
            }
        }
    }

}