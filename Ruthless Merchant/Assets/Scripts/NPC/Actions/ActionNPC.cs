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
        public enum ActionPriority
        {
            None,
            Low,
            Medium,
            High,
        }

        protected NPC parent;
        protected GameObject other;
        protected ActionPriority priority;

        public ActionPriority Priority
        {
            get
            {
                return priority;
            }
        }

        public ActionNPC(ActionPriority priority)
        {
            this.priority = priority;
        }

        /// <summary>
        /// Sets the parent and a target gameobject
        /// </summary>
        /// <param name="parent">Owner of the action</param>
        /// <param name="other">Target of the action</param>
        public virtual void StartAction(NPC parent, GameObject other)
        {
            this.parent = parent;
            this.other = other;
//#if NPCDebugging && Debug
            Debug.Log("Action \"" + GetType().FullName + "\" started!");
//#endif
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        public virtual void EndAction(bool executeEnd = true)
        {
           // #if NPCDebugging && Debug
            Debug.Log("Action \"" + GetType().FullName + "\" ended!");
           // #endif
        }
    }
}
