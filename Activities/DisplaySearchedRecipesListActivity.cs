using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Recipes.Adapters;
using Recipes.Models;

namespace Recipes.Activities
{
    [Activity(Label = "DisplayRecipesListActivity")]
    public class DisplaySearchedRecipesListActivity : Activity
    {
        #region Properties
        RecipesResults recipes;
        SearchRecipeDetails searchRecipeDetails;
        ListView listView;
        ProgressDialog progressDialog;
        Button previousRecipesButton;
        Button nextRecipesButton;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_display_searched_recipes);

            recipes = JsonConvert.DeserializeObject<RecipesResults>(Intent.GetStringExtra("recipes"));
            listView = FindViewById<ListView>(Resource.Id.recipesListView);
            listView.Adapter = new ListViewRecipesAdapter(this, recipes.Results);
            listView.ItemClick += OnListItemClick;

            progressDialog = new ProgressDialog(this);

            searchRecipeDetails = JsonConvert.DeserializeObject<SearchRecipeDetails>(Intent.GetStringExtra("searchRecipeDetails"));

            previousRecipesButton = FindViewById<Button>(Resource.Id.previousRecipesButton);
            previousRecipesButton.Click += PreviousRecipesButtonClick;

            nextRecipesButton = FindViewById<Button>(Resource.Id.nextRecipesButton);
            nextRecipesButton.Click += NextRecipesButtonClick;

            ButtonVisibilityHandler();
        }
        #endregion

        #region Unility Methods
        /// <summary>
        /// Handles previous/next buttons according to offset and results
        /// </summary>
        private void ButtonVisibilityHandler()
        {
            previousRecipesButton.Visibility = (recipes.Offset < Globals.offset ? ViewStates.Gone : ViewStates.Visible);
            nextRecipesButton.Visibility = (recipes.TotalResults <= Globals.offset ? ViewStates.Gone : ViewStates.Visible);
        }
        /// <summary>
        /// Fetches recipes by offset(previous or next results)
        /// </summary>
        private async void FetchRecipes(int offset)
        {
            progressDialog = ProgressDialog.Show(this, "Please wait...", "Searching for recipes...", true);

            recipes = await RecipesApiCalls.GetRecipes(searchRecipeDetails, offset);
            listView.Adapter = new ListViewRecipesAdapter(this, recipes.Results);
            ButtonVisibilityHandler();
            progressDialog.Cancel();
        }
        #endregion

        #region On Click Methods
        private void NextRecipesButtonClick(object sender, EventArgs e)
        {
            FetchRecipes(recipes.Offset + Globals.offset);
        }

        private void PreviousRecipesButtonClick(object sender, EventArgs e)
        {
            FetchRecipes(recipes.Offset - Globals.offset);
        }

        /// <summary>
        /// Displays picked recipe
        /// </summary>
        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Recipe recipe = recipes.Results[e.Position];
            Intent i = new Intent(this, typeof(DisplayRecipeActivity));
            i.PutExtra("recipe", JsonConvert.SerializeObject(recipe));
            StartActivity(i);
        }
        #endregion
    }
}