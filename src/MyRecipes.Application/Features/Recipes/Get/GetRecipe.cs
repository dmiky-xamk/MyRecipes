using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Application.Recipes.Queries;

namespace MyRecipes.Application.Features.Recipes.Get;

public class GetRecipe
{
    public class Query : IRequest<Result<QueryRecipeDto>?>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<QueryRecipeDto>?>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, ICurrentUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<Result<QueryRecipeDto>?> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            var recipe = await _db.GetFullRecipeAsync(request.Id, userId);

            if (recipe is null)
            {
                return null;
            }

            return Result<QueryRecipeDto>.Success(recipe.ToQueryRecipeDto());
        }
    }
}