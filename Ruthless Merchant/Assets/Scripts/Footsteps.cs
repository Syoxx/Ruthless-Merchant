using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string inputsound;
    bool playerismoving;
    public float walkingspeed;
    FMOD.Studio.EventInstance stepSound;


    void Update()
    {
        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
        {
            //Debug.Log("Character is moving");
            playerismoving = true;
        }
        else if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log("Character is not moving");
            playerismoving = false;
        }
    }


    void CallFootsteps()
    {
        if (playerismoving == true)
        {
            Debug.Log("Stepsound playing");
            FMODUnity.RuntimeManager.PlayOneShot(inputsound, GetComponent<Transform>().position);
        }
    }

    private void Awake()
    {
        stepSound = FMODUnity.RuntimeManager.CreateInstance(inputsound);
    }

    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, walkingspeed);
    }


    void OnDisable()
    {
        playerismoving = false;
    }
}