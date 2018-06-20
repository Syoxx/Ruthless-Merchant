using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Recipes : MonoBehaviour
    {

        [SerializeField]
        List<Recipe> recipes;

        [System.Serializable]
        struct Recipe
        {
            [SerializeField]
            Item Result;

            [SerializeField]
            List<Materials> materials;

            [SerializeField]
            bool autoUnlocked;

            [System.Serializable]
            struct Materials
            {
                [SerializeField]
                Item item;

                [SerializeField]
                int count;
            }
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

