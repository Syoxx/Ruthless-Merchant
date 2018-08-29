using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{

    public static class Fading
    {

        public static void FadingWithCallback(this Image image, float alpha, float duration, System.Action action)
        {
            MonoBehaviour mono = image.GetComponent<MonoBehaviour>();
            mono.StartCoroutine(FadingWithCallbackCOR(image, alpha, duration, action));
        }

        public static IEnumerator FadingWithCallbackCOR(Image image, float alpha, float duration, System.Action action)
        {
            Color currentColor = image.color;

            Color visibleColor = image.color;
            visibleColor.a = alpha;

            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                image.color = Color.Lerp(currentColor, visibleColor, counter / duration);
                yield return null;
            }

            //Done. Execute Callback
            action.Invoke();
        }
    }
}
