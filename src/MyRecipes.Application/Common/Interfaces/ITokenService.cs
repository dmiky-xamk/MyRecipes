using MyRecipes.Application.Users;

namespace MyRecipes.Application.Common.Interfaces;

public interface ITokenService
{
    public string GenerateToken(string username);
}