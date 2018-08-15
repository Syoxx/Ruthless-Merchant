using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationProgress : MonoBehaviour {

    private float currentRepImperialists, currentRepFreethinkers;
    private bool maxRepImperialists, maxRepFreethinkers;
    private GameObject checkMark, questName, questText;
	// Use this for initialization
	void Start () {
        checkMark = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        questName = this.gameObject.transform.GetChild(0).gameObject;
        questText = this.gameObject.transform.GetChild(3).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        currentRepImperialists = GameObject.FindGameObjectWithTag("Player").GetComponent<Reputation>().ImperalistenStanding;
	}
}
