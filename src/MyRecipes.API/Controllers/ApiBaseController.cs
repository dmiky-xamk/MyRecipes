using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyRecipes.Application.Infrastructure.Identity;

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

    protected ActionResult<string> HandleIdentityResult<T>(IdentificationResult<T> result)
    {
        // Unauthorized (user not found during login, credentials are wrong)
        // Bad Request (unknown error during registration)
        // ValidationProblem (credentials already in use)
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        // TODO: Refactor?
        switch (result.IdentityError)
        {
            case IdentificationError.UsernameTaken:
            case IdentificationError.EmailTaken:
                ModelState.AddModelError(result.ErrorState.Item1, result.ErrorState.Item2);
                return ValidationProblem(ModelState);

            case IdentificationError.WrongCredentials:
            case IdentificationError.UserNotFound:
                return Unauthorized();

            case IdentificationError.UnknownError:
            default:
                return BadRequest(result.Error);
        }
    }
}