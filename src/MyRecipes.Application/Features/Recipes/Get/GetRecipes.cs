using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Recipes.Dtos;
using MyRecipes.Application.Infrastructure.Persistence;

namespace MyRecipes.Application.Features.Recipes.Get;

public class GetRecipes
{
    public class Query : IRequest<IEnumerable<QueryRecipeDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<QueryRecipeDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICurrentUserService _userService;

        public Handler(ICurrentUserService userService, IRecipeRepository recipeRepository)
        {
            _userService = userService;
            _recipeRepository = recipeRepository;
        }

        public async Task<IEnumerable<QueryRecipeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            var recipes = (await _recipeRepository.GetRecipesAsync(userId))
                .Select(recipe => recipe.ToQueryRecipeDto());

            // Returning an empty list is fine, the user just hasn't made any recipes yet.
            return recipes;
        }
    }
}