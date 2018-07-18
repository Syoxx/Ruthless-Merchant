using System.Collections;
using System.Collections.Generic;
using RuthlessMerchant;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class PageLogic : MonoBehaviour
{
    #region Private Fields

    private BookPro myBook;
    private AutoFlip flipEffect;

    //Needed for helping to count Items
    private int ItemsAmount;
    private int numberPagesSkipped;
    private int maxWeaponsPerPage;

    [SerializeField]
    [Tooltip("Just put the canvas here itself")]
    private GameObject bookItSelf;

    private int flippedPages = 0;

    [Header("Bookmark book buttons")]
    [SerializeField]
    private Button btn_Notice;
    [SerializeField]
    private Button btn_Journal, btn_Inventory, btn_Recipes, btn_Menu;

    private bool flipToTheLeft;



    #endregion

    //Collecting pages
    [HideInInspector] public List<GameObject> InventoryPageList = new List<GameObject>();

    [HideInInspector] public List<GameObject> PageList = new List<GameObject>();

    private enum BookSection
    {
        FrontCover,
        TitlePage,
        NoticePage,
        JournalPage,
        InventoryPage,
        RecipePage,
        MenuPage,
        BackCover,
    }

    private BookSection myBookSection;
    // Use this for initialization
    void Start()
    {
        myBook = GetComponent<BookPro>();
        flipEffect = GetComponent<AutoFlip>();
    }

    void Update()
    {
        BookControlling();
        HighlightBookmarkButtons();
        CurrentActivePage();
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
            InventoryItem[] data = myObject.GetComponentsInChildren<InventoryItem>();
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

    /// <summary>
    /// Decide what sort of a page is currently active
    /// </summary>
    private void CurrentActivePage()
    {
        if (myBook.currentPaper == 0)
        {
            myBookSection = BookSection.FrontCover;
        }
        if (myBook.currentPaper == 1)
        {
            myBookSection = BookSection.TitlePage;
        }
        if (2 <= myBook.currentPaper && myBook.currentPaper <= 6)
        {
            myBookSection = BookSection.NoticePage;
        }
        if (7 <= myBook.currentPaper && myBook.currentPaper <= 9)
        {
            myBookSection = BookSection.JournalPage;
        }
        if (10 <= myBook.currentPaper && myBook.currentPaper <= 15)
        {
            myBookSection = BookSection.InventoryPage;
        }
        if (16 <= myBook.currentPaper && myBook.currentPaper <= 18)
        {
            myBookSection = BookSection.RecipePage;
        }
        if (19 <= myBook.currentPaper && myBook.currentPaper <= 20)
        {
            myBookSection = BookSection.MenuPage;
        }
        if (myBook.currentPaper == 21)
        {
            myBookSection = BookSection.BackCover;
        }

    }

    public void SwitchToCertainPages(int n)
    {
        flipEffect.PageFlipTime = 0.1f;
        StartCoroutine(FlipPageDelayed(n));
    }
    /// <summary>
    /// Highlighting bookmarks of the Book if player is in the right section
    /// </summary>
    private void HighlightBookmarkButtons()
    {
        switch (myBookSection)
        {
            case BookSection.InventoryPage:
                btn_Inventory.Select();
                break;
            case BookSection.JournalPage:
                btn_Journal.Select();
                break;
            case BookSection.MenuPage:
                btn_Menu.Select();
                break;
            case BookSection.NoticePage:
                btn_Notice.Select();
                break;
            case BookSection.RecipePage:
                btn_Recipes.Select();
                break;

        }
    }

    IEnumerator FlipPageDelayed(int n)
    {
        if (!flipToTheLeft)
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
        else
        {
            flipEffect.FlipLeftPage();
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


    /*
     *
     *Book - Control Functions for the Button and more
     *
     */
    public void OpenInventory()
    {
        int neededPage = 10;
        CheckPageLocation(neededPage);

    }

    public void OpenJournal()
    {
        int neededPage = 7;
        CheckPageLocation(neededPage);

    }

    public void OpenRecipes()
    {
        int neededPage = 17;
        CheckPageLocation(neededPage);
    }

    public void OpenMenu()
    {
        int neededPage = 19;
        CheckPageLocation(neededPage);
    }

    public void OpenNotices()
    {
        int neededPage = 2;
        CheckPageLocation(neededPage);
    }

    private void CheckPageLocation(int neededPage)
    {
        int timesToFlip = myBook.CurrentPaper - neededPage;
        if (timesToFlip < 0)
        {
            flipToTheLeft = false;
            timesToFlip = timesToFlip * -1;
            SwitchToCertainPages(timesToFlip);
        }
        else
        {
            flipToTheLeft = true;
            SwitchToCertainPages(timesToFlip);
        }
    }
}

