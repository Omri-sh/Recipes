using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Recipes.Models;
using SQLite;

namespace Recipes.Utils
{
    public class DatabaseUtil
    {
        #region Properties
        /// <summary>
        /// Store the path of the DB
        /// </summary>
        private string dbPath;
        #endregion

        #region Constructor
        public DatabaseUtil(string path)
        {
            //Create Recipe table in DB if do not exist
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                conn.CreateTable<Recipe>();
            }
            dbPath = path;
        }
        #endregion

        #region CRUD Ops
        /// <summary>
        /// Insert recipe to Recipe table
        /// </summary>
        public int InsertRecipeToDb(Recipe recipe)
        {
            int rowsCount = 0;
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {

                rowsCount = conn.InsertOrReplace(recipe);
            }

            return rowsCount;
        }

        /// <summary>
        /// Delete recipe from Recipe table
        /// </summary>
        public int DeleteRecipeFromDb(Recipe recipe)
        {
            int rowsCount = 0;
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {

                rowsCount = conn.Delete(recipe);
            }

            return rowsCount;
        }

        /// <summary>
        /// Get all recipes from Recipe table
        /// </summary>
        public List<Recipe> GetRecipesFromDb()
        {
            List<Recipe> recipes;
            using (SQLiteConnection conn = new SQLiteConnection(Globals.GetDatabasePath()))
            {
                conn.CreateTable<Recipe>();
                recipes = conn.Table<Recipe>().ToList();
            }

            return recipes;
        }

        /// <summary>
        /// Get recipe by id from Recipe table
        /// </summary>
        public Recipe GetRecipeByIdFromDb(int id)
        {
            List<Recipe> recipes;
            using (SQLiteConnection conn = new SQLiteConnection(Globals.GetDatabasePath()))
            {
                conn.CreateTable<Recipe>();
                recipes = conn.Query<Recipe>("SELECT Id FROM Recipe WHERE Id = ?", id);

            }
            if(recipes.Count > 0)
                return recipes[0];
            return null;
        }
        #endregion
    }
}