using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class UniqueIDGenerator : MonoBehaviour
    {
        private System.Guid myGuid;
        // Use this for initialization
        void Start()
        {
            myGuid = System.Guid.NewGuid();
        }

        // Update is called once per frame
        public System.Guid GetId()
        {
            return myGuid;
        }
    }

}

