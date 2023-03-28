using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Recipes.Queries;

public record QueryRecipeDto(
    string Id,
    string Name,
    string Description,
    string Image,
    IEnumerable<IngredientDto> Ingredients,
    IEnumerable<DirectionDto> Directions) : RecipeDto(Name, Description, Image, Ingredients, Directions);