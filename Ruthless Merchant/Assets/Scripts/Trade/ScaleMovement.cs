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
        public float TargetConnectorRotationZ; 

        public float SpeedModifier = 0.4f;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public float SpeedZ = 0;

        [SerializeField, Range(0,1)]
        float frictionZ = 0.95f;

        [SerializeField]
        float speedThresholdZ = 0.075f;

        [SerializeField]
        float rotationThresholdZ = 0.02f;

        VRSceneItem[] VRItems;

        void OnEnable()
        {
            VRItems = FindObjectsOfType<VRSceneItem>();
        }

        void Update()
        {
            TradeAbstract trade = TradeAbstract.Singleton;
            float targetDelta = TargetConnectorRotationZ - get180(trade.connector.localEulerAngles.z);

            // End Movement?
            if (Math.Abs(targetDelta) < rotationThresholdZ && Math.Abs(SpeedZ) < speedThresholdZ)
            {
                Vector3 traderDelta = trade.PlayerZone.position;
                trade.connector.transform.localEulerAngles = new Vector3(0, 0, TargetConnectorRotationZ);
                traderDelta -= trade.PlayerZone.position;

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += traderDelta;
                    }
                }

                enabled = false;
            }

            // Continue Movement.
            else
            {
                SpeedZ += (TargetConnectorRotationZ - get180(TradeAbstract.Singleton.connector.localEulerAngles.z)) * SpeedModifier;
                SpeedZ *= frictionZ;

                Vector3 traderDelta = trade.platePositionPlayer.transform.position;

                trade.connector.localEulerAngles += new Vector3(0, 0, SpeedZ * Time.deltaTime);

                trade.PlayerZone.position = trade.platePositionPlayer.transform.position;
                trade.TraderZone.position = trade.platePositionTrader.transform.position;

                traderDelta = trade.platePositionPlayer.transform.position - traderDelta;

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += traderDelta;
                    }
                }
            }
        }

        float get180(float float360)
        {
            if (float360 > 180)
                return float360 - 360;
            else
                return float360;
        }
    }
}
