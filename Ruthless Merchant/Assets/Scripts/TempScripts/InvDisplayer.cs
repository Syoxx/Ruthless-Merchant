using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class InvDisplayer : MonoBehaviour
    {

        [SerializeField]
        GameObject panelPrefab;

        Inventory inventory;

        // Use this for initialization
        void Start()
        {
            inventory = FindObjectOfType<Inventory>();
            inventory.InventoryChanged.AddListener(UpdateInv);
        }

        void UpdateInv()
        {
            for(int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                Debug.Log(transform.childCount);
                if(transform.childCount > 0)
                    Destroy(transform.GetChild(i).gameObject);
            }

            for(int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                GameObject newPanel = Instantiate(panelPrefab, transform);
                if (inventory.inventorySlots[i].Count <= 0)
                {
                    newPanel.GetComponentInChildren<Text>().text = "Empty";
                }
                else
                {
                    newPanel.GetComponentInChildren<Text>().text = inventory.inventorySlots[i].Item.gameObject.name + " - " + inventory.inventorySlots[i].Count;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
