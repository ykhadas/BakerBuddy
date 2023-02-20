using System.ComponentModel.DataAnnotations;
using BakerBuddy.Recipes;

namespace BakerBuddy.Api.Recipes
{
    public class CreateRecipeApiRequest
    {
        [Required]
        [Range(1, long.MaxValue)]
        public ulong UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IReadOnlyCollection<IngredientApiRequest> Ingredients { get; set; }

        public Recipe ToRecipe()
        {
            return Recipe.Make(UserId, Name, Description, Ingredients.Select(i=>i.ToIngredientRequest()).ToArray());
        }
    }
}
