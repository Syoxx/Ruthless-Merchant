using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace RuthlessMerchant {
    public class CollectionGoal : Goal {

        Hero hero;

        [SerializeField]
        private int CollectableID;
        private int QuestZoneID;

        private GameObject[] Materials;
        

        public List<Collectables> collectables;
        private List<Material> FoundMaterials;

        public CollectionGoal(string description, bool completed, List<Transform> waypoints)
        {
            Description = description;
            Completed = completed;
            Waypoints = waypoints;
        }

        public Hero Hero
        {
            get
            {
                if (hero == null)
                {
                    hero = GetComponent<Hero>();
                }
                return hero;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

        }
        private void Start()
        {
            collectables = new List<Collectables>();
            hero = GetComponent<Hero>();
            //Materials = GameObject.FindGameObjectsWithTag("Iron");
            Debug.Log("descr: "+Description);
            //CalcRequiredAmount();
        }
        private void Update()
        {
            if(Materials != null)
            Debug.Log("Count: "+Materials.Length);
            //Debug.Log(Materials.Length + " check");
            if (collectables.Count > 0)
            {
                //CalculateDistances();
            }
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
            
             Materials = GameObject.FindGameObjectsWithTag(collectables[0].material.ItemName);
        }
        void EvaluateCollectables(int index)
        {
            if (collectables[index].currentAmount >= collectables[index].requiredAmount)
            {
                collectables[index].completed = true;
                if(index < collectables.Count)
                    Materials = GameObject.FindGameObjectsWithTag(collectables[index+1].material.ItemName);
            }
            
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

        public void CalcNextWaypoint()
        {

            //float distance = Vector3.Distance(this.gameObject.transform.position, Materials[0].transform.position);
            //for (int i = 0; i < Materials.Length; i++)
            //{
            //    if (Vector3.Distance(this.gameObject.transform.position, Materials[i].transform.position) <= distance)
            //    {
            //        distance = Vector3.Distance(this.gameObject.transform.position, Materials[i].transform.position);
            //        //hero.AddNewWaypoint(new Waypoint(Materials[i].transform, true, 0), true);
            //        Debug.Log(Materials[i].transform);
            //        if (!(hero.CurrentAction is ActionAttack))
            //            Hero.SetCurrentAction(new ActionCollect(this, ActionNPC.ActionPriority.Medium), Materials[i], true, true);
            //    }
            //}


        }
    }
}
