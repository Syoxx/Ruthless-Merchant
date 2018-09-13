//---------------------------------------------------------------
// Author: Fabian Subat
//
//---------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    class AudioEvents : MonoBehaviour
    {
        /// <summary>
        /// Event fired by Animations of NPC's for stepsounds. Commented lines specify valid events
        /// </summary>
        /// <param name="_path"></param>
        void NPC_Steps(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Monster/MonsterStep
            //event:/Characters/Minions/MinionStepsFreeminds
            //event:/Characters/Minions/MinionStepsImperialists
        }

        /// <summary>
        /// Event fire by Animations of NPC's Attacks. Commented Lines specify valid events
        /// </summary>
        /// <param name="_path"></param>
        void NPC_Attack(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Minions/NPC swords
            //event:/Characters/Monster/MonsterHit
        }

        void NPC_Death(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Deaths/NPC Death
        }
    }
}
