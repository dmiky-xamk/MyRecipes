using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Auth.Current;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;
using MyRecipes.Application.Features.Recipes.Get;
using System.Net;

namespace MyRecipes.API.Controllers.v1;

// So that the user can register / login.
[AllowAnonymous]

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AccountController : ApiBaseController
{
    /// <summary>
    /// Logs the User in.
    /// </summary>
    /// <returns>A JWT token.</returns>
    /// <response code="200">The login was succesful</response>
    /// <response code="400">An unknown error happened</response>   
    /// <response code="401">The credentials are invalid</response>   
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // Token
        var result = await Mediator.Send(new LoginUser.Query{ LoginDto = loginDto });

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        if (result.ErrorValue == AuthError.InvalidCredentials)
        {
            return Problem(title: result.Error, statusCode: (int)HttpStatusCode.Unauthorized);
        }

        return Problem(title: result.Error, statusCode: (int)HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Registers a new User.
    /// </summary>
    /// <returns>A JWT token.</returns>
    /// <response code="200">The registration was succesful</response>
    /// <response code="400">The validation of the credentials failed</response>   
    /// <response code="400">An unknown error happened</response>   
    /// <response code="409">The credentials are already in use</response>   
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // Token
        var result = await Mediator.Send(new RegisterUser.Command { RegisterDto = registerDto });

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        if (result.ErrorValue == AuthError.EmailAlreadyTaken)
        {
            return Problem(title: result.Error, statusCode: (int)HttpStatusCode.Conflict);
        }

        return Problem(title: result.Error, statusCode: (int)HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Gets the current user's recipes.
    /// </summary>
    /// <returns>User's recipes.</returns>
    /// <response code="200">The registration was succesful</response>
    /// <response code="401">The credentials are invalid</response>   
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        // Get the user's email if the token is valid.
        var result = await Mediator.Send(new CurrentUser.Query { } );

        if (result.IsSuccess)
        {
            // Get the recipes for the user so no need for another request.
            var recipes = await Mediator.Send(new GetRecipes.Query { });

            // Create user with the email and the recipes.
            //UserDto user = new(result.Value, recipes.Value);

            // Return the recipes to the client.
            return Ok(recipes.Value);
        }

        // The token was invalid.
        return Problem(title: result.Error, statusCode: (int)HttpStatusCode.Unauthorized);
    }
}