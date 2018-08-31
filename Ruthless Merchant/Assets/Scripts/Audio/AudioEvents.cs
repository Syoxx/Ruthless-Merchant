using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    class AudioEvents : MonoBehaviour
    {
#if FMOD

        void NPC_Step(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Monster/MonsterStep
        }


#endif
    }
}
