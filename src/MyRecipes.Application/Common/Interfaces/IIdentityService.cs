using MyRecipes.Application.Common.Enums;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Users;

namespace MyRecipes.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<IdentificationResult<IdentificationError>> RegisterAsync(RegisterDto registerDto);
    Task<IdentificationResult<IdentificationError>> LoginAsync(LoginDto loginDto);
}