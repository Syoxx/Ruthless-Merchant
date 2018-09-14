using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Recipes : MonoBehaviour
    {
        #region Fields #############################################################################################


        [SerializeField,Tooltip("The number of recipes depicted on one page")]
        public int maxRecipesPerPage;

        [SerializeField, Tooltip("The book-object with the BookPro Script")]
        BookPro book;

        [SerializeField, Tooltip("The Prefab that will be used for displaying the recipes")]
        GameObject recipePanelPrefab;

        [SerializeField, Tooltip("The prefab that will be used for displaying the materials in the recipe-panels")]
        GameObject componentPanelPrefab;

        [SerializeField]
        Item[] SteelAndIron;

        [SerializeField, Tooltip("All recipes that can be created by the player")]
        List<Recipe> recipes;

        List<RecipePanel> recipePanels;

        /// <summary>
        /// The list of all the counters for the recipe-materials
        /// </summary>
        List<ItemCount> counts;

        GameObject[] recipePages;

        #endregion


        #region Properties #########################################################################################

        public List<RecipePanel> Panels { get { return recipePanels; } }

        #endregion


        #region Structs #############################################################################################

        [System.Serializable]
        public struct Recipe
        {
            [SerializeField, Tooltip("The weapon which can be created with this recipe")]
            Item result;

            [SerializeField, Tooltip("The materials required to create the item")]
            List<Materials> materials;

            [SerializeField, Tooltip("if this is false the player has to unlock it")]
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
                [SerializeField, Tooltip("The item-prefab that is being required")]
                Item item;

                [SerializeField, Tooltip("The amount of materials required")]
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

            /// <summary>
            /// Unlocks the recipe.
            /// </summary>
            /// <returns>returns false if recipe was already unlocked</returns>
            public bool Unlock()
            {
                if(unlocked)
                {
                    return false;
                }
                else
                {
                    unlocked = true;
                    return true;
                }
            }
        }

        struct ItemCount
        {
            private TMP_Text textfield;
            private Item itemToCount;
            private int count;

            public TMP_Text Textfield
            {
                get
                {
                    return textfield;
                }
            }
            public Item ItemToCount
            {
                get
                {
                    return itemToCount;
                }
            }
            public int Count { get { return count; } }

            public ItemCount(TMP_Text textfield, Item item, int count)
            {
                this.textfield = textfield;
                itemToCount = item;
                this.count = count;
            }
        }

        public struct RecipePanel
        {
            Button button;
            int recipe;

            public int Recipe { get { return recipe; } }
            public Button Button { get { return button; } }

            public RecipePanel(int recipe, Button button)
            {
                this.recipe = recipe;
                this.button = button;
            }
        }

        #endregion


        #region Private Functions ##################################################################################

        private void Awake()
        {
            if(SteelAndIron.Length > 1)
            {
                List<Recipe.Materials> steelList = new List<Recipe.Materials> { new Recipe.Materials(SteelAndIron[1], 2) };

                recipes.Insert(0, new Recipe(SteelAndIron[0], steelList, true));
            }

            recipePanels = new List<RecipePanel>();
            counts = new List<ItemCount>();
            if(!book)
            {
                book = GameObject.Find("Book").GetComponent<BookPro>();
            }
            recipePages = FindPanels();
        }

        /// <summary>
        /// finds all pages in the book that have the "Book_Recipes" tag.
        /// </summary>
        /// <returns>returns an array of all pages with the tag. the pages who come first are also the first in the array.</returns>
        private GameObject[] FindPanels()
        {
            if(!book)
            {
                throw new System.NullReferenceException("Please add the bookprefab to book-property");
            }
            List<GameObject> panels = new List<GameObject>();
            for(int i = 0; i < book.papers.Length; i++)
            {
                if(book.papers[i].Front.tag == "Book_Recipes")
                {
                    panels.Add(book.papers[i].Front);
                }
                if (book.papers[i].Back.tag == "Book_Recipes")
                {
                    panels.Add(book.papers[i].Back);
                }
            }

            return panels.ToArray();
        }

        private void Start()
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                if (recipes[i].Unlocked)
                {
                    AddRecipePanel(i);
                }
            }
            Player.Singleton.Inventory.InventoryChanged.AddListener(UpdateCounts);
        }

        /// <summary>
        /// Adds a recipepanel with all its materials and the result
        /// </summary>
        /// <param name="recipeIndex">the index of the recipe in the recipes-list. returns null if index is out of range</param>
        private GameObject AddRecipePanel(int recipeIndex)
        {
            if(!recipePanelPrefab)
            {
                throw new System.NullReferenceException("No recipePanelPrefab found");
            }
            if (recipeIndex >= recipes.Count || recipeIndex < 0)
            {
                return null;
            }

            Transform currentPageParent = transform;
            int currentPage = recipeIndex / maxRecipesPerPage;
            if(currentPage < recipePages.Length)
            {
                currentPageParent = recipePages[currentPage].transform.GetChild(0);
            }

            GameObject newPanel = Instantiate(recipePanelPrefab, currentPageParent);

            newPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = recipes[recipeIndex].Result.ItemInfo.ItemName;
            newPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = recipes[recipeIndex].Result.ItemInfo.ItemSprite;
            recipePanels.Add(new RecipePanel(recipeIndex, newPanel.GetComponent<Button>()));

            for (int i = 0; i < recipes[recipeIndex].ListOfMaterials.Count; i++)
            {
                GameObject comPanel = Instantiate(componentPanelPrefab, newPanel.transform);
                comPanel.transform.SetSiblingIndex(0);
                if (recipes[recipeIndex].ListOfMaterials[i].Item != null)
                {
                    comPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = recipes[recipeIndex].ListOfMaterials[i].Item.ItemInfo.ItemName;
                    comPanel.transform.GetChild(1).GetComponent<Image>().sprite = recipes[recipeIndex].ListOfMaterials[i].Item.ItemInfo.ItemSprite;
                }
                counts.Add(new ItemCount(comPanel.transform.GetChild(2).GetComponent<TMP_Text>(), recipes[recipeIndex].ListOfMaterials[i].Item, recipes[recipeIndex].ListOfMaterials[i].Count));
                counts[counts.Count - 1].Textfield.text = Player.Singleton.Inventory.GetNumberOfItems(counts[counts.Count - 1].ItemToCount).ToString() + "/" + counts[counts.Count - 1].Count;
            }

            UpdateCounts();
            return newPanel;
        }

        #endregion


        #region Public Functions ##################################################################################

        /// <summary>
        /// Returns all the recipes listed in this class
        /// </summary>
        public List<Recipe> GetRecipes()
        {
            return recipes;
        }

        /// <summary>
        /// Unlocks a recipe and adds a recipe-panel to the book
        /// </summary>
        /// <param name="recipeIndex">index of the activated recipe</param>
        /// <returns>returns the created panel-gameobject</returns>
        public GameObject ActivateRecipe(int recipeIndex)
        {
            if (recipes[recipeIndex].Unlock())
            {
                return AddRecipePanel(recipeIndex);
            }
            else return null;
        }

        /// <summary>
        /// Updates the numbers for counts in inventorypanels.
        /// </summary>
        public void UpdateCounts()
        {
            for (int i = 0; i < counts.Count; i++)
            {
                counts[i].Textfield.text = Player.Singleton.Inventory.GetNumberOfItems(counts[i].ItemToCount).ToString() + "/" + counts[i].Count.ToString();
                if (Player.Singleton.Inventory.GetNumberOfItems(counts[i].ItemToCount) < counts[i].Count)
                {
                    counts[i].Textfield.color = Color.red;
                }
                else
                {
                    counts[i].Textfield.color = Color.green;
                }
            }
        }

        /// <summary>
        /// Removes all listeners from the inventory-buttons
        /// </summary>
        public void ResetEvents()
        {
            foreach (RecipePanel recipePanel in recipePanels)
            {
                recipePanel.Button.onClick.RemoveAllListeners();
            }
        }

        #endregion
    }
}

