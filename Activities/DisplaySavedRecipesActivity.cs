using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Recipes.Adapters;
using Recipes.Models;
using Recipes.Utils;

namespace Recipes.Activities
{
    [Activity(Label = "DisplaySavedRecipesActivity")]
    public class DisplaySavedRecipesActivity : Activity
    {
        #region Properties
        List<Recipe> recipes;
        DatabaseUtil dbUtil;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_display_recipes);

            dbUtil = new DatabaseUtil(Globals.GetDatabasePath());

            recipes = dbUtil.GetRecipesFromDb();

            ListView listView = FindViewById<ListView>(Resource.Id.recipesListView);
            listView.Adapter = new ListViewRecipesAdapter(this, recipes);
            listView.ItemClick += OnListItemClick;

            TextView randomTextView = FindViewById<TextView>(Resource.Id.randomTextView);
            randomTextView.Visibility = ViewStates.Gone;
            TextView savedTextView = FindViewById<TextView>(Resource.Id.savedTextView);
            savedTextView.Visibility = ViewStates.Visible;
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            //Reload activity to update list for deleted recipes
            Intent intent = Intent;
            Finish();
            StartActivity(intent);
        }
        #endregion

        #region On Click Methods
        /// <summary>
        /// Displays picked recipe
        /// </summary>
        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Recipe recipe = recipes[e.Position];
            Intent i = new Intent(this, typeof(DisplayRecipeActivity));
            i.PutExtra("recipe", JsonConvert.SerializeObject(recipe));
            StartActivity(i);
        }
        #endregion
    }
}