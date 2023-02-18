using MyRecipes.Application.Ingredients;
using MyRecipes.Application.Recipes.Queries;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Features.Recipes;

// After reading about some interesting viewpoints on AutoMapper, I've decided
// to do the mapping manually. My mappings are quite simple, and I feel like
// doing them manually is more explicit and let's me catch possible errors earlier.
public static class RecipeMappingProfiles
{
    public static IngredientDto ToIngredientDto(this IngredientEntity ingredient)
    {
        return new IngredientDto(ingredient.Name, ingredient.Unit, ingredient.Amount);
    }

    public static IngredientEntity ToIngredientEntity(this IngredientDto ingredient, long recipeId)
    {
        return new IngredientEntity(recipeId, ingredient.Name, ingredient.Unit, ingredient.Amount);
    }

    public static QueryRecipeDto ToQueryRecipeDto(this RecipeEntity recipe)
    {
        var ingredients = recipe.Ingredients.Select(ing => ing.ToIngredientDto());
        return new QueryRecipeDto(recipe.Id, recipe.Name, recipe.Description, recipe.Image, ingredients);
    }

    public static RecipeEntity ToRecipeEntity(this RecipeDto recipe, long recipeId, string userId)
    {
        var ingredients = recipe.Ingredients.Select(ing => ing.ToIngredientEntity(recipeId)).ToList();
        return new RecipeEntity()
        {
            Id = recipeId,
            UserId = userId,
            Name = recipe.Name,
            Ingredients = ingredients,
            Description = recipe.Description,
        };
    }
}