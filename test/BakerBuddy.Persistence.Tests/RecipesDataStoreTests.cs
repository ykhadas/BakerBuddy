using AutoFixture;
using BakerBuddy.Persistence.Recipes;
using BakerBuddy.Ports;
using BakerBuddy.Recipes;
using FluentAssertions;
using Xunit;

namespace BakerBuddy.Persistence.Tests
{
    public class RecipesDataStoreTests
    {
        private readonly Fixture _fixture = new();
        
        [Fact(Skip = "Skipping for now")]
        public async Task CreateRecipeAsync_ReturnsCreatedRecipeId()
        {
            var flourIngredient1 = CreateTestIngredient(IngredientType.Flour);
            var flourIngredient2 = CreateTestIngredient(IngredientType.Flour);
            var waterIngredient1 = CreateTestIngredient(IngredientType.Water);
            var waterIngredient2 = CreateTestIngredient(IngredientType.Water);
            var otherIngredient = CreateTestIngredient(IngredientType.Other);

            var ingredients = new[] { flourIngredient1, flourIngredient2, waterIngredient1, waterIngredient2, otherIngredient };

            var recipe = Recipe.Make(
                userId: _fixture.Create<ulong>(),
                name: _fixture.Create<string>(),
                description: _fixture.Create<string>(),
                ingredients: ingredients
            ).WithBakerPercentage().WithHydrationLevel();

            var recipeId = await CreateSut().CreateRecipeAsync(recipe);

            //In the real life this test would be smarter: checking if the recipe entity is created with all the ingredients
            recipeId.Should().BeGreaterThan(0);
        }

        private Ingredient CreateTestIngredient(IngredientType type) => Ingredient.Make(
            name: _fixture.Create<string>(),
            gramms: _fixture.Create<int>(),
            ingredientType: type
        );

        private IRecipesDataStore CreateSut()
        {
            return new RecipesDataStore(TestSettings.Instance);
        }
    }
}
