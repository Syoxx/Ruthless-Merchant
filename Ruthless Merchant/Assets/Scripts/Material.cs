using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Material : Item
    {
        [SerializeField] private MaterialsType materialsType;


        public override void Start()
        {
            
        }

        public override void Update()
        {
            
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (gameObject.tag == "Iron")
            {
                if (collision.gameObject.tag == "Iron")
                    Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
            }
        }
    }

}

