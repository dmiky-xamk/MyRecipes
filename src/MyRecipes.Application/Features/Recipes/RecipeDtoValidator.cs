using FluentValidation;
using MyRecipes.Application.Features.Recipes;

namespace MyRecipes.Application.Recipes;

// Validates the user input when creating a new recipe.
public class RecipeDtoValidator :
    AbstractValidator<RecipeDto>
{
    public RecipeDtoValidator()
    {
        RuleFor(rec => rec.Name)
            .NotEmpty()
            .WithMessage("The recipe name is required.")
            .MaximumLength(40)
            .WithMessage("The recipe name must be under 40 characters long.");

        RuleFor(rec => rec.Ingredients)
            .NotEmpty()
            .WithMessage("At least one ingredient is required.");

        RuleForEach(rec => rec.Ingredients)
            .ChildRules(ingredients =>
            {
                ingredients.RuleFor(ing => ing.Name)
                .NotEmpty()
                .WithMessage("The ingredient name is required.");
            });
    }
}