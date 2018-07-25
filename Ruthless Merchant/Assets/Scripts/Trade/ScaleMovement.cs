using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ScaleMovement : MonoBehaviour
    {
        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public float TargetPositionPlayer; 

        public float TargetPositionTrader;

        public float SpeedModifier = 0.4f;

        [SerializeField, Range(0,1)]
        float yFriction = 0.95f;

        [SerializeField]
        float ySpeedThreshold = 0.075f;

        [SerializeField]
        float yDeltaThreshold = 0.02f;

        [SerializeField, ReadOnly]
        float ySpeed = 0;

        VRSceneItem[] VRItems;

        void OnEnable()
        {
            VRItems = FindObjectsOfType<VRSceneItem>();

            foreach (VRSceneItem VRItem in VRItems)
            {
                if (VRItem.WeightParent == transform)
                {
                    VRItem.GetComponent<Rigidbody>().useGravity = false;
                    VRItem.GetComponent<Rigidbody>().isKinematic = true;
                }
            }

            ySpeed = 0;
        }

        void Update()
        {
            Vector3 delta;

            if (Math.Sqrt(Math.Pow(TargetPositionPlayer - TradeAbstract.Singleton.PlayerZone.transform.position.y, 2)) < yDeltaThreshold && Math.Sqrt(Math.Pow(ySpeed, 2)) < ySpeedThreshold)
            {
                Vector3 temp = TradeAbstract.Singleton.PlayerZone.transform.position;
                TradeAbstract.Singleton.PlayerZone.transform.position += new Vector3(0, -TradeAbstract.Singleton.PlayerZone.transform.position.y + TargetPositionPlayer, 0);
                TradeAbstract.Singleton.TraderZone.transform.position += new Vector3(0, -TradeAbstract.Singleton.TraderZone.transform.position.y + TargetPositionTrader, 0);

                delta = new Vector3(0, TradeAbstract.Singleton.PlayerZone.transform.position.y - temp.y, 0);

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.WeightParent == TradeAbstract.Singleton.PlayerZone.transform)
                    {
                        VRItem.transform.position += delta;
                        VRItem.GetComponent<Rigidbody>().useGravity = true;
                        VRItem.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

                enabled = false;
            }
            else
            {
                ySpeed += (TargetPositionPlayer - TradeAbstract.Singleton.PlayerZone.transform.position.y) * SpeedModifier;
                ySpeed *= yFriction;

                delta = new Vector3(0, ySpeed, 0);

                TradeAbstract.Singleton.PlayerZone.transform.position += delta * Time.deltaTime;
                TradeAbstract.Singleton.TraderZone.transform.position -= delta * Time.deltaTime;

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.WeightParent == TradeAbstract.Singleton.PlayerZone.transform)
                    {
                        VRItem.transform.position += delta * Time.deltaTime;
                    }
                }
            }
        }
    }
}
