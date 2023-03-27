using MyRecipes.Application.Recipes.Queries;

namespace MyRecipes.Application.Users;

/// <summary>
/// The response from the authentication endpoint when the authentication succeeds.
/// </summary>
/// <param name="Token"></param>
/// <param name="Email"></param>
/// <param name="Recipes"></param>
public record AuthResponse(string Token, string Email, IEnumerable<QueryRecipeDto> Recipes);