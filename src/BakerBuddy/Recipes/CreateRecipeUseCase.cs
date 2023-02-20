using BakerBuddy.Ports;
using Microsoft.Extensions.Logging;

namespace BakerBuddy.Recipes
{
    public interface ICreateRecipeUseCase
    {
        Task<long> ExecuteAsync(Recipe recipe);
    }

    public class CreateRecipeUseCase: ICreateRecipeUseCase
    {
        private readonly IRecipesDataStore _recipesDataStore;
        private readonly ILogger<CreateRecipeUseCase> _logger;

        public CreateRecipeUseCase(IRecipesDataStore recipesDataStore, ILogger<CreateRecipeUseCase> logger)
        {
            _recipesDataStore = recipesDataStore;
            _logger = logger;
        }

        public async Task<long> ExecuteAsync(Recipe recipe)
        {
            try
            {
                var recipeId = await _recipesDataStore.CreateRecipeAsync(recipe.WithHydrationLevel().WithBakerPercentage());
                LogSuccess(recipe);
                return recipeId;
            }
            catch (Exception e)
            {
                LogFailure(recipe, e);
                throw;
            }
        }

        private void LogSuccess(Recipe recipe) =>
            _logger.LogInformation("Successfully created recipe {recipe}.", recipe);

        private void LogFailure(Recipe recipe, Exception exception) =>
            _logger.LogError(exception: exception, "An exception occurred while creating a recipe {recipe}", recipe);
    }
}
