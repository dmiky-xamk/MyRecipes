using FluentValidation;
using IdGen;
using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Features.Recipes.Update;

public class UpdateRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public required RecipeDto Recipe { get; set; }
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, ICurrentUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            RecipeEntity recipe = request.Recipe.ToRecipeEntity(request.Id, userId);

            var affectedRows = await _db.UpdateRecipeAsync(recipe);

            if (affectedRows == 0)
            {
                return Result<Unit>.Failure("Failed to update the recipe.");
            }

            await _db.UpdateIngredientsAsync(recipe.Ingredients);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}