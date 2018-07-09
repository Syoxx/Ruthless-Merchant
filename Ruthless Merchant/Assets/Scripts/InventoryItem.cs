using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script permits setting information of an inventory UI object
/// </summary>
public class InventoryItem : MonoBehaviour
{
    public enum UILocation
    {
        Unspecified,
        Inventory,
        ExternalList
    }

    public UILocation itemLocation;

    public Image ItemImage;

    public Text itemQuantity;

    public Text itemName;

    public Text itemRarity;

    public Text itemDescription;

    public Text itemPrice;

    public Text itemWeight; 
}
