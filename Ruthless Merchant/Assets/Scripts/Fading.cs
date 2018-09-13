using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{

    public static class Fading
    {
        /// <summary>
        /// Starts the Fading as a Coroutine with a Callback
        /// </summary>
        /// <param name="img">image used to Fade out and in</param>
        /// <param name="alpha">Desired Alpha value, 0 for transparent, 1 for opaque</param>
        /// <param name="duration">time it takes to fade</param>
        /// <param name="action">Action that is executed after fading</param>
        public static void FadingWithCallback(this Image image, float alpha, float duration, System.Action action)
        {
            MonoBehaviour mono = image.GetComponent<MonoBehaviour>();
            mono.StartCoroutine(FadingWithCallbackCOR(image, alpha, duration, action));
        }

        /// <summary>
        /// The Fading in itself, param definition is the same as above
        /// </summary>
        /// <param name="img"></param>
        /// <param name="alpha"></param>
        /// <param name="duration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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
