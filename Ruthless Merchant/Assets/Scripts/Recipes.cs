using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Recipes : MonoBehaviour
    {
        [SerializeField, Tooltip("The panel in the book in which the recipes will be displayed (currently named RecipeCanvas). It isn't required but the panel should have a vertical layout group component")]
        GameObject recipeCanvas;

        [SerializeField, Tooltip("The Prefab that will be used for displaying the recipes")]
        GameObject recipePanelPrefab;

        [SerializeField, Tooltip("The prefab that will be used for displaying the materials in the recipe-panels")]
        GameObject componentPanelPrefab;

        [SerializeField, Tooltip("All recipes that can be created by the player")]
        List<Recipe> recipes;

        List<RecipePanel> recipePanels;

        public List<RecipePanel> Panels { get { return recipePanels; } }

        List<ItemCount> counts;

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
            public Button Button{ get { return button; } }

            public RecipePanel(int recipe, Button button)
            {
                this.recipe = recipe;
                this.button = button;
            }
        }

        /// <summary>
        /// Returns all the recipes listed in this class
        /// </summary>
        public List<Recipe> GetRecipes()
        {
            return recipes;
        }

        /// <summary>
        /// Adds a recipepanel with all its materials and the result
        /// </summary>
        /// <param name="recipeIndex">the index of the recipe in the recipes-list. returns null if index is out of range</param>
        public GameObject AddRecipePanel(int recipeIndex)
        {
            if(recipeIndex >= recipes.Count || recipeIndex < 0)
            {
                return null;
            }
            GameObject newPanel = Instantiate(recipePanelPrefab, recipeCanvas.transform);

            newPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = recipes[recipeIndex].Result.itemName;
            newPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = recipes[recipeIndex].Result.itemSprite;
            recipePanels.Add(new RecipePanel(recipeIndex, newPanel.GetComponent<Button>()));

            for (int i = 0; i < recipes[recipeIndex].ListOfMaterials.Count; i++)
            {
                GameObject comPanel = Instantiate(componentPanelPrefab, newPanel.transform);
                comPanel.transform.SetSiblingIndex(0);
                comPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = recipes[recipeIndex].ListOfMaterials[i].Item.itemName;
                comPanel.transform.GetChild(1).GetComponent<Image>().sprite = recipes[recipeIndex].ListOfMaterials[i].Item.itemSprite;

                counts.Add(new ItemCount(comPanel.transform.GetChild(2).GetComponent<TMP_Text>(), recipes[recipeIndex].ListOfMaterials[i].Item, recipes[recipeIndex].ListOfMaterials[i].Count));
                counts[counts.Count - 1].Textfield.text = Player.Singleton.Inventory.GetNumberOfItems(counts[counts.Count - 1].ItemToCount).ToString() + "/" + counts[counts.Count - 1].Count;
            }

            UpdateCounts();
            return newPanel;
        }

        /// <summary>
        /// Updates the numbers for
        /// </summary>
        public void UpdateCounts()
        {
            for(int i = 0; i < counts.Count; i++)
            {
                counts[i].Textfield.text = Player.Singleton.Inventory.GetNumberOfItems(counts[i].ItemToCount).ToString() + "/" + counts[i].Count.ToString();
                if(Player.Singleton.Inventory.GetNumberOfItems(counts[i].ItemToCount) < counts[i].Count)
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

        private void Awake()
        {
            recipePanels = new List<RecipePanel>();
            counts = new List<ItemCount>();
        }

        private void Start()
        {
            for(int i = 0; i < recipes.Count; i++)
            {
                if(recipes[i].Unlocked)
                {
                    AddRecipePanel(i);
                }
            }
            Player.Singleton.Inventory.InventoryChanged.AddListener(UpdateCounts);
        }
    }
}

