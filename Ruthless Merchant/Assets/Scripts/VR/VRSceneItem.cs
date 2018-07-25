using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class VRSceneItem : MonoBehaviour
    {
        public VRItem Item;

        public Transform WeightParent;

        public List<GameObject> Collisions;

        [SerializeField]
        string itemName;

        [SerializeField]
        int value;

        bool touchesGround = false;

        void Awake()
        {
            Collisions = new List<GameObject>();
            Item.ItemName = itemName;
            Item.Value = value;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Collisions.Add(collision.gameObject);
            VRTriggerBox.Singleton.UpdateWeight = true;
            Debug.LogWarning("ADDED" + collision.gameObject.name + " in " + gameObject.name);
        }

        private void OnCollisionExit(Collision collision)
        {
            Collisions.Remove(collision.gameObject);
            VRTriggerBox.Singleton.UpdateWeight = true;
            Debug.LogWarning("REMOVED " + collision.gameObject.name + " from " + gameObject.name);
        }

        public bool TouchesGround()
        {
            foreach (GameObject collision in Collisions)
            {
                if (collision.name.Contains("TradeZone"))
                {
                    return true;
                }
            }

            foreach (GameObject collision in Collisions)
            {
                VRSceneItem item = collision.GetComponent<VRSceneItem>();

                if (item != null && item.TouchesGround())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
