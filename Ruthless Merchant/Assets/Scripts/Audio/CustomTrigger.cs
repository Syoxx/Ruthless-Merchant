using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour {

    public GameObject areaTrigger;

    [FMODUnity.EventRef]
    public string selectSound;

    private FMOD.Studio.EventInstance soundevent;
    private bool eventDimension;
    private bool fadeoutRunning = false;

    private float initialEventVolume;
    private float finalEventVolume;

    /// <summary>
    /// Plays given event - able to take 2D and 3D events
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered trigger area");
            if (eventDimension)
            {
                //Debug.Log("3D Event fired");
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, this.transform, this.GetComponent<Rigidbody>());
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
    /// Stops triggered sound
    /// TODO: Add Fadeout
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Player left trigger area");
            //soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            //Setting "fadeoutRunning" to true triggers the fadeout
            //fadeoutRunning = true;
            doEventFadeout();
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

        description.is3D(out eventDimension);

        //Debug.Log("Chosen event is:" + eventDimension);
    }

    /// <summary>
    /// TODO: Make work properly
    /// </summary>
    private void doEventFadeout()
    {
        float currentValue = initialEventVolume;
        float finalValue = finalEventVolume;

        //soundevent.getVolume(out currentValue, out finalValue);
        //Debug.Log("CurrentVolume: " + currentValue + " FinalVolume: " + finalValue);

        soundevent.getVolume(out currentValue, out finalValue);
        if(currentValue > 0)
        {
            currentValue = currentValue - 0.001f;
            soundevent.setVolume(currentValue);
            Debug.Log("Volume reduced to " + currentValue);
        }

        if (currentValue <= 0)
        {
            soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            soundevent.setVolume(initialEventVolume);
            fadeoutRunning = false;
        }

    }

    // Use this for initialization
    void Start () {
        soundevent = FMODUnity.RuntimeManager.CreateInstance(selectSound);
        soundevent.getVolume(out initialEventVolume, out finalEventVolume);
        is3DEvent(soundevent);
    }
	
	// Update is called once per frame
	void Update () {

	}
}
