using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestPanel : MonoBehaviour {

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;

    public GameObject questPanelPrefab;
    public PageLogic BookLogic = null;

    public void CreateQuestPanel(string name, string description)
    {
        Transform parent = BookLogic.InventoryPageList[BookLogic.PageForCurrentItemPlacement()].transform.Find("Panel");


        Name.text = name;
        Description.text = description;
    }
}
