using MyRecipes.Application.Entities;
using MyRecipes.Application.Features.Recipes.Dtos;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Features.Recipes;

// After reading about some interesting viewpoints on AutoMapper, I've decided
// to do the mapping manually. My mappings are quite simple, and I feel like
// doing them manually is more explicit and let's me catch possible errors earlier.
public static class RecipeMappingProfiles
{
    private static IngredientDto ToIngredientDto(this IngredientEntity ingredient)
    {
        return new IngredientDto(ingredient.Name, ingredient.Unit, ingredient.Amount);
    }

    private static IngredientEntity ToIngredientEntity(this IngredientDto ingredient, string recipeId)
    {
        return new IngredientEntity(recipeId, ingredient.Name.Trim(), ingredient.Unit.ToLower().Trim(), ingredient.Amount.Trim());
    }

    private static DirectionEntity ToDirectionEntity(this DirectionDto direction, string recipeId)
    {
        return new DirectionEntity(recipeId, direction.Step.Trim());
    }

    private static DirectionDto ToDirectionDto(this DirectionEntity direction)
    {
        return new DirectionDto(direction.Step);
    }

    public static QueryRecipeDto ToQueryRecipeDto(this RecipeEntity recipe)
    {
        var ingredients = recipe.Ingredients.Select(ing => ing.ToIngredientDto());
        var directions = recipe.Directions.Select(dir => dir.ToDirectionDto());

        return new QueryRecipeDto(recipe.Id.ToString(), recipe.Name, recipe.Description, recipe.Image, ingredients, directions);
    }

    public static RecipeEntity ToRecipeEntity(this RecipeDto recipe, string recipeId, string userId)
    {
        var ingredients = recipe.Ingredients
            .Select(ing => ing.ToIngredientEntity(recipeId))
            .ToList();

        // Filter our empty steps, no need to save them to the database.
        var directions = recipe.Directions
            .Where(dir => !string.IsNullOrWhiteSpace(dir.Step))
            .Select(dir => dir.ToDirectionEntity(recipeId))
            .ToList();

        return new RecipeEntity()
        {
            Id = recipeId,
            UserId = userId,
            Name = recipe.Name.Trim(),
            Ingredients = ingredients,
            Directions = directions,
            Description = recipe.Description.Trim(),
        };
    }
}