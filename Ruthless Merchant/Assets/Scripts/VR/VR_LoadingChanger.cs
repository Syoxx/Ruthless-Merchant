using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VR_LoadingChanger : MonoBehaviour
{
    TextMeshPro text;
    float timer;
    const float timerLimit = 0.5f;

	void Awake ()
    {
        text = GetComponent<TextMeshPro>();
        timer = Random.value;
    }
	
	void Update ()
    {
        timer += Time.deltaTime;

        if(timer > timerLimit)
        {
            int pointsInText = text.text.Replace("Loading", "").Length;

            if (pointsInText >= 3)
                text.text = "Loading";

            else
                text.text += ".";

            timer = 0;
        }
	}
}
