using UnityEngine;


/// <summary>
/// NOT USED CURRENTLY
/// </summary>
public class PageCheck : MonoBehaviour
{
    [HideInInspector]
    public bool PageIsFilled;

    [SerializeField]
    public bool StartingPage;

    public enum PageType { Notice, Journal, Inventory, Recipes, Menu };

    public PageType pageType;

    private GameObject inventoryCheck;

    // Use this for initialization

    // Update is called once per frame
    void Update ()
    {
        if (!StartingPage)
        {
            switch (pageType)
            {
                case PageType.Inventory:
                    if(GameObject.Find("PNL_ItemZone(Clone)") != null)
                    { 
                        PageIsFilled = true;                      
                    }
                    break;
                case PageType.Journal:
                    break;
                case PageType.Menu:
                    break;
                case PageType.Recipes:
                    break;
            }
        }
    }
}
