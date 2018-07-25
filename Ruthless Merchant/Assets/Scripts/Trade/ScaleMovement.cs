using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ScaleMovement : MonoBehaviour
    {
        public ScaleType scaleType;

        public enum ScaleType
        {
            Player,
            Trader
        }

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        public float TargetPosition;

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

            if (scaleType == ScaleType.Player)
            {
                foreach (VRSceneItem VRItem in VRItems)
                {
                    if (VRItem.WeightParent == transform)
                    {
                        VRItem.GetComponent<Rigidbody>().useGravity = false;
                        VRItem.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }

            ySpeed = 0;
        }

        void Update()
        {
            Vector3 delta;

            if (Math.Sqrt(Math.Pow(TargetPosition - transform.position.y, 2)) < yDeltaThreshold && Math.Sqrt(Math.Pow(ySpeed, 2)) < ySpeedThreshold)
            {
                Vector3 temp = transform.position;
                transform.position += new Vector3(0, -transform.position.y + TargetPosition, 0);

                delta = new Vector3(0, transform.position.y - temp.y, 0);

                if (scaleType == ScaleType.Player)
                {
                    foreach (VRSceneItem VRItem in VRItems)
                    {
                        if (VRItem.WeightParent == transform)
                        {
                            VRItem.transform.position += delta;
                            VRItem.GetComponent<Rigidbody>().useGravity = true;
                            VRItem.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                }

                enabled = false;
            }
            else
            {
                ySpeed += (TargetPosition - transform.position.y) * SpeedModifier;
                ySpeed *= yFriction;

                delta = new Vector3(0, ySpeed, 0);

                transform.position += delta * Time.deltaTime;

                if (scaleType == ScaleType.Player)
                {
                    foreach (VRSceneItem VRItem in VRItems)
                    {
                        if (VRItem.WeightParent == transform)
                        {
                            VRItem.transform.position += delta * Time.deltaTime;
                        }
                    }
                }
            }
        }
    }
}
