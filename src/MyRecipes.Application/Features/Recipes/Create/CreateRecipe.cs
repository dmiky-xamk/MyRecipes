using IdGen;
using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Features.Recipes.Create;

public class CreateRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public required RecipeDto Recipe { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly ICrud _db;
        private readonly IIdGenerator<long> _idGen;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, IIdGenerator<long> idGen, ICurrentUserService userService)
        {
            _db = db;
            _idGen = idGen;
            _userService = userService;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Create the recipe Snowflake Id
            long recipeId = _idGen.CreateId();
            string userId = _userService.UserId!;

            RecipeEntity recipe = request.Recipe.ToRecipeEntity(recipeId, userId);

            int affectedRows = await _db.CreateRecipeAsync(recipe);

            if (affectedRows == 0)
            {
                return Result<Unit>.Failure("Failed to create the recipe.");
            }

            await _db.CreateIngredientsAsync(recipe.Ingredients);

            // TODO: Return the new recipe so that the client can navigate to it?
            return Result<Unit>.Success(Unit.Value);
        }
    }
}