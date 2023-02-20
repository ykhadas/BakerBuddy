using AutoFixture;
using BakerBuddy.Exceptions;
using BakerBuddy.Recipes;
using FluentAssertions;
using Xunit;

namespace BakerBuddy.Tests.Recipes
{
    public class IngredientUnitTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void WithBakerPercentage_CalculatesBakersPercentage()
        {
            int flourInGramms = _fixture.Create<int>();

            var sut = Ingredient.Make(
                name: _fixture.Create<string>(),
                gramms: _fixture.Create<int>(),
                ingredientType: IngredientType.Water
            );

            var result = sut.WithBakerPercentage(flourInGramms);

            var expectedPercentage = 100 * (sut.Gramms / flourInGramms);

            result.BakerPercentage.Should().Be(expectedPercentage);
        }

        [Fact]
        public void WithBakerPercentage_WhenFlourAmountIsIncorrect_Throws()
        {
            int flourInGramms = 0;

            var sut = Ingredient.Make(
                name: _fixture.Create<string>(),
                gramms: _fixture.Create<int>(),
                ingredientType: IngredientType.Water
            );

            var action = () => sut.WithBakerPercentage(flourInGramms);

            action.Should().Throw<InvalidIngredientProportionException>();
        }
    }
}
