using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterLogic : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 guardPosition;
    private bool tradeIsDone;
    private bool guardsDead;
    private bool haveReachedGuards;

    private Collider playerCollider, triggerZoneCollider;

    private int attackCounter = 0;

    [SerializeField] private GameObject destination;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject triggerZone;


    [SerializeField] private float speed;

    void Start()
    {
        startPosition = transform.position;
        guardPosition = destination.transform.position;

        playerCollider = Player.GetComponent<Collider>();
        triggerZoneCollider = triggerZone.GetComponent<Collider>();
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        float distanceToGuard = Vector3.Distance(transform.position, destination.transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        //If trading is done, Monster will start to hunt his targets down
        if (tradeIsDone)
        {
            //If Monster is not close enough, he will move towards the guards
            if (distanceToGuard >= 6 && !haveReachedGuards)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);            
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
            if(distanceToPlayer >= 6 && guardsDead)
            { 
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
            }

            //if close = attack
            else
            {
                Attack();
                PlayerIsDead();
            }

        }

        //Player trying to leave the zone, he is dead
        if (playerCollider.bounds.Intersects(triggerZoneCollider.bounds))
        {
            Attack();
            PlayerIsDead();
        }
    }

    private void Attack()
    {
        //Animation
        //2 Attacks = Guards dead
        attackCounter = attackCounter + 1;
    }

    private void PlayerIsDead()
    {
        SceneManager.LoadScene("Islandtesting");
    }
}
