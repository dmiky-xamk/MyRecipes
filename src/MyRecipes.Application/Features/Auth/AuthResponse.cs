using MyRecipes.Application.Recipes.Queries;

namespace MyRecipes.Application.Users;

public record AuthResponse(string Token, string Email, IEnumerable<QueryRecipeDto> Recipes);