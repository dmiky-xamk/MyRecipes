using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.API.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    /// <summary>
    /// Map the <paramref name="result"/> to an appropriate HTTP response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns>A HTTP response of type <typeparamref name="IActionResult"/>.</returns>
    protected IActionResult HandleResult<T>(Result<T>? result)
    {
        if (result is null)
        {
            return NotFound();
        }

        if (result.IsSuccess && result.Value is null)
        {
            return NotFound();
        }

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}