using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Features.Recipes.Create;
using MyRecipes.Application.Features.Recipes.Delete;
using MyRecipes.Application.Features.Recipes.Get;
using MyRecipes.Application.Features.Recipes.Update;

namespace MyRecipes.API.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class RecipesController : ApiBaseController
{
    /// <summary>
    /// Gets the list of all Recipes.
    /// </summary>
    /// <returns>The list of Recipes.</returns>
    /// <response code="200">Returns the user's recipes</response>
    /// <response code="401">If the user is not authorized</response>   
    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var result = await Mediator.Send(new GetRecipes.Query());

        return HandleResult(result);
    }

    /// <summary>
    /// Gets a Recipe based on the Id.
    /// </summary>
    /// <returns>Recipe that matches the Id.</returns>
    /// <response code="200">If the Recipe is found</response>
    /// <response code="401">If the user is not authorized</response>   
    /// <response code="404">If the Recipe is not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(string id)
    {
        var result = await Mediator.Send(new GetRecipe.Query { Id = id });

        return HandleResult(result);
    }

    /// <summary>
    /// Creates a Recipe.
    /// </summary>
    /// <returns>Empty object.</returns>
    /// <response code="200">If the Recipe is created</response>
    /// <response code="400">If the creation failed</response>
    /// <response code="401">If the user is not authorized</response> 
    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipeDto recipe)
    {
        var result = await Mediator.Send(new CreateRecipe.Command { Recipe = recipe });

        return HandleResult(result);
    }

    /// <summary>
    /// Updates a Recipe.
    /// </summary>
    /// <returns>Empty object.</returns>
    /// <response code="200">If the Recipe is updated</response>
    /// <response code="400">If the update failed</response>
    /// <response code="401">If the user is not authorized</response> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe([FromBody] RecipeDto recipe, long id)
    {
        var result = await Mediator.Send(new UpdateRecipe.Command { Recipe = recipe, Id = id });

        return HandleResult(result);
    }

    /// <summary>
    /// Deletes a Recipe.
    /// </summary>
    /// <returns>Empty object.</returns>
    /// <response code="200">If the Recipe is deleted</response>
    /// <response code="400">If the deletion failed</response>
    /// <response code="401">If the user is not authorized</response> 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(string id)
    {
        var result = await Mediator.Send(new DeleteRecipe.Command { Id = id });

        return HandleResult(result);
    }
}
