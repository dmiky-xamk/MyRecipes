using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Recipes.Queries;

// Seperate Dto for queries for passing the Id to the client.
// TODO: Return empty strings instead of nulls for easier time on the client?
public record QueryRecipeDto(
    string Id,
    string Name,
    string Description,
    string Image,
    IEnumerable<IngredientDto> Ingredients,
    IEnumerable<DirectionDto> Directions) : RecipeDto(Name, Description, Image, Ingredients, Directions);