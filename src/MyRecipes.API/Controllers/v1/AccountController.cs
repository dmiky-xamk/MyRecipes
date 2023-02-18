using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;

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
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        var result = await Mediator.Send(new LoginUser.Query{ LoginDto = loginDto });

        return HandleIdentityResult(result);
    }

    /// <summary>
    /// Registers a new User.
    /// </summary>
    /// <returns>A JWT token.</returns>
    /// <response code="200">The registration was succesful</response>
    /// <response code="400">The credentials are already in use</response>   
    /// <response code="400">The credentials are invalid</response>   
    /// <response code="400">An unknown error happened</response>   
    /// <response code="401">The user is not authorized</response>  
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterDto registerDto)
    {
        var result = await Mediator.Send(new RegisterUser.Command { RegisterDto = registerDto });

        return HandleIdentityResult(result);
    }
}