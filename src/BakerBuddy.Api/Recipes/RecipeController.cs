using BakerBuddy.Api;
using BakerBuddy.Api.Recipes;
using BakerBuddy.Exceptions;
using BakerBuddy.Recipes;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class RecipeController : ControllerBase
{
    private readonly ICreateRecipeUseCase _createRecipeUseCase;

    public RecipeController(ICreateRecipeUseCase createRecipeUseCase)
    {
        _createRecipeUseCase = createRecipeUseCase;
    }

    [HttpPost]
    [Route("api/recipes/create")]
    public async Task<IActionResult> Create(CreateRecipeApiRequest request)
    {
        try
        {
            var recipeId = await _createRecipeUseCase.ExecuteAsync(request.ToRecipe());

            var result = new CreateRecipeResponse(recipeId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                InvalidIngredientProportionException _ => StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse(ex.Message)),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message))
            };
        }
    }
}