using System.Collections;
using System.Collections.Generic;
using RuthlessMerchant;
using UnityEngine;
using System.Linq;

public class PageLogic : MonoBehaviour
{
    #region Private Fields

    private BookPro myBook;
    private AutoFlip flipEffect;

    [SerializeField]
    [Tooltip("Just put the canvas here itself")]
    private GameObject bookItSelf;

    private int flippedPages = 0;

    #endregion

    //Collecting pages
    [HideInInspector] public List<GameObject> InventoryPageList = new List<GameObject>();

    [HideInInspector] public List<GameObject> PageList = new List<GameObject>();

    //Needed for helping to count Items
    int ItemsAmount;
    int numberPagesSkipped;
    int maxWeaponsPerPage;

    // Use this for initialization
    void Start()
    {
        myBook = GetComponent<BookPro>();
        flipEffect = GetComponent<AutoFlip>();
    }

    void Update()
    {

        BookControlling();
    }

    public void GeneratePages()
    {
        InventoryPageList = GameObject.FindGameObjectsWithTag("Book_Inventory")
            .OrderBy(inventoryPage => inventoryPage.name).ToList();
        PageList = PageList.OrderBy(child => child.name).ToList();
    }

    public void CheckIfNextPageEmpty()
    {
        int myCurrentPage = myBook.currentPaper * 2 + 1;
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
    }

    public void CheckIfPreviousPageEmpty()
    {
        int myCurrentPage = myBook.currentPaper * 2 + 1;
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
            SlotDisplay[] data = myObject.GetComponentsInChildren<SlotDisplay>();
            if (data != null)
            {
                itemsAmount += data.Length;
            }
        }

        return itemsAmount;
    }
    /// <summary>
    ///  Here we decide for the Page where should be the Item placed.
    /// </summary>
    /// <returns>Page for Item</returns>
    public int PageForCurrentItemPlacement()
    {
        maxWeaponsPerPage = GameObject.Find("NewPlayerPrefab").GetComponent<Player>().MaxItemsPerPage;
        if (maxWeaponsPerPage == 0)
            maxWeaponsPerPage = 1;
        return (CountWeaponsInInventory() /
                maxWeaponsPerPage);
    }

    /// <summary>
    /// BookControlling for pages flip
    /// </summary>
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
            myBook.CurrentPaper = 10;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Input.GetKeyDown(KeyCode.J) || Player.lastKeyPressed == KeyCode.J)
        {
            myBook.CurrentPaper = 7;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bookItSelf.SetActive(bookItSelf.activeSelf == false);
        }

        if (Player.lastKeyPressed == KeyCode.Escape)
        {
            myBook.CurrentPaper = 19;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Input.GetKeyDown(KeyCode.N) || Player.lastKeyPressed == KeyCode.N)
        {
            myBook.CurrentPaper = 2;
            Player.lastKeyPressed = KeyCode.None;
        }

        if (Input.GetKeyDown(KeyCode.R) || Player.lastKeyPressed == KeyCode.R)
        {
            myBook.CurrentPaper = 17;
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

