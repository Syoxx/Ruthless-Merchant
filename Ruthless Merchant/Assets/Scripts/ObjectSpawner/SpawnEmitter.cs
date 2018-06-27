//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using UnityEngine;

namespace RuthlessMerchant
{
    public class SpawnEmitter : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnObject = null;

        [SerializeField]
        [Range(0, 1000)]
        private float intervall = 1;

        [SerializeField]
        [Range(1, 1000)]
        private int count = 1;

        [SerializeField]
        private string[] possiblePaths = null;

        [HideInInspector]
        public Faction Faction;

        private ObjectSpawner spawner;
        private float elapsedTime = 0f;
        private int selectedPath = -1;

        private void Start()
        {
            spawner = GetComponent<ObjectSpawner>();
            spawner.OnObjectSpawned += Spawner_OnObjectSpawned;
        }

        private void Spawner_OnObjectSpawned(object sender, SpawnArgs e)
        {
            if(possiblePaths != null && selectedPath >= 0 && selectedPath < possiblePaths.Length)
            {
                NPC npc = e.SpawnedObject.GetComponent<NPC>();
                if (npc != null)
                {
                    npc.SetPath(possiblePaths[selectedPath], 3, true);
                    npc.ChangeFaction(Faction);
                    npc.SetCurrentAction(new ActionMove(), null);
                }
            }
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= intervall)
            {
                spawner.Spawn(spawnObject, count);
                elapsedTime = 0;
                if(possiblePaths != null)
                    selectedPath = Random.Range(0, possiblePaths.Length);
            }
        }
    }
}
