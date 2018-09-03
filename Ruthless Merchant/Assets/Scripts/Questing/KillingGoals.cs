using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class KillingGoals : MonoBehaviour
    {
        [SerializeField]
        private List<KillGoal> KillGoalsList;

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

        public void AssignQuest(GameObject button)
        {
            if (killGoal != null && questingEnabled)
            {
                Debug.Log("kill quest assigned.");

                if (killGoal.Completed || !killGoal.InProgress /*|| enemies killed <= 0*/)
                {

                    GameObject target = null; // targeted by the hero
                    float distance = float.MaxValue;

                    // List<GameObject> questTargets = new List<GameObject>();

                    killGoal.SetTargetList(button);

                    killGoal.CalcNextWayPoint();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = true;
            }


        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                questingEnabled = false;
            }
        }
    }
}
