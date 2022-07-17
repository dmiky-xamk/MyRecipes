using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result is null)
        {
            return NotFound();
        }

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        if (result.IsSuccess && result.Value is null)
        {
            return NotFound();
        }

        return BadRequest(result.Error);
    }
}