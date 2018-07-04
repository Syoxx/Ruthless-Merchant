using System.Collections;
using System.Collections.Generic;
using RuthlessMerchant;
using UnityEngine;

public class JumpToPaper : MonoBehaviour
{
    private BookPro _myBook;
    private AutoFlip _flipEffect;

    //Collecting pages
    public List<GameObject> _pagesList = new List<GameObject>();

    private GameObject blub;

   

    //Needed forhelping to count Items
    private int ItemsAmount;


    //[SerializeField] private int JumpToPage;
	// Use this for initialization
	void Start ()
	{
	    _myBook = GetComponent<BookPro>();          
	}

    public void GeneratePages()
    {
        foreach (GameObject inventoryPage in GameObject.FindGameObjectsWithTag("Book_Inventory"))
        {
            _pagesList.Add(inventoryPage);
        }
    }

	//Count the Childs in PNL_ZoneForItem (Located in Page 1 and Page 2 as a child). The counter == The items in _pagesList 
	//
	//
	//This shit is broken.
	//
	//An Marcel - Hier wird der falscher "itemsAmout" ausgespückt. Somit kann leider die Funktion "pageForCurrentWeaponPlacement" nicht funktionieren. 
	//Die Aufgabe ist - die Items von der pagesList zu zählen. Diese werden als "PNL_ItemZone" unter "Page1/2" ->  "PNL_ZoneForItem" als prefab hergestellt (Beim aufnahme eines Items)
	//Wenn du Fragen bezüglich den Items hast, sollst du an Richard wenden.
	//Falls du eine bessere Idee hast, kannst du es auch gerne deine Lösung schreiben.
	//P.S. Dieser Code funktioniert auch, wenn das Buch Deactivated in der Scene ist ABER es muss in Inspector aktiviert sein. Später werde ich die bestimmte Funktionen einfach an bestimmte Stellen aufrufen um das zu vermeiden.
	//Also, im Prinzip, du kannst das Buch wieder zuschließen um Items aufzuheben ("J") aber wie gesagt, bei "default" lass es an (Du startest mit geöffnetes Buch)
    private int CountWeaponsInInventory()
    {
        int itemsAmount = 0;
        foreach (GameObject myObject in _pagesList)
        { Debug.Log("name" + myObject.name);
            InventoryDisplayedData[] data = myObject.GetComponentsInChildren<InventoryDisplayedData>();
            //Transform t = myObject.transform.Find("PNL_ZoneForItem");
            if (data != null)
            {
                itemsAmount += data.Length;
                Debug.Log("count" + data.Length);
            }

        }
        Debug.Log("Items" + itemsAmount);
        return itemsAmount;
    }

	// Here we decide for the Page where should be the weapon placed.
	// CountWeaponsInInvetory = current itemsAmount, _maxWeaponsPerPage (adjusted at Player prefab). -1 stands for a case n/m where n=m.
    public int pageForCurrentWeaponPlacement()
    {
      // Debug.Log("Alo" + Mathf.Floor((CountWeaponsInInventory() - 2) / GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage));
        return (int) Mathf.Floor((CountWeaponsInInventory()  - 1)/ GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage);
    }

  
	// Hardcodded for Build. Will be changed in the future
	public void SwitchToMenu ()
	{
        
		//Debug.Log ("I'm here");
	    _myBook.CurrentPaper = 2;

        
	}

    public void SwitchToInventar()
    {
        _myBook.CurrentPaper = 1;
    }


 }

