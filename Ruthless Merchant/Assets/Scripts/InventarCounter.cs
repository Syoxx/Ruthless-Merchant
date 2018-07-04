using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarCounter : MonoBehaviour
{

    [SerializeField] private int MaxItemsPerPage;
    [SerializeField] private GameObject bookObject;

    public CurrentPage currentPage = CurrentPage.zero;

    public enum CurrentPage
    {
        zero,
        one,
        two,
        four
    }

	// Update is called once per frame
	void Update () {

	    if (MaxItemsPerPage <= transform.childCount)
	    {
	        currentPage++;
	    }
	}


}
