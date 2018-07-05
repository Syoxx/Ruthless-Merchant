using System.Collections;
using System.Collections.Generic;
using RuthlessMerchant;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;

public class JumpToPaper : MonoBehaviour
{
    private BookPro _myBook;
    private AutoFlip flipEffect;
    [SerializeField]
    [Tooltip("Just put the canvas here itself")]
    private GameObject BookItSelf;
    private Button FirstMenuButton;
    private Transform FirstInventoryPanel;

    private int flippedPages = 0;


    //Collecting pages
    [HideInInspector] public List<GameObject> InventoryPageList = new List<GameObject>();

    [HideInInspector] public List<GameObject> PageList = new List<GameObject>();

    private GameObject blub;



    //Needed for helping to count Items
    private int ItemsAmount;
    private int numberPagesSkipped;


    //[SerializeField] private int JumpToPage;
    // Use this for initialization
    void Start()
    {
        _myBook = GetComponent<BookPro>();
        flipEffect = GetComponent<AutoFlip>();
        Transform PauseObject = BookItSelf.transform.GetChild(0);
        
        // For finding the first selected button on inventory page
        Transform InventoryPage = PauseObject.GetChild(0);
        FirstInventoryPanel = InventoryPage.GetChild(1);

        // For finding first button of pause page
        Transform Pauseprefab = null;
        foreach (Transform child in PauseObject)
        {
            if (child.gameObject.name == "Page4")
            {
                Pauseprefab = child.GetChild(0);
                break;
            }
            continue;
        }
        
        Transform PausePanel = Pauseprefab.GetChild(0);
        Transform PausemenuParent = PausePanel.GetChild(0);
        FirstMenuButton = PausemenuParent.GetChild(0).gameObject.GetComponent<Button>(); // 1st pause menu button
    }

    void Update()
    {

        BookControlling();
    }

    public void GeneratePages()
    {
        InventoryPageList = GameObject.FindGameObjectsWithTag("Book_Inventory")
            .OrderBy(inventoryPage => inventoryPage.name).ToList();

        GameObject Pages = GameObject.Find("Pages");
        foreach (Transform child in Pages.transform)
        {
            // Debug.Log("M " + child);
        }

        PageList = PageList.OrderBy(child => child.name).ToList();
        foreach (var o in PageList)
        {
            //Debug.Log("MyObjectName " + o.name);
        }
    }

    public void CheckIfNextPageEmpty()
    {
        int myCurrentPage = _myBook.currentPaper * 2 + 1;
        while (myCurrentPage <= PageList.Count())
        {
            if (PageList[myCurrentPage].GetComponent<PageCheck>().PageIsFilled != true ||
                PageList[myCurrentPage].GetComponent<PageCheck>().StartingPage != true)
            {
                numberPagesSkipped++;
                myCurrentPage++;
            }

            else
            {
                break;
            }
        }
         Debug.Log("Next page " + myCurrentPage );

    // Here we decide for the Page where should be the weapon placed.
    // CountWeaponsInInvetory = current itemsAmount, _maxWeaponsPerPage (adjusted at Player prefab). -1 stands for a case n/m where n=m.
    public int pageForCurrentWeaponPlacement()
    {
        //Debug.Log("Alo" + Mathf.Floor(CountWeaponsInInventory()  / GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage));
        return (CountWeaponsInInventory() / GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage);
    }


    // Hardcodded for Build. Will be changed in the future
    public void SwitchToMenu()
    {
        _myBook.CurrentPaper = 2;
    }

    public void SwitchToInventar()
    {
        int myCurrentPage = _myBook.currentPaper * 2 + 1;
        while (myCurrentPage <= PageList.Count())
        {
            if (PageList[myCurrentPage].GetComponent<PageCheck>().PageIsFilled != true ||
                PageList[myCurrentPage].GetComponent<PageCheck>().StartingPage != true)
            {
                numberPagesSkipped++;
                myCurrentPage--;
            }

            else
            {
                break;
            }
        }
}

    private int CountWeaponsInInventory()
    {
        int itemsAmount = 0;
        foreach (GameObject myObject in InventoryPageList)
        {
            InventoryDisplayedData[] data = myObject.GetComponentsInChildren<InventoryDisplayedData>();
            if (data != null)
            {
                itemsAmount += data.Length;
            }
        }

        return itemsAmount;
    }

    // Here we decide for the Page where should be the weapon placed.
    public int PageForCurrentWeaponPlacement()
    {
        return (CountWeaponsInInventory() /
                GameObject.Find("NewPlayerPrefab").GetComponent<Player>()._maxWeaponsPerPage);
    }


    // Hardcodded for Build. Will be changed in the future
   // public void SwitchToMenu()
   // {
   //     _myBook.CurrentPaper = 2;
   // }
   //
   // public void SwitchToInventar()
   // {
   //     _myBook.CurrentPaper = 1;
   // }

 // public void StartingPage()
 // {
 //     if (Player.lastKeyPressed == KeyCode.I)
 //     {
 //         _myBook.CurrentPaper = 9;
 //         Player.lastKeyPressed = KeyCode.None;
 //     }
 //
 //     if (Player.lastKeyPressed == KeyCode.J)
 //     {
 //         _myBook.CurrentPaper = 6;
 //         Player.lastKeyPressed = KeyCode.None;
 //     }
 //
 //     if (Player.lastKeyPressed == KeyCode.N)
 //     {
 //         _myBook.CurrentPaper = 1;
 //         Player.lastKeyPressed = KeyCode.None;
 //     }
 //
 //     if (Player.lastKeyPressed == KeyCode.Escape)
 //     {
 //         _myBook.CurrentPaper = 19;
 //         Player.lastKeyPressed = KeyCode.None;
 //         
 //     }
 //
 // }

    //Flipping pages with MouseClick
    private void BookControlling()
    {
        //Mouse Control - Flip effect with LMB/RMB
        //
        //TODO
        //Currently turned off due of the missing navigation with W/A/S/D. Just uncomment it when navigation is done
        //
        //

        if (Input.GetMouseButtonDown(0))
        {
            flipEffect.FlipRightPage();
        }
        if (Input.GetMouseButtonDown(1))
        {
            flipEffect.FlipLeftPage();
        }


        //Buttons Control to jump to certain points
        if (Input.GetKeyDown(KeyCode.I) || Player.lastKeyPressed == KeyCode.I)
        {
            _myBook.CurrentPaper = 10;
            Player.lastKeyPressed = KeyCode.None;

            Button FirstInventoryButton = null;
            if (FirstInventoryPanel.gameObject.GetComponentInChildren<Button>() != null)
            {
                FirstInventoryButton = FirstInventoryPanel.gameObject.GetComponentInChildren<Button>();
            }
            if (FirstInventoryButton != null)
            {
                EventSystem.current.SetSelectedGameObject(FirstInventoryButton.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.J) || Player.lastKeyPressed == KeyCode.J)
        {
            _myBook.CurrentPaper = 7;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BookItSelf.SetActive(BookItSelf.activeSelf == false);
        }

        if (Player.lastKeyPressed == KeyCode.Escape)
        {
            _myBook.CurrentPaper = 19;
            Player.lastKeyPressed = KeyCode.None;
            
            EventSystem.current.SetSelectedGameObject(FirstMenuButton.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.N) || Player.lastKeyPressed == KeyCode.N)
        {
            _myBook.CurrentPaper = 1;
            Player.lastKeyPressed = KeyCode.None;
        }

    }

    public void SwitchToCertainPages(int n)
    {
        flipEffect.PageFlipTime = 0.1f;  
        StartCoroutine(FlipPageDelayed(n));
    }

    

    IEnumerator FlipPageDelayed(int n)
    {
        flipEffect.FlipRightPage();
        flippedPages++;

        if (flippedPages < n)
        {
            yield return new WaitForSeconds(flipEffect.PageFlipTime);

            StartCoroutine(FlipPageDelayed(n));
        }
        else
        {
            flippedPages = 0;
            flipEffect.PageFlipTime = 1f;
        }
    }
}

