using FluentValidation;
using FluentValidation.Results;
using IdGen;
using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Application.Recipes.Queries;
using MyRecipes.Domain.Entities;
using OneOf;
using OneOf.Types;

namespace MyRecipes.Application.Features.Recipes.Update;

public class UpdateRecipe
{
    public class Command : IRequest<OneOf<QueryRecipeDto, ValidationResult, NotFound, Error<string>>>
    {
        public required RecipeDto Recipe { get; init; }
        public required string Id { get; init; }
    }

    public class Handler : IRequestHandler<Command, OneOf<QueryRecipeDto, ValidationResult, NotFound, Error<string>>>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;
        private readonly IValidator<RecipeDto> _validator;

        public Handler(ICrud db, ICurrentUserService userService, IValidator<RecipeDto> validator)
        {
            _db = db;
            _userService = userService;
            _validator = validator;
        }

        public async Task<OneOf<QueryRecipeDto, ValidationResult, NotFound, Error<string>>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.Recipe, cancellationToken);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            string userId = _userService.UserId!;

            bool doesRecipeExist = await _db.CheckIfRecipeExists(request.Id, userId);
            if (!doesRecipeExist)
            {
                return new NotFound();
            }

            RecipeEntity recipe = request.Recipe.ToRecipeEntity(request.Id, userId);

            int affectedRows = await _db.UpdateRecipeAsync(recipe);

            if (affectedRows == 0)
            {
                // Log
                return new Error<string>("An unexpected error happened while updating your recipe.");
            }

            await _db.UpdateIngredientsAsync(recipe.Ingredients, recipe.Id);
            await _db.UpdateDirectionsAsync(recipe.Directions, recipe.Id);

            return recipe.ToQueryRecipeDto();
        }
    }
}