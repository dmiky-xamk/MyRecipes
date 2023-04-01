namespace MyRecipes.Application.Features.Recipes.Dtos;

public record QueryRecipeDto(
    string Id,
    string Name,
    string Description,
    string Image,
    IEnumerable<IngredientDto> Ingredients,
    IEnumerable<DirectionDto> Directions) : RecipeDto(Name, Description, Image, Ingredients, Directions);