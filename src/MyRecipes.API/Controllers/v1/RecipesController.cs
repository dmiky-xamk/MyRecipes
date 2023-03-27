using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.Mapping;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Features.Recipes.Create;
using MyRecipes.Application.Features.Recipes.Delete;
using MyRecipes.Application.Features.Recipes.Get;
using MyRecipes.Application.Features.Recipes.Update;
using MyRecipes.Application.Recipes.Queries;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<QueryRecipeDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var result = await Mediator.Send(new GetRecipes.Query());

        return Ok(result);
    }

    /// <summary>
    /// Gets a Recipe based on the Id.
    /// </summary>
    /// <response code="200">If the Recipe is found</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the Recipe is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryRecipeDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetRecipe(string id)
    {
        var result = await Mediator.Send(new GetRecipe.Query { Id = id });

        return result.Match<IActionResult>(
            recipe => Ok(recipe),
            _ => NotFound());
    }

    /// <summary>
    /// Creates a Recipe.
    /// </summary>
    /// <response code="201">If the Recipe is created</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="422">If the Recipe validation fails</response>
    /// <response code="500">If an unexpected error happened while creating the Recipe</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QueryRecipeDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipeDto recipe)
    {
        var result = await Mediator.Send(new CreateRecipe.Command { Recipe = recipe });

        return result.Match(
            recipe => CreatedAtAction(nameof(GetRecipe), new { recipe.Id }, recipe),
            validationResult => UnprocessableEntityProblem(validationResult.AddToModelState(ModelState)),
            error => ServerProblem(error.Value));
    }

    /// <summary>
    /// Updates a Recipe.
    /// </summary>
    /// <response code="200">If the Recipe is updated</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the Recipe is not found</response>
    /// <response code="422">If the Recipe validation fails</response>
    /// <response code="500">If an unexpected error happened while updating the Recipe</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryRecipeDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateRecipe([FromBody] RecipeDto recipe, string id)
    {
        var result = await Mediator.Send(new UpdateRecipe.Command { Recipe = recipe, Id = id });

        return result.Match(
            recipe => Ok(recipe),
            validationResult => UnprocessableEntityProblem(validationResult.AddToModelState(ModelState)),
            _ => NotFound(),
            error => ServerProblem(error.Value));
    }

    /// <summary>
    /// Deletes a Recipe.
    /// </summary>
    /// <response code="200">If the Recipe is deleted</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="500">If an unexpected error happened while deleting the Recipe</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteRecipe(string id)
    {
        var result = await Mediator.Send(new DeleteRecipe.Command { Id = id });

        return result.Match(
            _ => NoContent(),
            _ => NotFound(),
            error => ServerProblem(error.Value));
    }
}