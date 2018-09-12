//---------------------------------------------------------------
// Author: Fabian Subat
//
//---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class ScoreHandler : MonoBehaviour
    {

        [FMODUnity.EventRef]
        public string caveScore;
        [FMODUnity.EventRef]
        public string generalScore;

        [Tooltip("Add Trade Trigger Gameobject from Scene Hierarchy")]
        public GameObject tradeTrigger;

        private FMOD.Studio.EventInstance soundevent;
        private bool tutorialDone = false;

        void Start()
        {
            soundevent = FMODUnity.RuntimeManager.CreateInstance(caveScore);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, this.transform, this.GetComponent<Rigidbody>());
            soundevent.start();
        }

        void Update()
        {

        }

        /// <summary>
        /// Switches the score from Tutorialsoundtrack to Overworldsoundtrack on leaving the tutorial Cave
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "TriggerCancelTutorial" && !tutorialDone)
            {
                tutorialDone = true;
                Debug.Log("Switching scores");
                soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                soundevent = FMODUnity.RuntimeManager.CreateInstance(generalScore);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, this.transform, this.GetComponent<Rigidbody>());
                soundevent.start();
            }
        }
    }
}
