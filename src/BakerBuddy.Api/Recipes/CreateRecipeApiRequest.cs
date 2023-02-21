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
        public string Name { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        [Required]
        public IReadOnlyCollection<IngredientApiRequest> Ingredients { get; set; } = default!;

        public Recipe ToRecipe()
        {
            return Recipe.Make(UserId, Name, Description, Ingredients.Select(i=>i.ToIngredientRequest()).ToArray());
        }
    }
}
