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
    public int pageForCurrentWeaponPlacement()
    {
      // Debug.Log("Alo" + Mathf.Floor((CountWeaponsInInventory() - 2) / GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage));
        return (int) Mathf.Floor((CountWeaponsInInventory()  - 2)/ GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage);
    }

  
	// Hardcodded for Build. Will be changed in the future
	public void SwitchToMenu ()
	{
        
		Debug.Log ("I'm here");
	    _myBook.CurrentPaper = 2;

        
	}

    public void SwitchToInventar()
    {
        _myBook.CurrentPaper = 1;
    }


 }

