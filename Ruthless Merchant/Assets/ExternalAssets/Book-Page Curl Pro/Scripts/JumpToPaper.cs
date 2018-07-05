using System.Collections;
using System.Collections.Generic;
using RuthlessMerchant;
using UnityEngine;

public class JumpToPaper : MonoBehaviour
{
    private BookPro _myBook;
    private AutoFlip flipEffect;
    [SerializeField][Tooltip("Just put the canvas here itself")]
    private GameObject BookItSelf;
    

    //Collecting pages
    public List<GameObject> _pagesList = new List<GameObject>();

    private GameObject blub;

   

    //Needed for helping to count Items
    private int ItemsAmount;


    //[SerializeField] private int JumpToPage;
	// Use this for initialization
	void Start ()
	{
	    _myBook = GetComponent<BookPro>();
        flipEffect = GetComponent<AutoFlip>();
        Debug.Log("Start Launched");
	}

    void Update()
    {

        BookControlling();
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
        //Debug.Log("Alo" + Mathf.Floor(CountWeaponsInInventory()  / GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage));
        return (CountWeaponsInInventory() / GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage);
    }

  
	// Hardcodded for Build. Will be changed in the future
	public void SwitchToMenu ()
	{       
	    _myBook.CurrentPaper = 2;       
	}

    public void SwitchToInventar()
    {
        _myBook.CurrentPaper = 1;
    }

    public void StartingPage()
    {
        if (Player.lastKeyPressed == KeyCode.I)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Player.lastKeyPressed == KeyCode.J)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Player.lastKeyPressed == KeyCode.N)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Player.lastKeyPressed == KeyCode.Escape)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

    }

    //Flipping pages with MouseClick
    private void BookControlling()
    {
        //Mouse Control - Flip effect with LMB/RMB
        //
        //TODO
        //Currently turned off due of the missing navigation with W/A/S/D. Just uncomment it when navigation is done
        //
        //

        //if (Input.GetMouseButtonDown(0))
        //{
        //    flipEffect.FlipRightPage();
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    flipEffect.FlipLeftPage();
        //}


        //Buttons Control to jump to certain points
        if (Input.GetKeyDown(KeyCode.I) || Player.lastKeyPressed == KeyCode.I)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Input.GetKeyDown(KeyCode.J) || Player.lastKeyPressed == KeyCode.J)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BookItSelf.SetActive(BookItSelf.activeSelf == false);
        }

        if (Player.lastKeyPressed == KeyCode.Escape)
        {
            _myBook.CurrentPaper = 2;
            Player.lastKeyPressed = KeyCode.None;
        }
        if (Input.GetKeyDown(KeyCode.N) || Player.lastKeyPressed == KeyCode.N)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

    }

 }

