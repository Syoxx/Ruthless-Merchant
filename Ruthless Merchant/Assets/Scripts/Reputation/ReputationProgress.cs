using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReputationProgress : MonoBehaviour {

    private float currentRepImperialists, currentRepFreethinkers, maxRep;
    private bool maxRepImperialists, maxRepFreethinkers;
    private GameObject checkMark, questName, questText;
	// Use this for initialization
	void Start () {
        checkMark = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        questName = this.gameObject.transform.GetChild(0).gameObject;
        questText = this.gameObject.transform.GetChild(3).gameObject;
        maxRep = GameObject.FindGameObjectWithTag("Player").GetComponent<Reputation>().MaxReputation;
        checkMark.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        currentRepImperialists = GameObject.FindGameObjectWithTag("Player").GetComponent<Reputation>().ImperalistenStanding;
        currentRepFreethinkers = GameObject.FindGameObjectWithTag("Player").GetComponent<Reputation>().FreidenkerStanding;
        maxRepImperialists = GameObject.FindGameObjectWithTag("Player").GetComponent<Reputation>().ImperialistenMaxAchieved;
        maxRepFreethinkers = GameObject.FindGameObjectWithTag("Player").GetComponent<Reputation>().FreidenkerMaxAchieved;
        UpdateQuestText();

        if (maxRepImperialists)
            UpdateQuestStatus();
	}

    private void UpdateQuestText()
    {
        questName.GetComponent<TextMeshProUGUI>().SetText("Achieve maximum Reputation with the Imperialists (" + currentRepImperialists + "/" + maxRep + ")");
    }

    private void UpdateQuestStatus()
    {
        checkMark.SetActive(true);
    }
}
