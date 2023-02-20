using AutoFixture;
using BakerBuddy.Api.Recipes;
using BakerBuddy.Exceptions;
using BakerBuddy.Recipes;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BakerBuddy.Api.Tests
{
    public class RecipeControllerUnitTests
    {
        private readonly Mock<ICreateRecipeUseCase> _createRecipeUseCaseMock = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Create_WhenUseCaseSucceeds_ReturnsOkWithContent()
        {
            var recipeId = _fixture.Create<long>();

            var expectedResponse = new CreateRecipeResponse(recipeId);

            _createRecipeUseCaseMock.Setup(u => u.ExecuteAsync(It.IsAny<Recipe>())).ReturnsAsync(recipeId);

            var result = await CreateSut().Create(_fixture.Create<CreateRecipeApiRequest>());

            using (new AssertionScope())
            {
                var value = result.Should().BeOfType<OkObjectResult>().Which.Value;
                value.Should().BeEquivalentTo(
                    expectedResponse
                );
            }
        }

        [Fact]
        public async Task Create_WhenUseCaseThrowsUnhandledException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = new Exception("Exception message");

            _createRecipeUseCaseMock.Setup(u => u.ExecuteAsync(It.IsAny<Recipe>())).ThrowsAsync(exception);

            var result = await CreateSut().Create(_fixture.Create<CreateRecipeApiRequest>());

            using (new AssertionScope())
            {
                var apiResult = result.Should().BeOfType<ObjectResult>().Which;
                apiResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                apiResult.Value.Should().BeOfType<ErrorResponse>()
                         .Which.Message.Should().Contain(exception.Message);
            }
        }
        
        [Fact]
        public async Task Create_WhenUseCaseThrowsInvalidIngredientProportionException_ReturnsBadRequestWithMessage()
        {
            var exception = new InvalidIngredientProportionException("Exception message");

            _createRecipeUseCaseMock.Setup(u => u.ExecuteAsync(It.IsAny<Recipe>())).ThrowsAsync(exception);

            var result = await CreateSut().Create(_fixture.Create<CreateRecipeApiRequest>());

            using (new AssertionScope())
            {
                var apiResult = result.Should().BeOfType<ObjectResult>().Which;
                apiResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
                apiResult.Value.Should().BeOfType<ErrorResponse>()
                         .Which.Message.Should().Contain(exception.Message);
            }
        }

        private RecipeController CreateSut()
        {
            return new RecipeController(_createRecipeUseCaseMock.Object);
        }
    }
}