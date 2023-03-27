using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyRecipes.API.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    // Return a better formatted error (https://blog.frankel.ch/structured-errors-http-apis/)
    // https://stackoverflow.com/questions/63301306/add-detail-message-to-asp-net-core-3-1-standard-json-badrequest-response

    /// <summary>
    /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response with a <see cref="StatusCodes.Status401Unauthorized"/> status code.
    /// </summary>
    /// <param name="title">Message for the <see cref="ProblemDetails"/> title property.</param>
    /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
    protected IActionResult UnauthorizedProblem(string title)
    {
        return Problem(title: title, statusCode: StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response with a <see cref="StatusCodes.Status409Conflict"/> status code.
    /// </summary>
    /// <param name="title">Message for the <see cref="ProblemDetails"/> title property.</param>
    /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
    protected IActionResult ConflictProblem(string title)
    {
        return Problem(title: title, statusCode: StatusCodes.Status409Conflict);
    }

    /// <summary>
    /// Creates an <see cref="ActionResult"/> that produces a <see cref="StatusCodes.Status422UnprocessableEntity"/> response with a <see cref="ValidationProblemDetails"/> value.
    /// </summary>
    /// <param name="modelState">The <see cref="ModelStateDictionary"/>.</param>
    /// <returns>The created <see cref="ActionResult"/> for the response.</returns>
    protected IActionResult UnprocessableEntityProblem(ModelStateDictionary modelState)
    {
        return ValidationProblem(modelStateDictionary: modelState, statusCode: StatusCodes.Status422UnprocessableEntity);
    }

    /// <summary>
    /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response with a <see cref="StatusCodes.Status500InternalServerError"/> status code.
    /// </summary>
    /// <param name="title">Message for the <see cref="ProblemDetails"/> title property.</param>
    /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
    protected IActionResult ServerProblem(string title)
    {
        return Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
    }
}