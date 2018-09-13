using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace RuthlessMerchant {
    public class QuestButton : MonoBehaviour {

        public bool inProgress;
        public bool isDisabled;

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

        public void DiscardQuestButton(int Reward, List<Collectables> itemlist)
        {
            Debug.Log("Discard");
            Button btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { DiscardQuest(Reward, itemlist); });
        }
        private void DiscardQuest(int Reward, List<Collectables> itemlist)
        {
            Debug.Log("Destroy"+ this.gameObject);
            Destroy(gameObject);

            for (int i = 0; i < itemlist.Count; i++)
            {
                Player.Singleton.Inventory.Add(itemlist[i].item, itemlist[i].requiredAmount, true);
            }

            Player.Singleton.Inventory.RemoveGold(Reward);
        }

        public void GreyOut()
        {
            Debug.Log("greying out");
            var colors = GetComponent<Button>().colors;
            colors.normalColor = new Color32(102, 102, 102, 255);
            colors.highlightedColor = new Color32(102, 102, 102, 255);
            GetComponent<Button>().colors = colors;
            isDisabled = true;
        }

        public void DefaultColor()
        {
            var colors = GetComponent<Button>().colors;
            colors.normalColor = new Color32(255, 255, 255, 255);
            colors.highlightedColor = new Color32(255, 255, 255, 255);
            GetComponent<Button>().colors = colors;
            isDisabled = false;
        }
    }
}
