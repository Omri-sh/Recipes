using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Recipes.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Recipes.Utils;

namespace Recipes.Activities
{

    [Activity(Label = "DisplayRecipeActivity")]
    public class DisplayRecipeActivity : Activity
    {
        #region Properties
        WebView webView;
        Recipe recipe;
        Button saveRecipeButton;
        Button deleteRecipeButton;
        DatabaseUtil dbUtil;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_web_view);
            recipe = JsonConvert.DeserializeObject<Recipe>(Intent.GetStringExtra("recipe"));

            saveRecipeButton = FindViewById<Button>(Resource.Id.saveRecipeButton);
            saveRecipeButton.Click += SaveRecipeButtonClick;

            deleteRecipeButton = FindViewById<Button>(Resource.Id.deleteRecipeButton);
            deleteRecipeButton.Click += DeleteRecipeButtonClick;

            dbUtil = new DatabaseUtil(Globals.GetDatabasePath());

            if (dbUtil.GetRecipeByIdFromDb(recipe.Id) != null)
                SetAsDeleteRecipeButton();

            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new RecipeWebViewClient());
            webView.LoadUrl(recipe.SourceUrl);

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                Toast.MakeText(Application.Context, "Please check your internet connection", ToastLength.Short).Show();
            }
        }
        //Added functionality to return to previous page with back buttn
        public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            if (keyCode == Keycode.Back && webView.CanGoBack())
            {
                webView.GoBack();
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Change delete button to save button
        /// </summary>
        private void SetAsSaveRecipeButton()
        {
            saveRecipeButton.Visibility = ViewStates.Visible;
            deleteRecipeButton.Visibility = ViewStates.Gone;
        }

        /// <summary>
        /// Change save button to delete button
        /// </summary>
        private void SetAsDeleteRecipeButton()
        {
            saveRecipeButton.Visibility = ViewStates.Gone;
            deleteRecipeButton.Visibility = ViewStates.Visible;
        }
        #endregion

        #region On Click Methods
        private void SaveRecipeButtonClick(object sender, EventArgs e)
        {
            int rowsCount = dbUtil.InsertRecipeToDb(recipe);

            if (rowsCount > 0)
            {
                Toast.MakeText(Application.Context, "The recipe is saved", ToastLength.Short).Show();
                SetAsDeleteRecipeButton();
            }
            else
                Toast.MakeText(Application.Context, "An error occured while saving the recipe", ToastLength.Short).Show();
        }
        private void DeleteRecipeButtonClick(object sender, EventArgs e)
        {
            int rowsCount = dbUtil.DeleteRecipeFromDb(recipe);

            if (rowsCount > 0)
            {
                Toast.MakeText(Application.Context, "The recipe is deleted", ToastLength.Short).Show();
                SetAsSaveRecipeButton();
            }

            else
                Toast.MakeText(Application.Context, "An error occured while deleting the recipe", ToastLength.Short).Show();
        }
        #endregion
    }
    public class RecipeWebViewClient : WebViewClient
    {
        #region Overrided Methods
        //This function return false so base class will handle the url loading
        [Obsolete]
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return false;
        }
        #endregion
    }
}