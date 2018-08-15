using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterLogic : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition;
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
        endPosition = destination.transform.position;
        playerCollider = Player.GetComponent<Collider>();
        triggerZoneCollider = triggerZone.GetComponent<Collider>();
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        float distanceToGuard = Vector3.Distance(transform.position, destination.transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (tradeIsDone)
        {
            if (distanceToGuard >= 6 && !haveReachedGuards)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);            
            }

            else if (attackCounter < 2)
            {
                haveReachedGuards = true;
                Attack();
            }

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
            else
            {
                Attack();
            }

            if (attackCounter >= 3)
            {
                PlayerIsDead();
            }
        }

        if (playerCollider.bounds.Intersects(triggerZoneCollider.bounds))
        {
            PlayerIsDead();
        }
    }

    private void Attack()
    {
        //Animation
        //2 Attacks = Guards dead
        attackCounter = attackCounter + 1;
        Debug.Log(attackCounter);
    }

    private void PlayerIsDead()
    {
        SceneManager.LoadScene("Islandtesting");
    }
}
