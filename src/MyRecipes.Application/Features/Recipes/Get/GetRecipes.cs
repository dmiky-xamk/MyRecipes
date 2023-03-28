using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Application.Recipes.Queries;

namespace MyRecipes.Application.Features.Recipes.Get;

public class GetRecipes
{
    public class Query : IRequest<IEnumerable<QueryRecipeDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<QueryRecipeDto>>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, ICurrentUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<IEnumerable<QueryRecipeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            IEnumerable<QueryRecipeDto> recipes = (await _db.GetFullRecipesAsync(userId))
                .Select(recipe => recipe.ToQueryRecipeDto());

            // Returning an empty list is fine, the user just hasn't made any recipes yet.
            return recipes;
        }
    }
}