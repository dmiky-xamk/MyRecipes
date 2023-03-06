using FluentValidation.TestHelper;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Ingredients;
using MyRecipes.Application.Recipes;

namespace MyRecipes.Application.UnitTests.Recipes.Validation;

public class RecipeDtoValidationTests
{
    private readonly RecipeDtoValidator _validator;

    public RecipeDtoValidationTests()
    {
        _validator = new RecipeDtoValidator();
    }

    [Fact]
    public void RecipeDto_ShouldHaveValidationErrorForEmptyRecipeName()
    {
        List<IngredientDto> ingredients = new()
        {
            new IngredientDto("Flour", "dl", "4")
        };

        var recipe = new RecipeDto(string.Empty, string.Empty, string.Empty, ingredients, Enumerable.Empty<DirectionDto>());

        var result = _validator.TestValidate(recipe);

        result.ShouldHaveValidationErrorFor(recipe => recipe.Name);
    }
    
    [Fact]
    public void RecipeDto_ShouldHaveValidationErrorForEmptyIngredientName()
    {
        List<IngredientDto> ingredients = new()
        {
            new IngredientDto(string.Empty, "dl", "4")
        };

        var recipe = new RecipeDto("Test", string.Empty, string.Empty, ingredients, Enumerable.Empty<DirectionDto>());


        var result = _validator.TestValidate(recipe);

        result.ShouldHaveValidationErrorFor("Ingredients[0].Name");
    }
}