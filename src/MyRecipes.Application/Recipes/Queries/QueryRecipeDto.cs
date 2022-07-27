using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Recipes.Queries;

public class QueryRecipeDto : RecipeDto
{
    public string Id { get; set; } = string.Empty;
}