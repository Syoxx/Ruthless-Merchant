//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

#define NPCDebugging
using UnityEngine;

namespace RuthlessMerchant
{

    public abstract class ActionNPC
    {
        protected NPC parent;
        protected GameObject other;

        /// <summary>
        /// Sets the parent and a target gameobject
        /// </summary>
        /// <param name="parent">Owner of the action</param>
        /// <param name="other">Target of the action</param>
        public virtual void StartAction(NPC parent, GameObject other)
        {
            this.parent = parent;
            this.other = other;
#if NPCDebugging && Debug
            Debug.Log("Action \"" + GetType().FullName + "\" started!");
#endif
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        public virtual void EndAction()
        {

        }
    }
}
