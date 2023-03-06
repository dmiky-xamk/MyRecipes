using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Features.Recipes;

// Dto for when user creates a recipe.
public record RecipeDto(
    string Name,
    string Description,
    string Image,
    IEnumerable<IngredientDto> Ingredients,
    IEnumerable<DirectionDto> Directions);