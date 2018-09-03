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
        public float TargetConnectorRotation; 

        public float SpeedModifier = 0.4f;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public float SpeedX = 0;

        [SerializeField, Range(0,1)]
        float frictionY = 0.95f;

        [SerializeField]
        float speedThresholdY = 0.075f;

        [SerializeField]
        float positionThresholdY = 0.02f;

        [SerializeField]
        Transform scaleRight;

        [SerializeField]
        Transform scaleLeft;

        VRSceneItem[] VRItems;

        void OnEnable()
        {
            VRItems = FindObjectsOfType<VRSceneItem>();
        }

        void Update()
        {
            Vector3 rotationDelta;

            float targetDelta = TargetConnectorRotation - TradeAbstract.Singleton.connector.transform.eulerAngles.x;

            // End Movement?
            if (Math.Abs(targetDelta) < positionThresholdY && Math.Abs(SpeedX) < speedThresholdY)
            {
                Vector3 traderDelta = scaleRight.position;
                TradeAbstract.Singleton.connector.transform.eulerAngles = new Vector3(TargetConnectorRotation, -90, 90);
                traderDelta -= scaleRight.position;

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += traderDelta;
                        VRItem.GetComponent<Rigidbody>().useGravity = true;
                        VRItem.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

                Debug.LogWarning("Ended Movement!");

                enabled = false;
            }

            // Continue Movement.
            else
            {
                SpeedX += (TargetConnectorRotation - TradeAbstract.Singleton.connector.transform.eulerAngles.x) * SpeedModifier;
                SpeedX *= frictionY;

                float newX = TradeAbstract.Singleton.connector.localEulerAngles.x + SpeedX * Time.deltaTime;

                if (newX > -70)
                    newX = -70;
                else if (newX < -110)
                    newX = -110;

                TradeAbstract.Singleton.connector.localEulerAngles = new Vector3(newX, -90, 90);

                Vector3 traderDelta = scaleRight.position;

                scaleRight.position = TradeAbstract.Singleton.platePositionPlayer.transform.position;
                scaleLeft.position = TradeAbstract.Singleton.platePositionTrader.transform.position;

                traderDelta -= scaleRight.position;

                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.TouchesGround(true))
                    {
                        VRItem.transform.position += traderDelta;
                    }
                }
            }
        }
    }
}
