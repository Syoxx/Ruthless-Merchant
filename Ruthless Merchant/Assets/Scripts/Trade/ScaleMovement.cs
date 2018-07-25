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

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public float SpeedY = 0;

        [SerializeField, Range(0,1)]
        float frictionY = 0.95f;

        [SerializeField]
        float speedThresholdY = 0.075f;

        [SerializeField]
        float positionThresholdY = 0.02f;

        VRSceneItem[] VRItems;

        void OnEnable()
        {
            VRItems = FindObjectsOfType<VRSceneItem>();
        }

        void Update()
        {
            Vector3 positionDelta;

            float targetDelta = TargetPositionPlayer - TradeAbstract.Singleton.PlayerZone.transform.position.y;

            // End Movement?
            if (Math.Abs(targetDelta) < positionThresholdY && Math.Abs(SpeedY) < speedThresholdY)
            {
                positionDelta = new Vector3(0, targetDelta, 0);

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += positionDelta;
                        VRItem.GetComponent<Rigidbody>().useGravity = true;
                        VRItem.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

                TradeAbstract.Singleton.PlayerZone.transform.position += positionDelta;
                TradeAbstract.Singleton.TraderZone.transform.position += positionDelta;

                enabled = false;
            }

            // Continue Movement.
            else
            {
                SpeedY += (TargetPositionPlayer - TradeAbstract.Singleton.PlayerZone.transform.position.y) * SpeedModifier;
                SpeedY *= frictionY;

                positionDelta = new Vector3(0, SpeedY, 0);

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += positionDelta * Time.deltaTime;
                    }
                }

                TradeAbstract.Singleton.PlayerZone.transform.position += positionDelta * Time.deltaTime;
                TradeAbstract.Singleton.TraderZone.transform.position -= positionDelta * Time.deltaTime;
            }
        }
    }
}
