using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

namespace RuthlessMerchant
{
    public class VR_Contract : MonoBehaviour
    {
        public static VR_Contract Singleton;

        [SerializeField]
        GameObject traderAutograph;

        [SerializeField]
        GameObject buyerAutograph;

        Interactable interactable;

        [SerializeField]
        GameObject fallBackHand;

        [SerializeField]
        GameObject hand1;

        [SerializeField]
        GameObject hand2;

        Vector3 fixedPosition;

        bool getpressdown;

        private void Awake()
        {
            Singleton = this;
            interactable = GetComponent<Interactable>();
            fixedPosition = transform.position;
        }

        void LateUpdate()
        {
            if (transform.parent == fallBackHand.transform || transform.parent == hand1.transform || transform.parent == hand2.transform)
            {
                if (!getpressdown)
                {
                    WriteTraderAutograph();
                    getpressdown = true;
                    FindObjectOfType<VRTrade>().HandlePlayerOffer();
                }
            }
            else
            {
                getpressdown = false;
            }

            transform.position = fixedPosition;
        }

        public void WriteTraderAutograph()
        {
            traderAutograph.SetActive(true);
        }

        public void WriteBuyerAutograph()
        {
            buyerAutograph.SetActive(true);
        }

        public void EraseBuyerAutograph()
        {
            buyerAutograph.SetActive(false);
        }

        public void EraseAutographs()
        {
            traderAutograph.SetActive(false);
            buyerAutograph.SetActive(false);
        }
    }
}
