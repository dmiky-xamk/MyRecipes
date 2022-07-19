using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Recipes.Commands.CreateRecipe;
using MyRecipes.Application.Recipes.Commands.DeleteRecipe;
using MyRecipes.Application.Recipes.Queries.GetRecipes;

namespace MyRecipes.API.Controllers;


public class RecipesController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var result = await Mediator.Send(new GetRecipes.Query());

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var result = await Mediator.Send(new GetRecipe.Query { Id = id });

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipeVm recipe)
    {
        var result = await Mediator.Send(new CreateRecipe.Command { Recipe = recipe });

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var result = await Mediator.Send(new DeleteRecipe.Command { Id = id });

        return HandleResult(result);
    }
}
