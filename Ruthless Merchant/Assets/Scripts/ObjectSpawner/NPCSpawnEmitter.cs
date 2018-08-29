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

        [SerializeField, Range(0, 1)]
        private int laneSelectionIndex = 0;

        [SerializeField]
        private Faction faction;

        protected override void Start()
        {
            base.Start();
            spawner.OnObjectSpawned += Spawner_OnObjectSpawned;
        }

        protected override void Update()
        {
            if(NPC.NPCCount[faction] + 1 <= NPC.MaxNPCCountPerFaction)
                base.Update();
        }

        private void Spawner_OnObjectSpawned(object sender, SpawnArgs e)
        {
            NPC npc = e.SpawnedObject.GetComponent<NPC>();
            if (npc != null)
            {
                npc.SetPath(nextOutpost, 0, true, laneSelectionIndex);
                npc.SetCurrentAction(new ActionMove(), null);
                if(npc is Minion)
                {
                    Minion minion = (Minion)npc;
                    if (faction == Faction.Freidenker)
                        minion.FirstOutpost = nextOutpost;
                    else if (faction == Faction.Imperialisten)
                        minion.FirstOutpost = nextOutpost;
                }
            }
        }
    }
}
