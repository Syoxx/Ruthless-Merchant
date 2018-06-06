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
            if (other.transform != transform.parent)
                npc.OnEnterViewArea(other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform != transform.parent)
                npc.OnExitViewArea(other);
        }
    }
}
