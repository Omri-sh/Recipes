using System;
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
    [Activity(Label = "SearchIngredientSubstitutesActivity")]
    public class SearchIngredientSubstitutesActivity : Activity
    {
        #region Properties
        EditText ingredientSubstitutesEditText;
        ProgressDialog progressDialog;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_search_ingredient_substitutes);

            progressDialog = new ProgressDialog(this);

            Button searchIngredientSubstitutesActivityButton = FindViewById<Button>(Resource.Id.searchIngredientSubstitutesActivityButton);
            searchIngredientSubstitutesActivityButton.Click += SearchIngredientSubstitutesButtonClick;
            ingredientSubstitutesEditText = FindViewById<EditText>(Resource.Id.ingredientSubstitutesEditText);
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
        private async void SearchIngredientSubstitutesButtonClick(object sender, EventArgs e)
        {
            //Hide keyboard
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            if (ingredientSubstitutesEditText.Text == "")
            {
                Toast.MakeText(Application.Context, "Please enter an ingredient", ToastLength.Long).Show();
                return;
            }
            progressDialog = ProgressDialog.Show(this, "Please wait...", "Searching for recipes...", true);
            IngredientSubstitutes ingredientSubstitutes = await RecipesApiCalls.GetIngredientSubstitutes(ingredientSubstitutesEditText.Text);
            if (ingredientSubstitutes == null)
            {
                progressDialog.Cancel();
                Toast.MakeText(Application.Context, "Please check your internet connection", ToastLength.Long).Show();
            }
            else if (ingredientSubstitutes.Status == "failure")
            {
                progressDialog.Cancel();
                Toast.MakeText(Application.Context, ingredientSubstitutes.Message, ToastLength.Long).Show();
            }
            else
            {
                Intent i = new Intent(this, typeof(DisplaySubstitutesActivity));
                i.PutExtra("ingredientSubstitutes", JsonConvert.SerializeObject(ingredientSubstitutes));
                StartActivity(i);
            }
        }
        #endregion
    }
}