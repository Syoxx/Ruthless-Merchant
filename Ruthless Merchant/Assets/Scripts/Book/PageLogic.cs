using System.Collections;
using System.Collections.Generic;
using RuthlessMerchant;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Includes almost the whole selfmade logic for the book, that were not in the asset
/// </summary>
public class PageLogic : MonoBehaviour
{
    #region Private Fields

    private BookPro myBook;
    private AutoFlip flipEffect;

    //Needed for helping to count Items
    private int ItemsAmount;
    private int numberPagesSkipped;
    private int maxWeaponsPerPage;

    [SerializeField] [Tooltip("Just put the canvas here itself")]
    private GameObject bookItSelf;

    private int flippedPages = 0;

    [Header("Bookmark book buttons")] [SerializeField]
    private Button btn_Notice;

    [SerializeField] private Button btn_Journal, btn_Inventory, btn_Recipes, btn_Menu;

    private bool flipToTheLeft;
    private bool couritineIsFinished;

    #endregion

    //Collecting pages
    [HideInInspector] public List<GameObject> InventoryPageList = new List<GameObject>();

    [HideInInspector] public List<GameObject> RecipePageList = new List<GameObject>();

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

    private void Awake()
    {
        myBook = GetComponent<BookPro>();
        flipEffect = GetComponent<AutoFlip>();
    }

    void Start()
    {
        couritineIsFinished = true;
    }

    void Update()
    {
        HighlightBookmarkButtons();
        CurrentActivePage();
    }

    /// <summary>
    /// Generating an Array of all "Inventory" pages
    /// </summary>
    public void GeneratePages()
    {
        InventoryPageList = GameObject.FindGameObjectsWithTag("Book_Inventory")
            .OrderBy(inventoryPage => inventoryPage.name).ToList();
        PageList = PageList.OrderBy(child => child.name).ToList();
    }

    /// <summary>
    /// Checking if the next page of the book is empty
    /// </summary>
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

    /// <summary>
    /// Checking if the previous page of the book is empty
    /// </summary>
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

    /// <summary>
    /// Counts how much Items you have in your inventory
    /// </summary>
    /// <returns>Returns an integer amount of the your items</returns>
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
    /// Jumps to the needed page of the book by clicking a Button
    /// </summary>
    /// <param name="key">Key is the pressed button</param>
    public void GoToPage(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.I:
                myBook.CurrentPaper = 10;
                OpenInventory();
                break;
            case KeyCode.J:
                myBook.CurrentPaper = 7;
                OpenJournal();
                break;
            case KeyCode.N:
                myBook.CurrentPaper = 2;
                OpenNotices();
                break;
            case KeyCode.R:
                myBook.CurrentPaper = 16;
                OpenRecipes();
                break;
            case KeyCode.Escape:
                myBook.CurrentPaper = 19;
                OpenMenu();
                break;
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

    
    /// <summary>
    /// Method to jump to a need page while book is open
    /// </summary>
    /// <param name="n">Is number of the page we are going to jump</param>
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
    /// <summary>
    /// Don't touch it. A delay for a page. Needed for an animation while flipping pages
    /// </summary>
    /// <param name="n">Is a amount of how many pages are going to be skipped</param>
    /// <returns>Checking if we've reached the target</returns>
    IEnumerator FlipPageDelayed(int n)
    {
        if (!flipToTheLeft)
        {
            flipEffect.FlipRightPage();
            flippedPages++;

            if (flippedPages < n)
            {
                yield return new WaitForSeconds(flipEffect.PageFlipTime);
                couritineIsFinished = false;
                StartCoroutine(FlipPageDelayed(n));
            }
            else
            {
                flippedPages = 0;
                flipEffect.PageFlipTime = 1f;
                couritineIsFinished = true;
            }
        }
        else
        {
            flipEffect.FlipLeftPage();
            flippedPages++;

            if (flippedPages < n)
            {
                yield return new WaitForSeconds(flipEffect.PageFlipTime);

                couritineIsFinished = false;
                StartCoroutine(FlipPageDelayed(n));
            }
            else
            {
                flippedPages = 0;
                flipEffect.PageFlipTime = 1f;
                couritineIsFinished = true;
            }
        }
    }

    #region Book - Control Functions for the Buttons
    /// <summary>
    /// Opens Inventory
    /// </summary>
    public void OpenInventory()
    {
        if (couritineIsFinished)
        {
            int neededPage = 10;
            CheckPageLocation(neededPage);
        }
    }
    /// <summary>
    /// Opens Journal
    /// </summary>
    public void OpenJournal()
    {
        if (couritineIsFinished)
        {
            int neededPage = 7;
            CheckPageLocation(neededPage);
        }
    }
    /// <summary>
    /// Opens Recipes
    /// </summary>
    public void OpenRecipes()
    {
        if (couritineIsFinished)
        {
            int neededPage = 16;
            CheckPageLocation(neededPage);
        }
    }
    /// <summary>
    /// Opens Menu
    /// </summary>
    public void OpenMenu()
    {
        if (couritineIsFinished)
        {
            int neededPage = 19;
            CheckPageLocation(neededPage);
        }
    }
    /// <summary>
    /// Opens Notices
    /// </summary>
    public void OpenNotices()
    {
        if (couritineIsFinished)
        {
            int neededPage = 2;
            CheckPageLocation(neededPage);
        }
    }
    /// <summary>
    /// Opens Settings
    /// </summary>
    public void OpenSettings()
    {
        if (couritineIsFinished)
        {
            int neededPage = 20;
            CheckPageLocation(neededPage);
        }
    }
    /// <summary>
    /// Checking for the page location we are going to jump while book is open
    /// Also there is a check if we are going to jump to the left/right direction
    /// </summary>
    /// <param name="neededPage">The page we are looking for</param>
    private void CheckPageLocation(int neededPage)
    {
        int timesToFlip = myBook.CurrentPaper - neededPage;
        if (timesToFlip < 0)
        {
            flipToTheLeft = false;
            timesToFlip = timesToFlip * -1;
            SwitchToCertainPages(timesToFlip);
        }
        else if (timesToFlip > 0)
        {
            flipToTheLeft = true;
            SwitchToCertainPages(timesToFlip);
        }
    }
   #endregion

}