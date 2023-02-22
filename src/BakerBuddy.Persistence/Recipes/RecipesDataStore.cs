using System.Transactions;
using BakerBuddy.Persistence.Exceptions;
using BakerBuddy.Ports;
using BakerBuddy.Recipes;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace BakerBuddy.Persistence.Recipes
{
    public class RecipesDataStore : IRecipesDataStore
    {
        private readonly IDbSettings _settings;
        private readonly ILogger<RecipesDataStore> _logger;

        public RecipesDataStore(ILogger<RecipesDataStore> logger, IDbSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task<long> CreateRecipeAsync(Recipe recipe)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var recipeId = await CreateRecipe(recipe);
                await CreateIngredients(recipe, recipeId);
                transaction.Complete();
                return recipeId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while executing query to create a recipe");
                throw new PersistenceException("Exception occurred while persisting a recipe.");
            }
        }

        public async Task CreateIngredients(Recipe recipe, long recipeId)
        {
            await using var conn = new SqlConnection(_settings.DbConnectionString);

            foreach (var ingredient in recipe.Ingredients)
            {
                var ingredientsDynamicParameters = new DynamicParameters(
                    new
                    {
                        RecipeId = recipeId,
                        ingredient.Gramms,
                        ingredient.Name,
                        ingredient.BakerPercentage
                    }
                );

                await conn.ExecuteAsync(
                     sql: @"INSERT INTO [dbo].[Ingredient] (RecipeId, Name, Gramms, BakerPercentage)
                                                    VALUES (@RecipeId, @Name, @Gramms, @BakerPercentage);",
                     param: ingredientsDynamicParameters
                 );
            }
        }

        public async Task<long> CreateRecipe(Recipe recipe)
        {
            await using var conn = new SqlConnection(_settings.DbConnectionString);

            var recipeDynamicParameters = new DynamicParameters(
                new
                {
                    UserId = (long)recipe.UserId,
                    recipe.Name,
                    recipe.Description,
                    recipe.HydrationLevel
                }
            );

            var recipeId = await conn.QuerySingleAsync<long>(
                sql: @"INSERT INTO [dbo].[Recipe] (UserId, Name, Description, HydrationLevel)
                                                    output inserted.Id 
                                                    VALUES (@UserId, @Name, @Description, @HydrationLevel);",
                param: recipeDynamicParameters
            );

            return recipeId;
        }
    }
}
