namespace MyRecipes.Application.Features.Auth;

public interface ITokenService
{
    public string GenerateToken(string username, string userId);
}