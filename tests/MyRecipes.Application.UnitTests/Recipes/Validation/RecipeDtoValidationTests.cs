using FluentValidation.TestHelper;
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
            new IngredientDto() { Name = "Flour", Unit = "dl", Amount = 4 }
        };

        var recipe = new RecipeDto
        {
            Name = string.Empty,
            Ingredients = ingredients
        };

        var result = _validator.TestValidate(recipe);

        result.ShouldHaveValidationErrorFor(recipe => recipe.Name);
    }
    
    [Fact]
    public void RecipeDto_ShouldHaveValidationErrorForEmptyIngredientName()
    {
        List<IngredientDto> ingredients = new()
        {
            new IngredientDto() { Name = string.Empty, Unit = "dl", Amount = 4 }
        };

        var recipe = new RecipeDto
        {
            Name = "Pancakes",
            Ingredients = ingredients
        };

        var result = _validator.TestValidate(recipe);

        result.ShouldHaveValidationErrorFor("Ingredients[0].Name");
    }
}