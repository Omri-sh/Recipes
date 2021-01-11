using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes
{
    public static class RecipesApiCalls
    {
        #region API Calls
        /// <summary>
        /// Get a joke from external API
        /// </summary>
        public static async Task<string> GetJoke()
        {
            string text = "";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var tokenSource = new CancellationTokenSource();
                    tokenSource.CancelAfter(TimeSpan.FromSeconds(Globals.timeoutInSeconds));
                    var response = await client.GetAsync(GetRandomJokeUrl(), tokenSource.Token);
                    var json = await response.Content.ReadAsStringAsync();

                    var joke = JsonConvert.DeserializeObject<Joke>(json);

                    text = "\"" + joke.Text + "\"";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    text = "\"My friend thinks he is smart. He told me an onion is the only food that makes you cry, so I threw a coconut at his face\"";
                }
            }
            return text;
        }

        /// <summary>
        /// Get recipes by criteria from external API
        /// </summary>
        public static async Task<RecipesResults> GetRecipes(SearchRecipeDetails searchRecipeDetailsstring, int offset = 0)
        {

            RecipesResults recipes = new RecipesResults();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var tokenSource = new CancellationTokenSource();
                    tokenSource.CancelAfter(TimeSpan.FromSeconds(Globals.timeoutInSeconds));
                    var response = await client.GetAsync(GetSearchRecipesUrl(searchRecipeDetailsstring.RecipeName, searchRecipeDetailsstring.Cuisine, searchRecipeDetailsstring.Diet, searchRecipeDetailsstring.ExcludedIngredients, searchRecipeDetailsstring.Intolerance, offset), tokenSource.Token);
                    var json = await response.Content.ReadAsStringAsync();

                    recipes = JsonConvert.DeserializeObject<RecipesResults>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    recipes = null;
                }
            }

            return recipes;
        }

        /// <summary>
        /// Get substitutes of ingredient from external API
        /// </summary>
        public static async Task<IngredientSubstitutes> GetIngredientSubstitutes(string ingredient)
        {
            IngredientSubstitutes ingredientSubstitutes = new IngredientSubstitutes();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var tokenSource = new CancellationTokenSource();
                    tokenSource.CancelAfter(TimeSpan.FromSeconds(Globals.timeoutInSeconds));

                    var response = await client.GetAsync(GetIngredientSubstitutesUrl(ingredient), tokenSource.Token);
                    var json = await response.Content.ReadAsStringAsync();

                    ingredientSubstitutes = JsonConvert.DeserializeObject<IngredientSubstitutes>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ingredientSubstitutes = null;
                }
            }

            return ingredientSubstitutes;
        }

        /// <summary>
        /// Get random recipes from external API
        /// </summary>
        public static async Task<List<Recipe>> GetRandomRecipes(string tags)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var tokenSource = new CancellationTokenSource();
                    tokenSource.CancelAfter(TimeSpan.FromSeconds(Globals.timeoutInSeconds));
                    var response = await client.GetAsync(GetRandomRecipeUrl(tags), tokenSource.Token);
                    var json = await response.Content.ReadAsStringAsync();

                    var recipesRoot = JsonConvert.DeserializeObject<RandomRecipesResults>(json);

                    recipes = recipesRoot.Recipes as List<Recipe>;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    recipes = null;
                }
            }

            return recipes;
        }
        #endregion

        #region QueriesMethods
        /// <summary>
        /// generate recipes search query
        /// </summary>
        private static string GetSearchRecipesUrl(string query, string cicuisine = "", string diet = "", string excludeIngredients = "", string intolerances = "", int offset = 0)
        {
            return "https://api.spoonacular.com/recipes/search?apiKey=" + Globals.apiKey + "&query=" + query + "&cicuisine=" + cicuisine + "&diet=" + diet + "&excludeIngredients=" + excludeIngredients + "&intolerances=" + intolerances + "&offset=" + offset;
        }

        /// <summary>
        /// generate random recipes query
        /// </summary>
        private static string GetRandomRecipeUrl(string tags)
        {
            return "https://api.spoonacular.com/recipes/random?apiKey=" + Globals.apiKey + "&tags=" + tags + "&number=3";
        }

        /// <summary>
        /// generate substitutes of ingredient query
        /// </summary>
        private static string GetIngredientSubstitutesUrl(string ingredient)
        {
            return "https://api.spoonacular.com/food/ingredients/substitutes?apiKey=" + Globals.apiKey + "&ingredientName=" + ingredient;
        }

        /// <summary>
        /// generate joke query
        /// </summary>
        private static string GetRandomJokeUrl()
        {
            return "https://api.spoonacular.com/food/jokes/random?apiKey=" + Globals.apiKey;
        }
        #endregion
    }
}