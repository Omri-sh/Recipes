using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Recipes.Models;

namespace Recipes.Adapters
{
    public class ListViewRecipesAdapter : BaseAdapter<Recipe>
    {
        #region Properties
        List<Recipe> items;
        Activity context;
        #endregion

        #region Constructor
        public ListViewRecipesAdapter(Activity context, List<Recipe> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        #endregion

        #region Public Methods
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Recipe this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Manange view of a row in the list view
        /// </summary>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.layout_recipe_row, null);
            view.FindViewById<TextView>(Resource.Id.primaryTitleText).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.secondaryTitleText).Text = "Ready in " + item.ReadyInMinutes + " minutes";
            Bitmap imageBitmap = GetImageBitmapFromUrl(GenerateRecipeImageUrl(item.Image));
            view.FindViewById<ImageView>(Resource.Id.image).SetImageBitmap(imageBitmap);
            return view;
        }
        #endregion

        #region Private Methods
        private class WebClient : System.Net.WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                return lWebRequest;
            }
        }

        /// <summary>
        /// Get bitmap image from url
        /// </summary>
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Timeout = Globals.timeoutInSeconds * 1000;
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return imageBitmap;
        }

        /// <summary>
        /// Get recipe image url
        /// </summary>
        /// <returns>
        /// If valid url, then returns it, otherwise add external API path with the image path
        /// </returns>
        private string GenerateRecipeImageUrl(string imageUrl)
        {
            if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                return imageUrl;
            else
                return "https://spoonacular.com/recipeImages/" + imageUrl;
        }
        #endregion
    }
}