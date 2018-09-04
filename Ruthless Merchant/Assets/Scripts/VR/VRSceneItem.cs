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
        string itemName;

        [SerializeField]
        int value;

        [SerializeField]
        bool SmithingHammer;

        /// <summary>
        /// True if the VRSceneItem has been checked for contact with the ground in the current frame. 
        /// Used for internal recursive purposes in the TouchesGround Method.
        /// </summary>
        public bool CheckedGndTouch = false;

        void Awake()
        {
            Collisions = new List<GameObject>();
            Item.ItemName = itemName;
            Item.Value = value;
        }

        public void ChangeToHammerController(int hand)
        {
            if (VRSmithing.Singleton.hammerItem == gameObject && VRSmithing.Singleton.AllIronsPlaced && (transform.parent == VRSmithing.Singleton.hand1 || transform.parent == VRSmithing.Singleton.hand2))
            {
                if (hand == 1)
                {
                    foreach (GameObject gObject in VRSmithing.Singleton.controllerObjectsToDeactivate1)
                        gObject.SetActive(false);

                    VRSmithing.Singleton.hammerController1.SetActive(true);
                }

                else if (hand == 2)
                {
                    foreach (GameObject gObject in VRSmithing.Singleton.controllerObjectsToDeactivate2)
                        gObject.SetActive(false);

                    VRSmithing.Singleton.hammerController2.SetActive(true);
                }

                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Collisions.Add(collision.gameObject);

            if (VRPlayerTradeZone.Singleton != null)
                VRPlayerTradeZone.Singleton.UpdateWeight = true;

            VRSmithing vrSmithing = FindObjectOfType<VRSmithing>();

            if (vrSmithing != null && vrSmithing.AllIronsPlaced && collision.gameObject.name == "WorkbenchHammer")
            {
                foreach(GameObject iron in VRSmithing.Singleton.Irons)
                {
                    iron.SetActive(false);
                }

                VRSmithing.Singleton.FinalSword.SetActive(true);
            }
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
                    allItem.CheckedGndTouch = false;
                }
            }

            foreach (GameObject collision in Collisions)
            {
                if (collision.name.Contains("Schale"))
                {
                    return true;
                }
            }

            CheckedGndTouch = true;

            foreach (GameObject collision in Collisions)
            {
                VRSceneItem item = collision.GetComponent<VRSceneItem>();

                if (item != null && !item.CheckedGndTouch && item.TouchesGround())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
