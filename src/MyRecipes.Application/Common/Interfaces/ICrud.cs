using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Common.Interfaces;

public interface ICrud
{
    Task CreateRecipeAsync(RecipeEntity recipe);

    Task<List<RecipeEntity>> GetRecipesAsync();
}