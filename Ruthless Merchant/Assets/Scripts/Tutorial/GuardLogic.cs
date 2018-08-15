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

    }

    private void Dead()
    {
       //Plays Death Animation
    }
}
