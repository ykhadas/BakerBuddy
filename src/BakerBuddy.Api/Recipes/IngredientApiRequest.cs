using System.ComponentModel.DataAnnotations;
using BakerBuddy.Recipes;

namespace BakerBuddy.Api.Recipes
{
    public class IngredientApiRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Gramms { get; set; }

        [Required]
        public IngredientType IngredientType { get; set; }

        public Ingredient ToIngredientRequest()
        {
            return Ingredient.Make(Name, Gramms, (BakerBuddy.Recipes.IngredientType)(short)IngredientType);
        }
    }
}
