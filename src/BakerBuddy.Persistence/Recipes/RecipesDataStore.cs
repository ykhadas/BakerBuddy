using System.Transactions;
using BakerBuddy.Ports;
using BakerBuddy.Recipes;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BakerBuddy.Persistence.Recipes
{
    public class RecipesDataStore : IRecipesDataStore
    {
        private readonly IDbSettings _settings;

        public RecipesDataStore(IDbSettings settings)
        {
            _settings = settings;
        }

        public async Task<long> CreateRecipeAsync(Recipe recipe)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var recipeId = await CreateRecipe(recipe);
            await CreateIngredients(recipe, recipeId);
            transaction.Complete();
            return recipeId;
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
