using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Activities
{
    [Activity(Label = "DisplaySubstitutesActivity")]
    public class DisplaySubstitutesActivity : Activity
    {
        #region Overrided Mathods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_display_substitutes);
            IngredientSubstitutes ingredientSubstitutes = JsonConvert.DeserializeObject<IngredientSubstitutes>(Intent.GetStringExtra("ingredientSubstitutes"));
            TextView ingredientTextView = FindViewById<TextView>(Resource.Id.ingredientTextView);
            ingredientTextView.Text = "Substitues for " + ingredientSubstitutes.Ingredient + ":";
            ListView listView = FindViewById<ListView>(Resource.Id.substitutesListView);
            var arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.layout_substitute_row, ingredientSubstitutes.Substitutes);
            listView.Adapter = arrayAdapter;

            Toast.MakeText(Application.Context, ingredientSubstitutes.Message, ToastLength.Long).Show();
        }
        #endregion
    }
}