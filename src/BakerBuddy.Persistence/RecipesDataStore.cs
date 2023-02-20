using BakerBuddy.Ports;
using BakerBuddy.Recipes;

namespace BakerBuddy.Persistence.Recipes
{
    public class RecipesDataStore: IRecipesDataStore
    {
        public async Task<long> CreateRecipeAsync(Recipe recipe)
        {
            return await Task.FromResult(new Random().Next());
        }
    }
}
