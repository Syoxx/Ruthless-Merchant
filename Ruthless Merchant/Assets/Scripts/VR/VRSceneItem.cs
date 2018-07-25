using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSceneItem : MonoBehaviour
{
    public VRItem Item;

    public Transform WeightParent;
    
    [SerializeField]
    string itemName;

    [SerializeField]
    int value;

    void Awake()
    {
        Item.ItemName = itemName;
        Item.Value = value;
    }
}
