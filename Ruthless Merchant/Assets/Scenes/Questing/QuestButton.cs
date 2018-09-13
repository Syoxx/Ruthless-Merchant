using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace RuthlessMerchant {
    public class QuestButton : MonoBehaviour {
        //script to make visual adjustments and enabling/disabling the questButtons


        public bool inProgress;
        public bool isDisabled;

        //Sets the button in a completed state with changed color
        public void CompleteButton()
        {
            Debug.Log("Color changed");
                    var colors = GetComponent<Button>().colors;
                    colors.normalColor = new Color32(171,243, 116, 255);
                    colors.highlightedColor = new Color32(171, 243, 116, 255);
                    GetComponent<Button>().colors = colors;
        }

        //Sets the button in a still-in-progress state with changed color
        public void InProgressButton()
        {
            Debug.Log("InProgress");
            var colors = GetComponent<Button>().colors;
            colors.normalColor = new Color32(179, 239, 250, 255);
            colors.highlightedColor = new Color32(179, 239, 250, 255);
            GetComponent<Button>().colors = colors;
            inProgress = true;
        }

        //Removes previous button listener and adds a listener to discard button when completed
        public void DiscardQuestButton(int Reward)
        {
            Button btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { DiscardQuest(Reward); });
        }

        //Discrads the button and gives out the reward money 
        private void DiscardQuest(int Reward)
        {
            Destroy(gameObject);
            Player.Singleton.Inventory.RemoveGold(Reward);
        }

        //Greys out and disables button
        public void GreyOut()
        {
            var colors = GetComponent<Button>().colors;
            colors.normalColor = new Color32(102, 102, 102, 255);
            colors.highlightedColor = new Color32(102, 102, 102, 255);
            GetComponent<Button>().colors = colors;
            isDisabled = true;
        }

        //Sets Default color and enables button
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
