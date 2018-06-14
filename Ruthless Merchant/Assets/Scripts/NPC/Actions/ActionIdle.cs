//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class ActionIdle : ActionNPC
    {
        private float elapsedIdleTime;

        public override void StartAction(NPC parent, GameObject other)
        {
            elapsedIdleTime = 0;
            base.StartAction(parent, other);
        }

        public override void Update(float deltaTime)
        {
            elapsedIdleTime += deltaTime;
        }
    }
}
