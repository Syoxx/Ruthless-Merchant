using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{
    public class MonsterLogic : MonoBehaviour
    {
        //Positions for "MoveToward" function
        private Vector3 startPosition;
        private Vector3 traderPosition;

        //Check if guards are dead -> hunt player
        private bool guardsDead;

        //If Guards are reached - monster should kill them
        private bool haveReachedGuards;

        //Collider for triggerzone of a player -> he can't leave -> he will die
        private Collider playerCollider, triggerZoneCollider;
        private Animator animator;

        //Hardcoded integer -> needed to count attacks. 2 Attacks = Guard dead. 3rd Attack = Player dead
        private int attackCounter = 0;


        [SerializeField]
        [Tooltip("Drag MonsterDestination from Tutorial there")]
        private GameObject MonsterDestinationObject;
        [SerializeField]
        [Tooltip("Drag Player there")]
        private GameObject playerObject;
        [SerializeField]
        [Tooltip("Drag DeathTriggerZone from Tutorial there")]
        private GameObject triggerZoneObject;



        //Speed for Monster
        [Tooltip("Adjust speed of the Monster")]
        [SerializeField] private float speed;

        void Start()
        {
            //Getting the positions and colliders of needed object
            startPosition = transform.position;
            traderPosition = MonsterDestinationObject.transform.position;
            animator = gameObject.GetComponent<Animator>();

            playerCollider = playerObject.GetComponent<Collider>();
            triggerZoneCollider = triggerZoneObject.GetComponent<Collider>();
        }

        void Update()
        {
            float step = speed * Time.deltaTime;
            float distanceToGuard = Vector3.Distance(transform.position, MonsterDestinationObject.transform.position);
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);


            //If trading is done, Monster will start to hunt his targets down
            if (Tutorial.Singleton.TradeIsDone)
            {
                MonsterHunt(step, distanceToGuard, distanceToPlayer);
            }

            //Player trying to leave the zone, he is dead
            if (playerCollider.bounds.Intersects(triggerZoneCollider.bounds))
            {
                PlayerIsDead();
            }
        }

        private void MonsterHunt(float step, float distanceToGuard, float distanceToPlayer)
        {
            //If Monster is not close enough, he will move towards the guards
            if (distanceToGuard >= 1 && !haveReachedGuards)
            {
                transform.position = Vector3.MoveTowards(transform.position, MonsterDestinationObject.transform.position, step);
                transform.rotation =
                    Quaternion.LookRotation(playerObject.transform.position);
            }

            // 2 Guards = 2 Attacks (1 hit to kill)
            else if (attackCounter < 2)
            {
                haveReachedGuards = true;
                Attack();
            }

            // 2 hits = dead
            if (attackCounter == 2)
            {
                guardsDead = true;
            }

            // Hardcoded. If Monster has attacked twice = Guards are dead.
            // Monster trying to reach Player
            if (distanceToPlayer >= 1 && guardsDead)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, step);
            }

            //if close = attack
            else if (distanceToPlayer <= 1 && guardsDead)
            {
                Attack();
                PlayerIsDead();
            }

        }

        private void Attack()
        {
            //Animation
            //2 Attacks = Guards dead
            //
            attackCounter = attackCounter + 1;
            Debug.Log(attackCounter);
        }

        private void PlayerIsDead()
        {
            SceneManager.LoadScene("Islandtesting");
        }
    }
}
