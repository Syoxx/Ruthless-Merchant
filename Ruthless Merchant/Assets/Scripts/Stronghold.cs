using UnityEngine;

namespace RuthlessMerchant
{
    public class Stronghold : MonoBehaviour
    {
        private CaptureTrigger captureTrigger;
        private SpawnEmitter spawnEmitter;

        [SerializeField]
        private Faction startFaction = Faction.None;

        // Use this for initialization
        void Start()
        {
            captureTrigger = GetComponent<CaptureTrigger>();
            captureTrigger.Owner = startFaction;

            spawnEmitter = GetComponentInChildren<SpawnEmitter>();
            spawnEmitter.Faction = startFaction;            
        }

        // Update is called once per frame
        void Update()
        {
            if (spawnEmitter != null)
                spawnEmitter.enabled = captureTrigger.Owner != Faction.None;
        }
    }
}
