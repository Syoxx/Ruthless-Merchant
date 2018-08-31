﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant {
    public class KillGoal : Goal {

        //[SerializeField]
        //private int EnemyID;
        [SerializeField]


        
        QuestButton questButton;

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

        private void Update()
        {
            //fot Testing
            if (Input.GetKeyDown(KeyCode.K))
                EnemyDied();
        }

        void EnemyDied()
        {
            CurrentAmount++;
            Evaluate();
        }
    }

}