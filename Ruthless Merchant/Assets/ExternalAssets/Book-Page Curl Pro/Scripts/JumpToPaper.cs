using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToPaper : MonoBehaviour
{
    private BookPro _currentPage;

    //[SerializeField] private int JumpToPage;
	// Use this for initialization
	void Start ()
	{
	    _currentPage = GetComponent<BookPro>();
	}
	
	// Hardcodded for Build. Will be changed in the future
	public void SwitchToMenu ()
	{
	    _currentPage.CurrentPaper = 1;
	}

    public void SwitchToInventar()
    {
        _currentPage.CurrentPaper = 2;
    }
 }

