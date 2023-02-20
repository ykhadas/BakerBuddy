using AutoFixture;
using BakerBuddy.Ports;
using BakerBuddy.Recipes;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BakerBuddy.Tests.Recipes
{
    public class CreateRecipeUseCaseUnitTests
    {
        private readonly Mock<IRecipesDataStore> _recipesDataStoreMock = new();
        private readonly Mock<ILogger<CreateRecipeUseCase>> _loggerMock = new();

        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Execute_WhenRecipeInformationIsProvided_ReturnsNewRecipeId()
        {
            var recipeId = _fixture.Create<long>();
            var recipe = _fixture.Create<Recipe>();

            _recipesDataStoreMock.Setup(d => d.CreateRecipeAsync(It.IsAny<Recipe>())).ReturnsAsync(recipeId);

            var result = await CreateSut().ExecuteAsync(recipe);

            using (new AssertionScope())
            {
                _recipesDataStoreMock.Verify(m => m.CreateRecipeAsync(It.IsAny<Recipe>()));
                result.Should().Be(recipeId);
            }
        }

        [Fact]
        public async Task Execute_WhenRecipeInformationIsProvided_CallsDataStoreWithCalculatedHydrationLevelAndBakerPercentage()
        {
            var recipeId = _fixture.Create<long>();

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
            );

            var dataStoreRecipeRequest = new List<Recipe>();

            _recipesDataStoreMock.Setup(d => d.CreateRecipeAsync(Capture.In(dataStoreRecipeRequest))).ReturnsAsync(recipeId);

            await CreateSut().ExecuteAsync(recipe);

            using (new AssertionScope())
            {
                var flourAmount = ingredients.Where(i => i.IngredientType == IngredientType.Flour).Sum(i => i.Gramms);
                var waterAmount = ingredients.Where(i => i.IngredientType == IngredientType.Water).Sum(i => i.Gramms);

                dataStoreRecipeRequest.Should().NotBeEmpty().And.HaveCount(1);

                var request = dataStoreRecipeRequest.First();

                var expectedLevel = 100 * (waterAmount / flourAmount);

                request.HydrationLevel.Should().Be(expectedLevel);

                foreach (var ingredient in request.Ingredients)
                {
                    var expectedPercentage = 100 * (ingredient.Gramms / flourAmount);
                    ingredient.BakerPercentage.Should().Be(expectedPercentage);
                }
            }
        }

        private Ingredient CreateTestIngredient(IngredientType type) => Ingredient.Make(
            name: _fixture.Create<string>(),
            gramms: _fixture.Create<int>(),
            ingredientType: type
        );

        [Fact]
        public async Task Execute_WhenRepositoryThrowsException_RethrowsException()
        {
            var recipe = _fixture.Create<Recipe>();

            _recipesDataStoreMock.Setup(d => d.CreateRecipeAsync(It.IsAny<Recipe>())).ThrowsAsync(new Exception("Exception"));

            Func<Task> call = async () => await CreateSut().ExecuteAsync(recipe);

            await call.Should().ThrowExactlyAsync<Exception>();
        }

        private ICreateRecipeUseCase CreateSut() => new CreateRecipeUseCase(_recipesDataStoreMock.Object, _loggerMock.Object);
    }
}
