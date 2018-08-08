using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string Steps;
    public float stepSpeed;

    private string soundToPlay;
    private bool playerismoving;
    private FMOD.Studio.EventInstance stepSound;
    private FMOD.Studio.ParameterInstance floorMaterial;
 
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
            //Debug.Log("Stepsound playing");
            FMODUnity.RuntimeManager.PlayOneShot(Steps, GetComponent<Transform>().position);
        }
    }

    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, stepSpeed);
        stepSound = FMODUnity.RuntimeManager.CreateInstance(Steps);
    }

    void OnDisable()
    {
        playerismoving = false;
    }
}