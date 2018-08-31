using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class KillingGoals : MonoBehaviour
    {

        private bool questingEnabled;
        private KillGoal killGoal;
        Hero hero;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AssignQuest()
        {
            if (killGoal != null && questingEnabled)
            {
                Debug.Log("kill quest assigned.");

                GameObject target = null; // targeted by the hero
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
                        }
                    }
                }

                // killgoal.setAction
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = true;
            }
        }
    }
}
