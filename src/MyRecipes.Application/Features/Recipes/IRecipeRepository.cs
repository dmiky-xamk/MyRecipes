using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Features.Recipes;

/// <summary>
/// Defines the contract for the recipe repository.
/// </summary>
public interface IRecipeRepository
{
    Task<IEnumerable<RecipeEntity>> GetRecipesAsync(string userId);
    Task<RecipeEntity?> GetRecipeAsync(string recipeId, string userId);
    Task<bool> CreateRecipeAsync(RecipeEntity recipe);
    Task<bool> UpdateRecipeAsync(RecipeEntity recipe);
    Task<bool> DeleteRecipeAsync(string id, string userId);
    Task<bool> CheckIfRecipeExistsAsync(string id, string userId);
}