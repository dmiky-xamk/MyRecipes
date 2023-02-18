namespace MyRecipes.Application.Features.Auth;

public interface ICurrentUserService
{
    string? UserId { get; }
}