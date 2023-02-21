using System.ComponentModel.DataAnnotations;
using BakerBuddy.Recipes;

namespace BakerBuddy.Api.Recipes
{
    public class IngredientApiRequest
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public int Gramms { get; set; } = default!;

        [Required]
        public IngredientType IngredientType { get; set; }

        public Ingredient ToIngredientRequest()
        {
            return Ingredient.Make(Name, Gramms, (BakerBuddy.Recipes.IngredientType)(short)IngredientType);
        }
    }
}
