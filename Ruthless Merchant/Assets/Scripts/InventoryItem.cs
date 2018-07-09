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

    public TextMeshProUGUI itemQuantity;

    public TextMeshProUGUI itemName;

    public TextMeshProUGUI itemRarity;

    public TextMeshProUGUI itemDescription;

    public TextMeshProUGUI itemPrice;

    public TextMeshProUGUI itemWeight; 
}
