using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Activities
{
    [Activity(Label = "SearchRecipeActivity")]
    public class SearchRecipeActivity : Activity
    {
        #region Properties
        EditText recipeEditText;
        EditText excludedIngredientsEditText;
        Spinner cuisineSpinner;
        Spinner dietSpinner;
        Spinner intoleranceSpinner;
        ProgressDialog progressDialog;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_search_recipe);

            progressDialog = new ProgressDialog(this);

            Button searchReciptActivityButton = FindViewById<Button>(Resource.Id.searchReciptActivityButton);
            searchReciptActivityButton.Click += SearchRecipeClick; 

            recipeEditText = FindViewById<EditText>(Resource.Id.recipeEditText);

            excludedIngredientsEditText = FindViewById<EditText>(Resource.Id.excludedIngredientsEditText);

            cuisineSpinner = FindViewById<Spinner>(Resource.Id.cuisineSpinner);
            var cuisineAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.cuisines_array, Resource.Drawable.spinner_drawable);
            cuisineSpinner.Adapter = cuisineAdapter;

            dietSpinner = FindViewById<Spinner>(Resource.Id.dietSpinner);
            var dietAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.diets_array, Resource.Drawable.spinner_drawable);
            dietSpinner.Adapter = dietAdapter;

            intoleranceSpinner = FindViewById<Spinner>(Resource.Id.intoleranceSpinner);
            var intoleranceAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.intolerances_array, Resource.Drawable.spinner_drawable);
            intoleranceSpinner.Adapter = intoleranceAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();
            //Cancel progress dialog if returned to this activity
            if (progressDialog.IsShowing)
                progressDialog.Cancel();
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Get Selected item from Spinner.
        /// </summary>
        /// <returns>
        /// Returns selected item, otherwise returns empty string
        /// </returns>
        private string GetSelectedItemFromSpinner(Spinner spinner)
        {
            if (spinner.SelectedItemPosition == 0)
                return "";
            return spinner.SelectedItem.ToString();
        }
        #endregion

        #region On Click Methods
        private async void SearchRecipeClick(object sender, EventArgs e)
        {
            //Hide keyboard
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            if (recipeEditText.Text == "")
            {
                Toast.MakeText(Application.Context, "Recipe's name is mandatory!", ToastLength.Long).Show();
                return;
            }
            progressDialog = ProgressDialog.Show(this, "Please wait...", "Searching for recipes...", true);

            SearchRecipeDetails searchRecipeDetails = new SearchRecipeDetails(recipeEditText.Text, GetSelectedItemFromSpinner(cuisineSpinner), GetSelectedItemFromSpinner(dietSpinner), excludedIngredientsEditText.Text, GetSelectedItemFromSpinner(intoleranceSpinner));

            RecipesResults recipes = await RecipesApiCalls.GetRecipes(searchRecipeDetails);
            if (recipes == null)
            {
                progressDialog.Cancel();
                Toast.MakeText(Application.Context, "Please check your internet connection", ToastLength.Long).Show();
            }
            else if (recipes.Results.Count == 0)
            {
                progressDialog.Cancel();
                Toast.MakeText(Application.Context, "No recipes found that match your query", ToastLength.Long).Show();
            }
            else
            {
                Intent i = new Intent(this, typeof(DisplaySearchedRecipesListActivity));
                i.PutExtra("recipes", JsonConvert.SerializeObject(recipes));
                i.PutExtra("searchRecipeDetails", JsonConvert.SerializeObject(searchRecipeDetails));
                StartActivity(i);
            }
        }
        #endregion
    }
}