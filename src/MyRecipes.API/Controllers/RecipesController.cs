using Microsoft.AspNetCore.Mvc;
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

    // POST api/<RecipesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<RecipesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<RecipesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
