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

        /// <summary>
        /// Base constructor of Actions
        /// </summary>
        /// <param name="priority">Action priority</param>
        public ActionNPC(ActionPriority priority)
        {
            this.priority = priority;
        }

        /// <summary>
        /// Initialaztion of the action
        /// </summary>
        /// <param name="parent">Action owner</param>
        /// <param name="other">Action target</param>
        public virtual void StartAction(NPC parent, GameObject other)
        {
            this.parent = parent;
            this.other = other;
        }

        /// <summary>
        /// Updates the action
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// EndAction can be used to do some cleanup
        /// </summary>
        /// <param name="executeEnd">Indicates if abort code should be executed</param>
        public virtual void EndAction(bool executeEnd = true)
        {
        }
    }
}
