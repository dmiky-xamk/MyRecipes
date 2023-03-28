using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;
using OneOf;

namespace MyRecipes.Application.Infrastructure.Identity;

public interface IIdentityService
{
    Task<OneOf<string, AuthenticationError>> RegisterAsync(RegisterDto registerDto);
    Task<OneOf<string, AuthenticationError>> LoginAsync(LoginDto loginDto);
}