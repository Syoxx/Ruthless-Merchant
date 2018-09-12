using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace RuthlessMerchant {
    public class CollectionGoal : Goal {
        /// <summary>
        /// This class sits on the hero and handles the execution of a collection quest by the hero
        /// /// <summary>


        /// <summary>
        ///The hero to control the hero state once he receives a collectionGoal-quest
        /// </summary>
        Hero hero;
        /// <summary>
        ///The button-script to make adjustments on buttons from each outpost (CollectionGoals)
        /// </summary>
        QuestButton questButton;
        /// <summary>
        ///Quest-Goals are being executed
        /// </summary>
        private bool inProgress;
        /// <summary>
        ///The reputition gain the player gathers from handing out quests
        /// </summary>
        private float repGain;
        /// <summary>
        ///A list of the collectables in the scene to calculate the nearest collectable rquired in the quest
        /// </summary>
        private GameObject[] Materials;

        /// <summary>
        ///A list of collectables that the hero will gather once the quest got received
        /// </summary>
        public List<Collectables> collectables;

        
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
        /// <summary>
        ///This method gets called when a collectable is found to increment the counter(Current Amount) and to check if the quest is completed
        /// </summary>
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

        /// <summary>
        ///Gets called by the CollectionGoals-Script attached to the various outposts and hands over the quest-goals[as soon as the player assignes a quest] with its collectables and required amounts
        /// </summary>
        public void FillList(List<Collectables> Collectables, float reputationGain, GameObject btn)
        {
            Debug.Log("Fills list");
            collectables = Collectables;
            repGain = reputationGain;
            Completed = false;
            questButton = btn.GetComponentInChildren<QuestButton>();
            Materials = GameObject.FindGameObjectsWithTag(collectables[0].item.ItemInfo.ItemName); 
           
            questButton.InProgressButton();
            InProgress = true;
        }

        /// <summary>
        ///Checks if the rquired amount of one collectable is met
        /// </summary>
        private void EvaluateCollectables(int index)
        {
            if (collectables[index].currentAmount >= collectables[index].requiredAmount)
            {
                collectables[index].completed = true;
                if(index+1 < collectables.Count)
                    Materials = GameObject.FindGameObjectsWithTag(collectables[index+1].item.ItemInfo.ItemName);
            }         
        }

        /// <summary>
        ///Checks if the required amount of all collectables is met
        /// </summary>
        private bool EvaluateGoal()
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
                questButton.DiscardQuestButton(Reward);
            }
            return true;
        }

        /// <summary>
        ///Calculates the next waypoint that the hero should walk to to collect the needed collectable [Sets the currentState of the hero]
        /// </summary>
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

        /// <summary>
        ///Greys out button when disabled
        /// </summary>
        public void GreyOutButton()
        {
            questButton.GreyOut();
        }

        /// <summary>
        ///Sets the buttons [that is linked to this quest] default color
        /// </summary>
        public void DefaultColor()
        {
            questButton.DefaultColor();
        }
    }
}
