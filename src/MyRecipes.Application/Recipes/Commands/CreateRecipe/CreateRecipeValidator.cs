using FluentValidation;
using MyRecipes.Application.Recipes.Queries.GetRecipes;

namespace MyRecipes.Application.Recipes.Commands.CreateRecipe;

public class CreateRecipeValidator :
    AbstractValidator<RecipeVm>
{
    public CreateRecipeValidator()
    {
        RuleFor(rec => rec.Recipe.Name)
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