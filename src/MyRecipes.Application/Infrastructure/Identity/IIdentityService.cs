using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;

namespace MyRecipes.Application.Infrastructure.Identity;

public interface IIdentityService
{
    Task<IdentificationResult<IdentificationError>> RegisterAsync(RegisterDto registerDto);
    Task<IdentificationResult<IdentificationError>> LoginAsync(LoginDto loginDto);
}