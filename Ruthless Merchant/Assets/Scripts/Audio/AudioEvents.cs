using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    class AudioEvents : MonoBehaviour
    {
        void NPC_Steps(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Monster/MonsterStep
            //event:/Characters/Minions/MinionStepsFreeminds
            //event:/Characters/Minions/MinionStepsImperialists
        }

        void NPC_Attack(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Minions/NPC swords
            //event:/Characters/Monster/MonsterHit
        }
    }
}
