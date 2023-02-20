using BakerBuddy.Recipes;

namespace BakerBuddy.Ports
{
    public interface IRecipesDataStore
    {
        Task<long> CreateRecipeAsync(Recipe recipe);
    }
}
