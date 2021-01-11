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

namespace Recipes.Activities
{
    [Activity(Label = "DisplayRandomRecipesListActivity")]
    public class DisplayRandomRecipesListActivity : Activity
    {
        #region Properties
        List<Recipe> recipes;
        ListView listView;
        #endregion

        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_display_recipes);

            recipes = JsonConvert.DeserializeObject<List<Recipe>>(Intent.GetStringExtra("recipes"));
            listView = FindViewById<ListView>(Resource.Id.recipesListView);
            listView.Adapter = new ListViewRecipesAdapter(this, recipes);
            listView.ItemClick += OnListItemClick;

            TextView randomTextView = FindViewById<TextView>(Resource.Id.randomTextView);
            randomTextView.Visibility = ViewStates.Visible;
            TextView savedTextView = FindViewById<TextView>(Resource.Id.savedTextView);
            savedTextView.Visibility = ViewStates.Gone;
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