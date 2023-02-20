using AutoFixture;
using BakerBuddy.Exceptions;
using BakerBuddy.Recipes;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace BakerBuddy.Tests.Recipes
{
    public class RecipeUnitTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void WithHydrationLevel_CalculatesHydrationLevel()
        {
            var flourIngredient1 = CreateTestIngredient(IngredientType.Flour);
            var flourIngredient2 = CreateTestIngredient(IngredientType.Flour);
            var waterIngredient1 = CreateTestIngredient(IngredientType.Water);
            var waterIngredient2 = CreateTestIngredient(IngredientType.Water);
            var otherIngredient = CreateTestIngredient(IngredientType.Other);

            var ingredients = new[] { flourIngredient1, flourIngredient2, waterIngredient1, waterIngredient2, otherIngredient };

            var sut = Recipe.Make(
                userId: _fixture.Create<ulong>(),
                name: _fixture.Create<string>(),
                description: _fixture.Create<string>(),
                ingredients: ingredients
            );

            var result = sut.WithHydrationLevel();

            var expectedLevel = 100 * (ingredients.Where(i => i.IngredientType == IngredientType.Water).Sum(i => i.Gramms) /
                                       ingredients.Where(i => i.IngredientType == IngredientType.Flour).Sum(i => i.Gramms));

            result.HydrationLevel.Should().Be(expectedLevel);
        }

        private Ingredient CreateTestIngredient(IngredientType type) => Ingredient.Make(
            name: _fixture.Create<string>(),
            gramms: _fixture.Create<int>(),
            ingredientType: type
        );

        [Fact]
        public void WithHydrationLevel_WhenFlourAmountIsIncorrect_Throws()
        {
            var waterIngredient1 = CreateTestIngredient(IngredientType.Water);
            var waterIngredient2 = CreateTestIngredient(IngredientType.Water);
            var otherIngredient = CreateTestIngredient(IngredientType.Other);

            var ingredients = new[] { waterIngredient1, waterIngredient2, otherIngredient };

            var sut = Recipe.Make(
                userId: _fixture.Create<ulong>(),
                name: _fixture.Create<string>(),
                description: _fixture.Create<string>(),
                ingredients: ingredients
            );

            var action = () => sut.WithHydrationLevel();

            action.Should().Throw<InvalidIngredientProportionException>().WithMessage("Flour ingredient should be present in the recipe.");
        }

        [Fact]
        public void WithBakerPercentage_CalculatesPercentageForEachIngredient()
        {
            var flourIngredient1 = CreateTestIngredient(IngredientType.Flour);
            var waterIngredient1 = CreateTestIngredient(IngredientType.Water);
            var waterIngredient2 = CreateTestIngredient(IngredientType.Water);
            var otherIngredient = CreateTestIngredient(IngredientType.Other);

            var ingredients = new[] { flourIngredient1, waterIngredient1, waterIngredient2, otherIngredient };

            var sut = Recipe.Make(
                userId: _fixture.Create<ulong>(),
                name: _fixture.Create<string>(),
                description: _fixture.Create<string>(),
                ingredients: ingredients
            );

            var result = sut.WithBakerPercentage();

            using var _ = new AssertionScope();

            foreach (var ingredient in result.Ingredients)
            {
                var expectedPercentage = 100 * (ingredient.Gramms / flourIngredient1.Gramms);
                ingredient.BakerPercentage.Should().Be(expectedPercentage);
            }
        }

        [Fact]
        public void WithBakerPercentage_WhenFlourAmountIsIncorrect_Throws()
        {
            var waterIngredient1 = CreateTestIngredient(IngredientType.Water);
            var waterIngredient2 = CreateTestIngredient(IngredientType.Water);
            var otherIngredient = CreateTestIngredient(IngredientType.Other);

            var ingredients = new[] { waterIngredient1, waterIngredient2, otherIngredient };

            var sut = Recipe.Make(
                userId: _fixture.Create<ulong>(),
                name: _fixture.Create<string>(),
                description: _fixture.Create<string>(),
                ingredients: ingredients
            );

            var action = () => sut.WithBakerPercentage();

            action.Should().Throw<InvalidIngredientProportionException>().WithMessage("Flour ingredient should be present in the recipe.");
        }
    }
}