//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class SeeScript : MonoBehaviour
    {
        private NPC npc;

        public void Start()
        {
            npc = GetComponentInParent<NPC>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger && other.transform != transform.parent && (other.CompareTag("NPC") || other.CompareTag("Player") || other.CompareTag("Item")))
                npc.OnEnterViewArea(other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger && other.transform != transform.parent && (other.CompareTag("NPC") || other.CompareTag("Player") || other.CompareTag("Item")))
                npc.OnExitViewArea(other);
        }
    }
}
