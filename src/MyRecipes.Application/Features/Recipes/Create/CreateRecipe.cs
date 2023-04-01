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

namespace MyRecipes.Application.Features.Recipes.Create;

public class CreateRecipe
{
    public class Command : IRequest<OneOf<QueryRecipeDto, ValidationResult, Error<string>>>
    {
        public required RecipeDto Recipe { get; init; }
    }

    public class Handler : IRequestHandler<Command, OneOf<QueryRecipeDto, ValidationResult, Error<string>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIdGenerator<long> _idGen;
        private readonly ICurrentUserService _userService;
        private readonly IValidator<RecipeDto> _validator;

        public Handler(IIdGenerator<long> idGen, ICurrentUserService userService, IValidator<RecipeDto> validator, IRecipeRepository recipeRepository)
        {
            _idGen = idGen;
            _userService = userService;
            _validator = validator;
            _recipeRepository = recipeRepository;
        }

        public async Task<OneOf<QueryRecipeDto, ValidationResult, Error<string>>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.Recipe, cancellationToken);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            // Create the recipe Snowflake Id
            string recipeId = _idGen.CreateId().ToString();
            string userId = _userService.UserId!;

            RecipeEntity recipe = request.Recipe.ToRecipeEntity(recipeId, userId);

            bool isCreateSuccess = await _recipeRepository.CreateRecipeAsync(recipe);
            if (!isCreateSuccess)
            {
                // Log
                return new Error<string>("An unexpected error happened while creating your recipe.");
            }

            return recipe.ToQueryRecipeDto();
        }
    }
}