using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RuthlessMerchant {
    public class QuestButton : MonoBehaviour {


        public void CompleteButton()
        {
            Debug.Log("Color changed");
                    var colors = GetComponent<Button>().colors;
                    colors.disabledColor = new Color32(171,243, 116, 128);
                    GetComponent<Button>().colors = colors;
        }

        public void InProgressButton()
        {
            Debug.Log("InProgress");
            var colors = GetComponent<Button>().colors;
            colors.normalColor = new Color32(179, 239, 250, 128);
            colors.disabledColor = new Color32(179, 239, 250, 128);
            GetComponent<Button>().colors = colors;
        }

        public void ButtonSettings(bool interactable)
        {
            GetComponent<Button>().interactable = interactable;
        }

    }
}
