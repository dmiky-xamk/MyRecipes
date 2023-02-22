using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Common.Models;
using System.Net;

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

        // Return a better formatted error (https://blog.frankel.ch/structured-errors-http-apis/)
        // https://stackoverflow.com/questions/63301306/add-detail-message-to-asp-net-core-3-1-standard-json-badrequest-response
        return Problem(title: result.Error, statusCode: (int)HttpStatusCode.BadRequest);
    }
}