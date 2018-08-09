//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class Monster : Fighter
    {
        public GameObject usedSpawnLocation;
        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Monster");
        }
    }
}