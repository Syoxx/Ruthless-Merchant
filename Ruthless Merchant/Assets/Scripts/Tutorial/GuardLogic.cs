using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLogic : MonoBehaviour
{

    public GameObject Monster, Triggerzone;
    public int speed;

    private Collider monsterColider, triggerZoneCollider;
	// Use this for initialization
	void Start () {
	    monsterColider = Monster.GetComponent<Collider>();
	    triggerZoneCollider = Triggerzone.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (monsterColider.bounds.Intersects(triggerZoneCollider.bounds))
	    {
	        float step = speed * Time.deltaTime;
	        transform.position = Vector3.MoveTowards(transform.position, Monster.transform.position, step);
        }

    }

    private void Dead()
    {
       Destroy(gameObject);
    }
}
