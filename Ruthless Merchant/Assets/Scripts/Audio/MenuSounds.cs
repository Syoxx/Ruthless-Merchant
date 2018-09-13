//---------------------------------------------------------------
// Author: Fabian Subat
//
//---------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour {

    private bool soundPlayed;
    private FMOD.Studio.EventInstance soundevent;
    FMOD.Studio.PLAYBACK_STATE playbackState;

    private void OnMouseOver()
    {
        if (!soundPlayed)
        {
            soundPlayed = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI Generic/Mouseover");
        }

    }

    private void OnMouseExit()
    {
        soundPlayed = false;
    }
}
