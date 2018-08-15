using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Tutorial : MonoBehaviour
    {
     [SerializeField]
     private Item ironSword;

     public GameObject teleportCaveUp;
     public GameObject caveEnter;
     public GameObject PlayerObject;
     private Collider playerCollider, teleportUpCollider;
    
	// Hardcoded Stuff
	void Start ()
	{
	    playerCollider = PlayerObject.GetComponent<Collider>();
	    teleportUpCollider = teleportCaveUp.GetComponent<Collider>();

	    Player.Singleton.Inventory.Add(ironSword, 5, true);
	}
	
	// Update is called once per frame
	void Update () {

	    if (playerCollider.bounds.Intersects(teleportUpCollider.bounds))
	    {
           transform.position = new Vector3(caveEnter.transform.position.x,caveEnter.transform.position.y - 5,caveEnter.transform.position.z);
	    }
	}
}


}
