using Microsoft.AspNetCore.Mvc;

[ApiController]
public class RecipeController: ControllerBase
{
    [HttpPost]
    [Route("api/recipes/create")]
    public IActionResult Create()
    {
        return Ok();
    }
}
