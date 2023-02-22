using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;
using System.Security.Claims;

namespace MyRecipes.Application.Infrastructure.Identity;

public interface IIdentityService
{
    Task<Result<string, AuthError>> RegisterAsync(RegisterDto registerDto);
    Task<Result<string, AuthError>> LoginAsync(LoginDto loginDto);
    //Task<Result<string>> GetCurrentUserAsync(ClaimsPrincipal user);
}