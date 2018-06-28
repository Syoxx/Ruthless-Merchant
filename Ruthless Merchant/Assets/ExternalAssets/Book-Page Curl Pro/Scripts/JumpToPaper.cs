using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToPaper : MonoBehaviour
{
    private BookPro _currentPage;

    [SerializeField] private int JumpToPage;
	// Use this for initialization
	void Start ()
	{
	    _currentPage = GetComponent<BookPro>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.anyKeyDown)
	        _currentPage.CurrentPaper = JumpToPage;
	}
}
