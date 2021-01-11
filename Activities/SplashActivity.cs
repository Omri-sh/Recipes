using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes.Activities
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        #region Overrided Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task jokeTask = new Task(async () => { 
                string joke = await RecipesApiCalls.GetJoke();
                GoToMainActivity(joke);
            });
            jokeTask.Start();
        }
        #endregion

        #region Utility Methods
        protected void GoToMainActivity(string joke)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            i.PutExtra("joke", joke);
            StartActivity(i);
        }
        #endregion
    }
}