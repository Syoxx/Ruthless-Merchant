using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class ScrollingText : MonoBehaviour {

        [SerializeField]
        private string mainMenu = "newMainMenu";

        [SerializeField]
        private float scrollSpeed = 1;

        [SerializeField]
        private Image fadeImage;

        // Use this for initialization
        void Start() {
            fadeImage.FadingWithCallback(0f, 2f, delegate
            {
                Debug.Log("Starting Credits Scene");
            });
    
    }

        // Update is called once per frame
        void Update() {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                fadeImage.FadingWithCallback(1f, 2f, delegate
                {
                    SceneManager.LoadScene("NewMainMenu");
                });
            }
        }
    }
}
