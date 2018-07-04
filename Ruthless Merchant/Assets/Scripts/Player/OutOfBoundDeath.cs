using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{

    public class OutOfBoundDeath : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void OnTriggerEnter(Collider col)
        {
            Debug.Log("enter");
            Player player = col.GetComponent<Player>();
            if (col.gameObject.tag == "Player")
            {
                player.OnOutOfBound();
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Player player = collision.collider.GetComponent<Player>();
            if (collision.collider.gameObject.tag == "Player")
                player.OnOutOfBound();
        }
    }
}
