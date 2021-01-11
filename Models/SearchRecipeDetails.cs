using System;

namespace Recipes.Models
{
    public class SearchRecipeDetails
    {
        #region Properties
        public string RecipeName { get; }
        public string Cuisine { get; }
        public string Diet { get; }
        public string ExcludedIngredients { get; }
        public string Intolerance { get; }
        #endregion

        #region Constructor
        public SearchRecipeDetails(string recipeName, string cuisine = "", string diet = "", string excludedIngredients = "", string intolerance = "") {
            RecipeName = recipeName;
            Cuisine = cuisine;
            Diet = diet;
            ExcludedIngredients = excludedIngredients;
            Intolerance = intolerance;
        }
        #endregion
    }
}