//Author: Marcel Croonenbroeck

using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    [SerializeField]
    private GameObject BloodFxPrefab = null;

    [SerializeField, Tooltip("Indicates how many particlesystems should be created on game start and can be played at once")]
    private int bloodFxPoolSize = 20;

    public static Queue<ParticleSystem> freeSystems = new Queue<ParticleSystem>();
    private static List<ParticleSystem> systemsInUse = new List<ParticleSystem>();

	// Use this for initialization
	void Awake ()
    {
        for (int i = 0; i < bloodFxPoolSize; i++)
        {
            GameObject fxObject = Instantiate(BloodFxPrefab);
            freeSystems.Enqueue(fxObject.GetComponent<ParticleSystem>());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		lock(freeSystems)
        {
            for (int i = 0; i < systemsInUse.Count; i++)
            {
                if(!systemsInUse[i].isPlaying)
                    freeSystems.Enqueue(systemsInUse[i]);
            }
        }
	}

    public static ParticleSystem GetFreeFX()
    {
        lock (freeSystems)
        {
            ParticleSystem fx = freeSystems.Dequeue();
            if (fx != null)
                systemsInUse.Add(fx);

            return fx;
        }
    }
}
