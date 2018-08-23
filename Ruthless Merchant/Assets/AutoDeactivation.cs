using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoDeactivation : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    float timePerLetter;

    float time;

    int textLength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(time + textLength * timePerLetter - Time.time);
		if(Time.time > time + textLength * timePerLetter)
        {
            gameObject.SetActive(false);
        }
	}

    private void OnEnable()
    {
        time = Time.time;
        textLength = text.text.Length;
    }
}
