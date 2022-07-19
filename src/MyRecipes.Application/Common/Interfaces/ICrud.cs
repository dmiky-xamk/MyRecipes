using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Common.Interfaces;

public interface ICrud
{
    Task<int> CreateRecipeAsync(RecipeEntity recipe);
    Task CreateIngredientAsync(IngredientEntity ingredient);
    Task<List<IngredientEntity>> GetIngredientsAsync(int recipeId);
    Task<List<RecipeEntity>> GetRecipesAsync();
    Task<RecipeEntity?> GetRecipeAsync(int id);
}