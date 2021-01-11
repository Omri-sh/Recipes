using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content;
using Recipes.Activities;

namespace Recipes
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            TextView jokeTextView = FindViewById<TextView>(Resource.Id.jokeTextView);
            jokeTextView.Text = Intent.GetStringExtra("joke");

            Button searchRecipeButton = FindViewById<Button>(Resource.Id.searchRecipeButton);
            searchRecipeButton.Click += SearchRecipeButtonClick;

            Button randomRecipeButton = FindViewById<Button>(Resource.Id.randomRecipeButton);
            randomRecipeButton.Click += RandomRecipeButtonClick;

            Button ingredientSubstitutesButton = FindViewById<Button>(Resource.Id.ingredientSubstitutesButton);
            ingredientSubstitutesButton.Click += IngredientSubstitutesButtonClick;

            Button savedRecipesButton = FindViewById<Button>(Resource.Id.savedRecipesButton);
            savedRecipesButton.Click += SavedRecipesButtonClick;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion

        #region On Click Methods
        private void SearchRecipeButtonClick(object sender, EventArgs e)
        {
            StartActivity(new Intent(Application.Context, typeof(SearchRecipeActivity)));
        }

        private void RandomRecipeButtonClick(object sender, EventArgs e)
        {
            StartActivity(new Intent(Application.Context, typeof(RandomRecipeActivity)));
        }

        private void IngredientSubstitutesButtonClick(object sender, EventArgs e)
        {
            StartActivity(new Intent(Application.Context, typeof(SearchIngredientSubstitutesActivity)));
        }

        private void SavedRecipesButtonClick(object sender, EventArgs e)
        {
            StartActivity(new Intent(Application.Context, typeof(DisplaySavedRecipesActivity)));
        }
        #endregion
    }
}