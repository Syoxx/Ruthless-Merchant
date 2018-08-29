//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class Monster : Fighter
    {
        public override void Interact(GameObject caller)
        {
            Debug.Log(caller.name + ": Interaction with Monster");
        }

        public override void Update()
        {
            if (CurrentAction is ActionIdle || CurrentAction == null)
                SetCurrentAction(new ActionWander(6, 2, 5, ActionNPC.ActionPriority.Low), null);

            base.Update();
        }
    }
}