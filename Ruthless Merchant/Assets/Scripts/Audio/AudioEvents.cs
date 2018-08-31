using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    class AudioEvents : MonoBehaviour
    {
        void NPC_Step(string _path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_path, this.transform.position);
            //event:/Characters/Monster/MonsterStep
        }
    }
}
