using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Application.Users;
using MyRecipes.Application.Users.Commands.RegisterUser;
using MyRecipes.Application.Users.Queries.LoginUser;

namespace MyRecipes.API.Controllers.v1;

// So that the user can register / login.
[AllowAnonymous]

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AccountController : ApiBaseController
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        var result = await Mediator.Send(new LoginUser.Query{ LoginDto = loginDto });

        return HandleIdentityResult(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterDto registerDto)
    {
        var result = await Mediator.Send(new RegisterUser.Command { RegisterDto = registerDto });

        return HandleIdentityResult(result);
    }
}