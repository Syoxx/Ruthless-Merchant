using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class VR_TutorialScript : MonoBehaviour
    {
        [SerializeField]
        private string triggerTag = "RespawnTrigger";

        [SerializeField]
        private string triggerName = "ExitTrigger";

        [SerializeField]
        private string featureShowcaseScene;

        [SerializeField]
        private Image fadeImage;

        [SerializeField]
        private float fadeTime;

        // Use this for initialization
        void Start()
        {
            if (fadeImage == null)
                fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();

            fadeImage.FadingWithCallback(0f, fadeTime, delegate
            {
                Debug.Log("Tutorial started");
            });
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == triggerTag || collision.gameObject.name == triggerName)
            {
                fadeImage.FadingWithCallback(1f, fadeTime, delegate
                {
                    SceneManager.LoadScene(featureShowcaseScene);
                });
            }
        }
    }
}
