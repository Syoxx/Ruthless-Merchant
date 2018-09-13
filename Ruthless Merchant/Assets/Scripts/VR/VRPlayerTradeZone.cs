using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class VRPlayerTradeZone : MonoBehaviour
    {
        public static VRPlayerTradeZone Singleton;

        public int TotalWeight = 0;

        [System.NonSerialized]
        public bool UpdateWeight = false;

        void Awake()
        {
            Singleton = this;
        }

        void LateUpdate()
        {
            if(UpdateWeight && !TradeAbstract.Singleton.Exit)
            {
                UpdateTotalWeight();
                TradeAbstract.Singleton.ModifyOffer();
                UpdateWeight = false;
            }
        }

        void UpdateTotalWeight()
        {
            TotalWeight = 0;

            foreach (VRSceneItem item in FindObjectsOfType<VRSceneItem>())
            {
                if (item.TouchesGround(true))
                {
                    TotalWeight += item.Item.Value;
                }
            }            
        }
    }
}