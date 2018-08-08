using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RuthlessMerchant {
    public class CollectionGoal : Goal {

        [SerializeField]
        private int CollectableID;
        private int QuestZoneID;

        public List<Collectables> collectables;
        private List<Material> FoundMaterials;

        public CollectionGoal(/*int collectableID,*/ string description, bool completed, /*int currentAmount, int requiredAmount,*/ List<Transform> waypoints/*, int questZoneID*/)
        {
            //CollectableID = collectableID;
            //QuestZoneID = questZoneID;
            Description = description;
            Completed = completed;
            //CurrentAmount = currentAmount;
            //RequiredAmount = requiredAmount;
            Waypoints = waypoints;
        }

        public override void Initialize()
        {
            base.Initialize();

        }
        private void Start()
        {
            collectables = new List<Collectables>();
            //CalcRequiredAmount();
        }
        private void Update()
        {

        }
        //Dont need a requiredAmount
        public void CalcRequiredAmount()
        {
            for (int i = 0; i < collectables.Count; i++)
            {
                RequiredAmount += collectables[i].requiredAmount;
            }
        }
        public void CollectableFound(Material foundMaterial)
        {
            //Possible solution is also just to increment the current amount, depending on if its necessary to keep track of the amount of the specific materials or not
            for (int i = 0; i < collectables.Count; i++)
            {
                if (collectables[i].material == foundMaterial)
                {
                    collectables[i].currentAmount++;
                    EvaluateCollectables(i);

                    //CurrentAmount++;
                    break;
                }                
            }
            Completed = EvaluateGoal();
            //CurrentAmount++;
            //Evaluate();
        }
        public void FillList(/*Material material, int requiredAmount, int index*/List<Collectables> Collectables)
        {
            collectables = Collectables;
            Completed = false;
        }
        void EvaluateCollectables(int index)
        {
            if (collectables[index].currentAmount >= collectables[index].requiredAmount)
                collectables[index].completed = true;
            
        }
        bool EvaluateGoal()
        {
            for (int i = 0; i < collectables.Count; i++)
            {
                if(!collectables[i].completed)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
