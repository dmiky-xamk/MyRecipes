using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Application.Recipes.Queries;
using OneOf;
using OneOf.Types;

namespace MyRecipes.Application.Features.Recipes.Get;

public class GetRecipe
{
    public class Query : IRequest<OneOf<QueryRecipeDto, NotFound>>
    {
        public required string Id { get; init; }
    }

    public class Handler : IRequestHandler<Query, OneOf<QueryRecipeDto, NotFound>>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, ICurrentUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<OneOf<QueryRecipeDto, NotFound>> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            var recipe = await _db.GetFullRecipeAsync(request.Id, userId);

            if (recipe is null)
            {
                return new NotFound();
            }

            return recipe.ToQueryRecipeDto();
        }
    }
}