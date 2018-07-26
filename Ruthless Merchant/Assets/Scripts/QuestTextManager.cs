using UnityEngine;
using TMPro;

namespace RuthlessMerchant
{
    public class QuestTextManager : MonoBehaviour
    {
        #region Serialize Fields
        [SerializeField][Tooltip("Drag a wood Prefab there")]
        private Item woodPrefab;
        [SerializeField] [Tooltip("Drag a iron Prefab there")]
        private Item ironPrefab;
        #endregion

        #region Private Fields
        private string questDialogue;
        private int woodCounter;
        private int ironCounter;
        private TextMeshProUGUI textMesh;
        private Player player;
        #endregion

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
            questDialogue =
               "1.    Den Wald besuchen\n" +
               "2.    Holz sammeln (" + woodCounter + "/5)\n" +
               "3.    Eisen sammeln (" + ironCounter + "/5)\n" +
               "4.    Crafting aus Eisen + Holz = neue Waffe\n" +
               "5.    Waffe an der Workbench zerlegen oder\n" +
               "6.    Mit einem Händler handeln\n" +
               "7.    Sammle mind. 3 Trankzutaten\n" +
               "8.    Verarbeite die Zutaten beim Alchemisten zu einem Trank";
            textMesh.text = questDialogue.ToString();
        }
    }
}
