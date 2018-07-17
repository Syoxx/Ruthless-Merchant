using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAudioController : MonoBehaviour
{
    #region Variables

    public AudioClip rdmAudio;

    #endregion

    AudioSource audio;
    
    //NOTE: Change description
    /// <summary>
    /// Start
    /// </summary>
    void Start ()
    {
        audio = GetComponent<AudioSource>();
    }

    //NOTE: Change description
    /// <summary>
    /// Update
    /// </summary>
	void Update ()
	{
		//Previous Code here
        audio.PlayOneShot(rdmAudio, 0.7f);
	}
}
