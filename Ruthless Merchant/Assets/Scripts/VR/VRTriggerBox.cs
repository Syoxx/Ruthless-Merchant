using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class VRTriggerBox : MonoBehaviour
    {
        public static VRTriggerBox Singleton;

        public int TotalWeight = 0;

        public bool UpdateWeight = false;

        void Awake()
        {
            Singleton = this;
        }

        void LateUpdate()
        {
            if(UpdateWeight)
            {
                UpdateTotalWeight();
                UpdateWeight = false;
                Debug.Log("Updated!");
            }
        }

        void UpdateTotalWeight()
        {
            TotalWeight = 0;

            foreach (VRSceneItem item in FindObjectsOfType<VRSceneItem>())
            {
                if (item.TouchesGround())
                {
                    TotalWeight += item.Item.Value;
                }
            }

            TradeAbstract.Singleton.ModifyOffer();
        }
    }
}