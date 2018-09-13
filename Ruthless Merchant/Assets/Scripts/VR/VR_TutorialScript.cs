using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class VR_TutorialScript : MonoBehaviour
    {
        public static VR_TutorialScript Singleton;

        [SerializeField]
        string triggerTag = "RespawnTrigger";

        [SerializeField]
        string triggerName = "ExitTrigger";

        public Image FadeImage;

        public float FadeTime;

        void Awake()
        {
            Singleton = this;
        }

        void Start()
        {
            if (FadeImage == null)
                FadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();

            FadeImage.FadingWithCallback(0f, FadeTime, delegate
            {
                Debug.Log("Tutorial started");
            });
        }
    }
}
