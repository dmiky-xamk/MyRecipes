using MyRecipes.Application.Common.Interfaces;
using System.Security.Claims;

namespace MyRecipes.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Get the current user's ID to get only the recipes belonging to him.
    public string? UserId 
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
