using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{
    public class VR_ExitTrigger : MonoBehaviour
    {
        [SerializeField]
        string featureShowcaseScene;

        [SerializeField]
        GameObject triggerObject;

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger: " + other.gameObject.name);

            if (other.gameObject == triggerObject)
            {
                VR_TutorialScript.Singleton.FadeImage.FadingWithCallback(1f, VR_TutorialScript.Singleton.FadeTime, delegate
                {
                    VR_SceneSwitcher.Singleton.LoadScene("VR_SmithScene");
                });
            }
        }
    }
}
