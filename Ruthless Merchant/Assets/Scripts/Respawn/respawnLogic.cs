using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class respawnLogic : MonoBehaviour {

        [SerializeField]
        [Tooltip("Tag of the Respawn Trigger Zone")]
        private string triggerTag;

        [SerializeField]
        [Tooltip("Tag of the Respawn Points")]
        private string respawnPointTag;

        [SerializeField]
        private Image fadeImage;

        [SerializeField]
        private float fadeTime = 2f;

        private float shortestDistance;
        private GameObject nearestRespawnPoint;
        private GameObject respawnPoint;
        private GameObject FadeCanvas;
        private bool fadingDone;

        private void Start()
        {
            FadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas");
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == triggerTag)
            {
                Debug.Log("Trigger");
                InitiateRespawn();
            }
        }

        public void InitiateRespawn()
        {
            respawnPoint = GetRespawnPoint();
            CrossFadeAlphaWithCallback(fadeImage, 1f, fadeTime, delegate
            {
                transform.position = respawnPoint.transform.position;
                CrossFadeAlphaWithCallback(fadeImage, 0f, fadeTime, delegate
                {
                    Debug.Log("Done fading");
                });
            });
        }

        public GameObject GetRespawnPoint()
        {
            nearestRespawnPoint = null;
            shortestDistance = 0;
            GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag(respawnPointTag);
            for (int i = 0; i < respawnPoints.Length; i++)
            {
                float currentDistance = Vector3.Distance(gameObject.transform.position, respawnPoints[i].transform.position);

                if (nearestRespawnPoint == null)
                {
                    shortestDistance = currentDistance;
                    nearestRespawnPoint = respawnPoints[i];
                }

                if (currentDistance < shortestDistance)
                {
                    shortestDistance = currentDistance;
                    nearestRespawnPoint = respawnPoints[i];
                }
            }

            return nearestRespawnPoint;
        }

        public void CrossFadeAlphaWithCallback(Image img, float alpha, float duration, System.Action action)
        {
            StartCoroutine(CrossFadeAlphaCOR(img, alpha, duration, action));
        }

        IEnumerator CrossFadeAlphaCOR(Image img, float alpha, float duration, System.Action action)
        {
            Color currentColor = img.color;
            Color visibleColor = img.color;
            visibleColor.a = alpha;

            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                img.color = Color.Lerp(currentColor, visibleColor, counter / duration);
                yield return null;
            }

            action.Invoke();
        }
    }
}
