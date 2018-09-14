using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Script by Nikolas Pietrek
/// </summary>
namespace RuthlessMerchant
{
    public class respawnLogic : MonoBehaviour {

        [SerializeField]
        [Tooltip("Tag of the Trigger Zone which triggeres the respawn")]
        private string oobTriggerTag = "RespawnTrigger";

        [SerializeField]
        [Tooltip("Tag of the Outpost Trigger Zone, Saves latest as respawn Point")]
        private string outpostTriggerTag = "CaptureTrigger";

        [SerializeField]
        private Image fadeImage;

        [SerializeField]
        private float fadeTime = 2f;

        private float shortestDistance;
        private GameObject nearestRespawnPoint;
        private GameObject respawnPoint;
        private bool fadingDone;
        private Vector3 respawnPosition;
        private Character character;
        /// <summary>
        /// gets the Fade Image at the Start
        /// </summary>
        private void Start()
        {
            respawnPosition = GameObject.Find("RespawnPointPlayer").transform.position;

            character = GetComponent<Character>();
        }

        /// <summary>
        /// checks for Collision with a Trigger Zone
        /// when colliging with the respawn Trigger, initiates the respawn
        /// when colliding with the outpost Trigger Zone saves the current Position for respawn
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == oobTriggerTag)
            {
                InitiateRespawn();
            }

            else if (other.tag == outpostTriggerTag)
            {
                respawnPosition = gameObject.transform.position;
            }
        }

        /// <summary>
        /// Initiates the Respawn, calls the Fading out with the teleport and the fading in as Callbacks
        /// </summary>
        public void InitiateRespawn()
        {
            fadeImage.FadingWithCallback(1f, fadeTime, delegate
            {
                if (character != null)
                    character.HealthSystem.ChangeHealth(character.HealthSystem.MaxHealth, character);

                transform.position = respawnPosition;
                fadeImage.FadingWithCallback(0f, 2f, delegate
                {
                    Debug.Log("Done fading");
                });
            });
        }
    }
}
