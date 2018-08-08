using UnityEngine;
using System.Collections;

public class PaperCraftAnim : MonoBehaviour 
{
    //Variables //////////////////////////////////////////////////////////////////
    Animation anim;
    public AnimationClip animClip;

    public float stepLength = 0.5f;
    float deltaTime = 0;
    float deltaStep = 0;

    //Functions ////////////////////////////////////////////////////////////
    void Start () 
    {
	    anim = GetComponent<Animation>();
	}

    
	void Update () 
    {
        deltaTime += Time.deltaTime;
        deltaStep += Time.deltaTime;


        if (deltaStep > stepLength)
        {
            //Just play one frame of the animation
            anim[animClip.name].time = deltaTime;
            anim.Play();

            //One frame, Carl
            deltaStep = 0;
        }
        else
            anim.Stop();
	}
}
