using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace RuthlessMerchant {
    public class CollectionGoal : Goal {

        Hero hero;
        //GameObject button;
        QuestButton questButton;

        [SerializeField]
        private bool inProgress;
        private int QuestZoneID;
        private float repGain;
        private int heroReward;

        private GameObject[] Materials;
        

        public List<Collectables> collectables;
        private List<Material> FoundMaterials;
        
        public CollectionGoal(string description, bool completed, string questTitle, bool inProgress)
        {
            QuestTitle = questTitle;
            Description = description;
            Completed = completed;
            InProgress = inProgress;
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

        public void CollectableFound(Item foundMaterial)
        {
            for (int i = 0; i < collectables.Count; i++)
            {
                if (foundMaterial != null)
                {
                    if (collectables[i].item.ItemInfo.ItemName == foundMaterial.ItemInfo.ItemName)
                    {
                        collectables[i].currentAmount++;
                        EvaluateCollectables(i);

                        break;
                    }
                }
                else if (collectables[i].item.ItemInfo.ItemName == "Iron")
                {
                    collectables[i].currentAmount += 3;
                    EvaluateCollectables(i);
                    break;
                }
            }
            Completed = EvaluateGoal();

        }

        public void FillList(List<Collectables> Collectables, float reputationGain, int reward, GameObject btn)
        {
            Debug.Log("Fills list");
            collectables = Collectables;
            repGain = reputationGain;
            heroReward = reward;
            Completed = false;
            //button = btn;
            questButton = btn.GetComponentInChildren<QuestButton>();

            Materials = GameObject.FindGameObjectsWithTag(collectables[0].item.ItemInfo.ItemName); 
           
            
            
            // Materials = GameObject.FindGameObjectsWithTag(collectables[0].material.ItemInfo.ItemName);
            questButton.InProgressButton();
            InProgress = true;
            //questButton.ButtonSettings(false);
            Debug.Log(Materials.Length);
        }

        void EvaluateCollectables(int index)
        {
            if (collectables[index].currentAmount >= collectables[index].requiredAmount)
            {
                collectables[index].completed = true;
                if(index+1 < collectables.Count)
                    Materials = GameObject.FindGameObjectsWithTag(collectables[index+1].item.ItemInfo.ItemName);
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
            Player.Singleton.GetComponent<Reputation>().ChangeStanding(hero.Faction, repGain);


            if (questButton)
            {
                questButton.CompleteButton();
                InProgress = false;
                //button.GetComponent<Button>().onClick.AddListener(delegate {  });
                questButton.DiscardQuestButton(heroReward, collectables);
            }
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
        public void GreyOutButton()
        {
            if (questButton)
            {
                questButton.GreyOut();
            }
            
        }
        public void DefaultColor()
        {
            if (questButton)
            {
                questButton.DefaultColor();
            }
            
        }
        //void RemoveButton()
        //{
        //    Debug.Log("Destroy" + button);
        //    Destroy(button);
        //}

    }
}
