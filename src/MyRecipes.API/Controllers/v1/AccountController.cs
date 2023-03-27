using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.Mapping;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Auth.Current;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;
using MyRecipes.Application.Users;

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
    /// <response code="200">The login was succesful</response>
    /// <response code="401">The credentials are invalid</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await Mediator.Send(new LoginUser.Query { LoginDto = loginDto });

        return result.Match(
            user => Ok(user),
            authError => UnauthorizedProblem(authError.ErrorMessage));
    }

    /// <summary>
    /// Registers a new User.
    /// </summary>
    /// <response code="200">The registration was succesful</response>
    /// <response code="400">The validation of the credentials failed</response>
    /// <response code="400">An unknown error happened</response>
    /// <response code="409">The credentials are already in use</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await Mediator.Send(new RegisterUser.Command { RegisterDto = registerDto });

        return result.Match(
            user => Ok(user),
            validationError => UnprocessableEntityProblem(validationError.AddToModelState(ModelState)),
            authError =>
            {
                return authError.ErrorType switch
                {
                    AuthError.InvalidCredentials => UnauthorizedProblem(authError.ErrorMessage),
                    AuthError.EmailAlreadyTaken => ConflictProblem(authError.ErrorMessage),
                    _ => ServerProblem(authError.ErrorMessage),
                };
            });
    }

    /// <summary>
    /// Gets the current user's recipes.
    /// </summary>
    /// <response code="200">The registration was succesful</response>
    /// <response code="401">The credentials are invalid</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await Mediator.Send(new CurrentUser.Query { });

        return result.Match<IActionResult>(
            user => Ok(user),
            error => Unauthorized());
    }
}