using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class CraftingSystem : MonoBehaviour
    {

        Recipes recipes;

        private void Awake()
        {
            recipes = GetComponent<Recipes>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
