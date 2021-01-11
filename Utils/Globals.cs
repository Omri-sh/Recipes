using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views.InputMethods;
using Newtonsoft.Json;
using Recipes.Models;

namespace Recipes
{
    public static class Globals
    {
        #region Properties
        /// <summary>
        /// Api key, used for external api queries
        /// </summary>
        public const string apiKey = "UseYourOwnApiKey";
        /// <summary>
        /// Internal DB file name
        /// </summary>
        private const string dbName = "recipes_db.sqlite";
        /// <summary>
        /// The folder name of application
        /// </summary>
        private readonly static string folderName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        /// <summary>
        /// Number of results to return from external API
        /// </summary>
        public const int offset = 10;
        /// <summary>
        /// Timeout in seconds of API call
        /// </summary>
        public const int timeoutInSeconds = 10;
        #endregion

        #region Methods
        /// <summary>
        /// Generate DB path on file system
        /// </summary>
        public static string GetDatabasePath()
        {
           return Path.Combine(folderName, dbName);
        }
        #endregion
    }
}