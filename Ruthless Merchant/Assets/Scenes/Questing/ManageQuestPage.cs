using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageQuestPage : MonoBehaviour {

    public void DisableAllButtons()
    {
        Debug.Log("Disabled all buttons");
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        GetComponentInChildren<GameObject>().gameObject.SetActive(false);
    }
}
