using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Tutorial : MonoBehaviour
    {
     [SerializeField]
     private Item ironSword;
	// Hardcoded Stuff
	void Start ()
	{

	    Player.Singleton.Inventory.Add(ironSword, 5, true);
	    //MovementForMonster() 
	    //FightFunction() for 2 Minions vs Monster
	    //DeathFunction() for 2 Minions
	    //HuntFunction() for Monster
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


}
