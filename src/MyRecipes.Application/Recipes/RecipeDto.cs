using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Recipes;

public class RecipeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public IEnumerable<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
}