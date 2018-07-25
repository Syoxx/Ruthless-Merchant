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
        public float YSpeed = 0;

        VRSceneItem[] VRItems;

        void OnEnable()
        {
            VRItems = FindObjectsOfType<VRSceneItem>();

            foreach (VRSceneItem VRItem in VRItems)
            {
                if (VRItem.TouchesGround(true))
                {
                    //VRItem.GetComponent<Rigidbody>().useGravity = false;
                    //VRItem.GetComponent<Rigidbody>().isKinematic = true;
                }
            }

            YSpeed = 0;
        }

        void Update()
        {
            Vector3 delta;

            float targetDelta = TargetPositionPlayer - TradeAbstract.Singleton.PlayerZone.transform.position.y;

            if (Math.Sqrt(Math.Pow(targetDelta, 2)) < yDeltaThreshold && Math.Sqrt(Math.Pow(YSpeed, 2)) < ySpeedThreshold)
            {
                delta = new Vector3(0, targetDelta, 0);

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += delta;
                        VRItem.GetComponent<Rigidbody>().useGravity = true;
                        VRItem.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

                TradeAbstract.Singleton.PlayerZone.transform.position += delta;
                TradeAbstract.Singleton.TraderZone.transform.position += delta;

                enabled = false;
            }
            else
            {
                YSpeed += (TargetPositionPlayer - TradeAbstract.Singleton.PlayerZone.transform.position.y) * SpeedModifier;
                YSpeed *= yFriction;

                delta = new Vector3(0, YSpeed, 0);

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += delta * Time.deltaTime;
                    }
                }

                TradeAbstract.Singleton.PlayerZone.transform.position += delta * Time.deltaTime;
                TradeAbstract.Singleton.TraderZone.transform.position -= delta * Time.deltaTime;
            }
        }
    }
}
