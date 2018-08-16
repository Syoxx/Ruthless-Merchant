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

        public CollectionGoal(string description, bool completed, string questTitle)
        {
            QuestTitle = questTitle;
            Description = description;
            Completed = completed;
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
        }
        private void Update()
        {

        }

        public void CalcRequiredAmount()
        {
            for (int i = 0; i < collectables.Count; i++)
            {
                RequiredAmount += collectables[i].requiredAmount;
            }
        }

        public void CollectableFound(Material foundMaterial)
        {
            for (int i = 0; i < collectables.Count; i++)
            {
                if (collectables[i].material.ItemName == foundMaterial.ItemName)
                {
                    collectables[i].currentAmount++;
                    EvaluateCollectables(i);

                    break;
                }                
            }
            Completed = EvaluateGoal();

        }

        public void FillList(List<Collectables> Collectables)
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
                if(index+1 < collectables.Count)
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
            Materials = new GameObject[Materials.Length];
            return true;
        }

        public void CalcNextWaypoint()
        {
            GameObject gobj = null;
            float distance = float.MaxValue;
            for (int i = 0; i < Materials.Length; i++)
            {
                if (Materials[i] != null)
                {
                    float newDistance = Vector3.Distance(gameObject.transform.position, Materials[i].transform.position);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        gobj = Materials[i];
                    }
                }
            }

            if (gobj != null && hero != null)
            {
                if (!(hero.CurrentAction is ActionAttack))
                    Hero.SetCurrentAction(new ActionCollect(this, ActionNPC.ActionPriority.Medium), gobj, true, true);
            }
        }
    }
}
