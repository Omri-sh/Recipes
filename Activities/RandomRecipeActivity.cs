using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Activities
{
    [Activity(Label = "RandomRecipeActivity")]
    public class RandomRecipeActivity : Activity
    {
        #region Properties
        EditText randomRecipeEditText;
        ProgressDialog progressDialog;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_random_recipe);

            progressDialog = new ProgressDialog(this);

            Button randomReciptActivityButton = FindViewById<Button>(Resource.Id.randomReciptActivityButton);
            randomReciptActivityButton.Click += RandomRecipeClick;

            randomRecipeEditText = FindViewById<EditText>(Resource.Id.randomRecipeEditText);
        }

        protected override void OnResume()
        {
            base.OnResume();
            //Cancel progress dialog if returned to this activity
            if (progressDialog.IsShowing)
                progressDialog.Cancel();
        }
        #endregion

        #region On Click Methods
        private async void RandomRecipeClick(object sender, EventArgs e)
        {
            //Hide keyboard
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            if (randomRecipeEditText.Text == "")
            {
                Toast.MakeText(Application.Context, "Please enter which type of recipe you want", ToastLength.Long).Show();
                return;
            }
            progressDialog = ProgressDialog.Show(this, "Please wait...", "Searching for recipes...", true);
            List<Recipe> recipes = await RecipesApiCalls.GetRandomRecipes(randomRecipeEditText.Text);
            if (recipes == null)
            {
                progressDialog.Cancel();
                Toast.MakeText(Application.Context, "Please check your internet connection", ToastLength.Long).Show();
            }
            else if(recipes.Count == 0)
            {
                progressDialog.Cancel();
                Toast.MakeText(Application.Context, "No recipes found that match your query", ToastLength.Long).Show();
            }
            else
            {
                Intent i = new Intent(this, typeof(DisplayRandomRecipesListActivity));
                i.PutExtra("recipes", JsonConvert.SerializeObject(recipes));
                StartActivity(i);
            }
        }
        #endregion
    }
}