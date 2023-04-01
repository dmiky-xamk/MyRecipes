using FluentValidation;
using FluentValidation.Results;
using IdGen;
using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Recipes.Dtos;
using MyRecipes.Application.Infrastructure.Persistence;
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
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICurrentUserService _userService;
        private readonly IValidator<RecipeDto> _validator;

        public Handler(ICurrentUserService userService, IValidator<RecipeDto> validator, IRecipeRepository recipeRepository)
        {
            _userService = userService;
            _validator = validator;
            _recipeRepository = recipeRepository;
        }

        public async Task<OneOf<QueryRecipeDto, ValidationResult, NotFound, Error<string>>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.Recipe, cancellationToken);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            string userId = _userService.UserId!;

            bool doesRecipeExist = await _recipeRepository.CheckIfRecipeExistsAsync(request.Id, userId);
            if (!doesRecipeExist)
            {
                return new NotFound();
            }

            RecipeEntity recipe = request.Recipe.ToRecipeEntity(request.Id, userId);

            bool isUpdateSuccess = await _recipeRepository.UpdateRecipeAsync(recipe);
            if (!isUpdateSuccess)
            {
                // Log
                return new Error<string>("An unexpected error happened while updating your recipe.");
            }

            return recipe.ToQueryRecipeDto();
        }
    }
}