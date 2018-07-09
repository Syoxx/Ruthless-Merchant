using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RuthlessMerchant
{
    public class QuestTextManager : MonoBehaviour
    {
        private string questDialogue;
        [SerializeField][Tooltip("Drag a wood Prefab there")]
        private Item woodPrefab;
        [SerializeField] [Tooltip("Drag a iron Prefab there")]
        private Item ironPrefab;

        private int woodCounter;
        private int ironCounter;
        Player player;
        private TextMeshProUGUI textMesh;
        void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>();

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        }

        // Update is called once per frame
        void Update()
        {

            woodCounter = player.Inventory.GetNumberOfItems(woodPrefab);
            ironCounter = player.Inventory.GetNumberOfItems(ironPrefab);

            questDialogue = "1.    Waffe aufsammeln \n" +
               "2.    Den Wald besuchen\n" +
               "3.    Holz sammeln (" + woodCounter + "/5)\n" +
               "4.    Iron sammeln (" + ironCounter + "/5)\n" +
               "5.    Alte Waffe zerlegen\n" +
               "6.    Crafting aus Eisen + Holz = neue Waffe\n" +
               "7.    Mit einem Händler handeln";

            textMesh.text = questDialogue.ToString();
        }
    }
}
