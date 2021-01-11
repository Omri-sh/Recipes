using System;
using System.Collections.Generic;
using SQLite;

namespace Recipes.Models
{
    public class Recipe
    {
        #region Properties
        [PrimaryKey]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(5)]
        public int ReadyInMinutes { get; set; }
        [MaxLength(50)]
        public string SourceUrl { get; set; }
        [MaxLength(50)]
        public string Image { get; set; }
        #endregion
    }

    public class RecipesResults
    {
        #region Properties
        public int Offset { get; set; }
        public int TotalResults { get; set; }
        public List<Recipe> Results { get; set; }
        #endregion

    }
    public class RandomRecipesResults
    {
        #region Properties
        public List<Recipe> Recipes { get; set; }
        #endregion
    }
}