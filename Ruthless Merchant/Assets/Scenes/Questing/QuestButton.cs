using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace RuthlessMerchant {
    public class QuestButton : MonoBehaviour {

        public bool inProgress;

        public void CompleteButton()
        {
            Debug.Log("Color changed");
                    var colors = GetComponent<Button>().colors;
                    colors.normalColor = new Color32(171,243, 116, 255);
                    colors.highlightedColor = new Color32(171, 243, 116, 255);
                    GetComponent<Button>().colors = colors;
        }

        public void InProgressButton()
        {
            Debug.Log("InProgress");
            var colors = GetComponent<Button>().colors;
            colors.normalColor = new Color32(179, 239, 250, 255);
            colors.highlightedColor = new Color32(179, 239, 250, 255);
            GetComponent<Button>().colors = colors;
            inProgress = true;
        }

        public void ButtonSettings(bool interactable)
        {
            GetComponent<Button>().interactable = interactable;
        }

        public void SetDisplayedData(string name)
        {
            QuestDisplayedData info = GetComponent<QuestDisplayedData>();
            info.Name.text = name;
        }

        public void DiscardQuestButton()
        {
            Debug.Log("Discard");
            Button btn = GetComponent<Button>();
            
            btn.onClick.AddListener(delegate { DiscardQuest(); });
        }
        private void DiscardQuest()
        {
            Debug.Log("Destroy"+ this.gameObject);
            Destroy(gameObject);
        }
    }
}
