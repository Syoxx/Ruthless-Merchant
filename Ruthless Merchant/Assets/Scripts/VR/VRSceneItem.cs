using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace RuthlessMerchant
{
    public class VRSceneItem : MonoBehaviour
    {
        public VRItem Item;

        public List<GameObject> Collisions;

        [SerializeField]
        int value;

        /// <summary>
        /// True if the VRSceneItem has been checked for contact with the ground in the current frame. 
        /// Used for internal recursive purposes in the TouchesGround Method.
        /// </summary>
        public bool CheckedGndTouch = false;

        void Awake()
        {
            Collisions = new List<GameObject>();
            Item.Value = value;
        }

        public IEnumerator ChangeToHammerController(int hand)
        {
            yield return new WaitForEndOfFrame();

            if (VRSmithing.Singleton.HammerItem == gameObject && VRSmithing.Singleton.smithingSteps == VRSmithing.SmithingSteps.CreatingSword && (transform.parent == VRSmithing.Singleton.Hand1.transform || transform.parent == VRSmithing.Singleton.Hand2.transform))
            {
                if (hand == 1)
                {
                    foreach (GameObject gObject in VRSmithing.Singleton.ControllerObjectsToDeactivate1)
                        gObject.SetActive(false);

                    VRSmithing.Singleton.HammerController1.SetActive(true);
                }

                else if (hand == 2)
                {
                    foreach (GameObject gObject in VRSmithing.Singleton.ControllerObjectsToDeactivate2)
                        gObject.SetActive(false);

                    VRSmithing.Singleton.HammerController2.SetActive(true);
                }

                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Collisions.Add(collision.gameObject);

            if (VRPlayerTradeZone.Singleton != null)
                VRPlayerTradeZone.Singleton.UpdateWeight = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            Collisions.Remove(collision.gameObject);

            if(VRPlayerTradeZone.Singleton != null)
                VRPlayerTradeZone.Singleton.UpdateWeight = true;
        }

        /// <summary>
        /// If the VRScene Item is touching the ground.
        /// </summary>
        /// <param name="first">Must be true when called, used for internal recursive purposes.</param>
        /// <returns></returns>
        public bool TouchesGround(bool first = false)
        {
            if (first)
            {
                foreach (VRSceneItem allItem in FindObjectsOfType<VRSceneItem>())
                {
                    if(allItem != null)
                        allItem.CheckedGndTouch = false;
                }
            }

            foreach (GameObject collision in Collisions)
            {
                if (collision != null && collision.name.Contains("Schale"))
                {
                    return true;
                }
            }

            CheckedGndTouch = true;

            foreach (GameObject collision in Collisions)
            {
                if (collision != null)
                {
                    VRSceneItem item = collision.GetComponent<VRSceneItem>();

                    if (item != null && !item.CheckedGndTouch && item.TouchesGround())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
