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
        public struct Recipe
        {
            [SerializeField]
            Item result;

            [SerializeField]
            List<Materials> materials;

            [SerializeField]
            bool unlocked;

            #region GetFunctions

            public Item Result
            {
                get
                {
                    return result;
                }
            }

            public List<Materials> ListOfMaterials
            {
                get
                {
                    return materials;
                }
            }

            public bool Unlocked
            {
                get
                {
                    return unlocked;
                }
            }

            #endregion

            [System.Serializable]
            public struct Materials
            {
                [SerializeField]
                Item item;

                [SerializeField]
                int count;

                #region GetFunctions

                public Item Item
                {
                    get
                    {
                        return item;
                    }
                }

                public int Count
                {
                    get
                    {
                        return count;
                    }
                }

                #endregion

                public Materials(Item item, int count)
                {
                    this.item = item;
                    this.count = count;
                }
            }

            public Recipe(Item result, List<Materials> materials, bool autoUnlocked)
            {
                this.result = result;
                this.materials = materials;
                this.unlocked = autoUnlocked;
            }
        }

        public List<Recipe> GetRecipes()
        {
            return recipes;
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

