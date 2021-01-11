using System;
using System.Collections.Generic;

namespace Recipes.Models
{
    public class IngredientSubstitutes
    {
        #region Properties
        public string Status { get; set; }
        public string Ingredient { get; set; }
        public List<string> Substitutes { get; set; }
        public string Message { get; set; }
        #endregion
    }
}