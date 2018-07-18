using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class VRTriggerBox : MonoBehaviour
    {
        public int TotalWeight = 0;

        [ReadOnly]
        List<VRSceneItem> items;

        void Start()
        {
            items = new List<VRSceneItem>();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name + " entered " + gameObject.name + " TriggerBox.");
            VRSceneItem item = other.GetComponent<VRSceneItem>();

            if (item != null)
            {
                items.Add(item);
            }

            UpdateTotalWeight();
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log(other.gameObject.name + " quitted " + gameObject.name + " TriggerBox.");
            VRSceneItem item = other.GetComponent<VRSceneItem>();

            if (item != null)
            {
                items.Remove(item);
            }

            UpdateTotalWeight();
        }

        void UpdateTotalWeight()
        {
            int result = 0;

            foreach(VRSceneItem item in items)
            {
                result += item.Item.Value;
            }

            TotalWeight = result;

            TradeAbstract.Singleton.ModifyOffer();
        }
    }
}