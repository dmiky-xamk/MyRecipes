using MyRecipes.Application.Entities;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Infrastructure.Persistence;

public interface ICrud
{
    /// <summary>
    /// Create a new recipe.
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns>The number of rows affected.</returns>
    Task CreateIngredientsAsync(IEnumerable<IngredientEntity> ingredient);
    Task<int> CreateRecipeAsync(RecipeEntity recipe);
    Task CreateDirectionsAsync(IEnumerable<DirectionEntity> directions);

    Task<IEnumerable<RecipeEntity>> GetFullRecipesAsync(string userId);
    Task<RecipeEntity?> GetFullRecipeAsync(string recipeId, string userId);

    Task<int> UpdateRecipeAsync(RecipeEntity recipe);
    Task UpdateIngredientsAsync(IEnumerable<IngredientEntity> ingredients, string recipeId);
    Task UpdateDirectionsAsync(IEnumerable<DirectionEntity> directions, string recipeId);

    Task<bool> CheckIfRecipeExists(string id, string userId);


    /// <summary>
    /// Delete a recipe along with its ingredients.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The number of rows affected.</returns>
    Task<int> DeleteRecipeAsync(string id, string userId);

    // Deprecated?
    //Task<List<IngredientEntity>> GetIngredientsAsync(string recipeId);
    //Task<List<RecipeEntity>> GetRecipesAsync();
    //Task<RecipeEntity?> GetRecipeAsync(string id);
}