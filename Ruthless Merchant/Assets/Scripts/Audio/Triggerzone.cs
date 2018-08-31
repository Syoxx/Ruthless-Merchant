using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerzone : MonoBehaviour
{
    public GameObject areaTrigger;

    [FMODUnity.EventRef]
    public string selectSound;

    [SerializeField, Tooltip("Defines how fast the triggered sound should fade out")]
    public float fadeoutSpeed = 0.015f;

    private FMOD.Studio.EventInstance soundevent;
    FMOD.Studio.PLAYBACK_STATE playbackState;
    private bool is3Dimensional;
    private bool isPlaying;
    private bool startupdone = false;
    private float currentVolume, finalVolume, initialVolume;

    /// <summary>
    /// Plays event if Player enters triggerzone
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(playbackState.ToString());
        if(other.gameObject.tag == "Player" && !startupdone)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, other.transform, other.GetComponent<Rigidbody>());
            soundevent.start();
            startupdone = true;
        }
        PlaySound(other);
    }


    /// <summary>
    /// Stops sound if Player exits triggerzone
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(doFadeout());
        }
    }

    IEnumerator doFadeout()
    {
        //Gets current volume levels
        soundevent.getVolume(out currentVolume, out finalVolume);

        //Fades volume down to zero
        while(currentVolume > 0)
        {
            currentVolume = currentVolume - fadeoutSpeed;
            soundevent.setVolume(currentVolume);
            yield return null;
        }

        //Stops the event
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        //Waits for stopped event
        yield return new WaitForSeconds(0.5f);
        soundevent.setVolume(initialVolume);
    }


    private void PlaySound(Collider _other)
    {
        if (_other.gameObject.tag == "Player" && playbackState.ToString() != "PLAYING")
        {
            if (is3Dimensional)
            {
                //Debug.Log("3D Event fired");

                //First Line attaches to the area and plays in it -> Kept because may be needed. Second Line attaches to colliding object for playeratmo
                //FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, this.transform, this.GetComponent<Rigidbody>());
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, _other.transform, _other.GetComponent<Rigidbody>());
                soundevent.start();
            }
            else
            {
                //Debug.Log("2D Event fired");
                soundevent.start();
            }
        }
    }

    /// <summary>
    /// Checks if the passed eventInstance is 3D or 2D event and sets Bool eventDimension
    /// </summary>
    /// <param name="eventInstance"></param>
    private void is3DEvent(FMOD.Studio.EventInstance eventInstance)
    {
        FMOD.Studio.EventDescription description;
        eventInstance.getDescription(out description);
        description.is3D(out is3Dimensional);
        //Debug.Log("Chosen event is:" + eventDimension);
    }

    private void OnDestroy()
    {
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // Use this for initialization
    void Start()
    {
        soundevent = FMODUnity.RuntimeManager.CreateInstance(selectSound);
        is3DEvent(soundevent);
        soundevent.getVolume(out initialVolume, out finalVolume);
    }

    private void Update()
    {
        soundevent.getPlaybackState(out playbackState);
    }
}
