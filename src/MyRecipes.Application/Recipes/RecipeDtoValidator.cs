using FluentValidation;

namespace MyRecipes.Application.Recipes;

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

        RuleForEach(rec => rec.Ingredients)
            .ChildRules(ingredients =>
            {
                ingredients.RuleFor(ing => ing.Name)
                .NotEmpty()
                .WithMessage("The ingredient name is required.");
            });
    }
}