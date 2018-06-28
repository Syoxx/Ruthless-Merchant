//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class NPCSpawnEmitter : SpawnEmitter
    {
        [SerializeField]
        private CaptureTrigger nextOutpost = null;

        protected override void Start()
        {
            base.Start();
            spawner.OnObjectSpawned += Spawner_OnObjectSpawned;
        }

        private void Spawner_OnObjectSpawned(object sender, SpawnArgs e)
        {
            NPC npc = e.SpawnedObject.GetComponent<NPC>();
            if (npc != null)
            {
                npc.SetPath(nextOutpost, 0, true);
                npc.SetCurrentAction(new ActionMove(), null);
            }
        }
    }
}
