using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void OnCollisionEnter(Collision collision)
        {
            Collisions.Add(collision.gameObject);
            VRPlayerTradeZone.Singleton.UpdateWeight = true;
            Debug.LogWarning("ADDED " + collision.gameObject.name + " in " + gameObject.name);
        }

        private void OnCollisionExit(Collision collision)
        {
            Collisions.Remove(collision.gameObject);
            VRPlayerTradeZone.Singleton.UpdateWeight = true;
            Debug.LogWarning("REMOVED " + collision.gameObject.name + " from " + gameObject.name);
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
