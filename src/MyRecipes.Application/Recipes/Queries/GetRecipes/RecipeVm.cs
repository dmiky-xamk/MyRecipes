using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class RecipeVm
{
    public RecipeDto Recipe { get; set; } = new();
    public List<IngredientDto> Ingredients { get; set; } = new();
}