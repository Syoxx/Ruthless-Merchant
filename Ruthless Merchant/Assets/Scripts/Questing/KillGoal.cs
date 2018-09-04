using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class KillGoal : Goal {

        Hero hero;
        GameObject currentTarget;
        Character targetChar;
        private GameObject[] Targets;
        private QuestButton questButton;
        private float repGain;
        private bool wasKillCounted;

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
            wasKillCounted = true;
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

            //hero.CurrentAction is ActionIdle
            //if (InProgress && currentTarget != null)
            //{
            //hero.SetCurrentAction(new ActionMonsterQuest(this, ActionNPC.ActionPriority.Medium), currentTarget, true, true);
            //}

            if (questButton && currentTarget)
            {

                if (targetChar != null)
                {
                    if (InProgress && targetChar.IsDying)
                    {
                        EnemyKilled();
                        targetChar = null;
                    }
                }
            else if (!Completed)
            {
                    // only calc next waypoint if target monster was destroyed
                CalcNextWayPoint();
            }
        }
    }

        public void EnemyKilled()
        {
            if (!wasKillCounted)
            {
                wasKillCounted = true;
                CurrentAmount += 1;
                CalcNextWayPoint();
            }
            
            //Evaluate();
            Completed = EvaluateGoal();
        }

        private bool EvaluateGoal()
        {
            if (CurrentAmount < RequiredAmount)
            {
                //CalcNextWayPoint();
                return false;
            }
            else
            {
                Player.Singleton.GetComponent<Reputation>().ChangeStanding(hero.Faction, repGain);

                InProgress = false;

                if (questButton)
                {
                    questButton.CompleteButton();
                    questButton.DiscardQuestButton(Reward);
                }
                
                hero.SetCurrentAction(new ActionMove(ActionNPC.ActionPriority.Medium), hero.Outpost.gameObject, true, true);

                return true;
            }
        }

        public void SetTargetList(GameObject button, int monsterAmount, float reputationGain)
        {
            Completed = false;

            if (CurrentAmount != 0)
            {
                CurrentAmount = 0;
            }

            questButton = button.GetComponentInChildren<QuestButton>();

            questButton.InProgressButton();
            InProgress = true;

            RequiredAmount = monsterAmount;
            repGain = reputationGain;
            
        }

        public void CalcNextWayPoint()
        {
            wasKillCounted = false;
            Debug.Log("calculating monster point");
            currentTarget = null;
            float distance = float.MaxValue;

            // detect monsters
            GameObject[] Monsters = GameObject.FindGameObjectsWithTag("NPC");

            for (int i = 0; i < Monsters.Length; i++)
            {
                if (Monsters[i].gameObject.name.Contains("NPC_Monster") && !Monsters[i].GetComponent<Character>().IsDying) 
                {
                    float newDistance = Vector3.Distance(gameObject.transform.position, Monsters[i].transform.position);

                    if (newDistance < distance)
                    {
                        currentTarget = Monsters[i];
                        targetChar = currentTarget.GetComponent<Character>();
                        distance = newDistance;
                    }
                }
            }

            if (currentTarget != null && hero != null)
            {
                //if (!(hero.CurrentAction is ActionAttack))
                {
                    hero.SetCurrentAction(new ActionMonsterQuest(this, ActionNPC.ActionPriority.Medium), currentTarget, true, true);
                    //InProgress = true;
                }

                wasKillCounted = false;
            }

        }

        public void GreyOut()
        {
            questButton.GreyOut();
        }

        public void DefaultColor()
        {
            questButton.DefaultColor();
        }
    }

}