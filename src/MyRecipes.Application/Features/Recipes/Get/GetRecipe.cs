using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Recipes.Dtos;
using MyRecipes.Application.Infrastructure.Persistence;
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
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICurrentUserService _userService;

        public Handler(ICurrentUserService userService, IRecipeRepository recipeRepository)
        {
            _userService = userService;
            _recipeRepository = recipeRepository;
        }

        public async Task<OneOf<QueryRecipeDto, NotFound>> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            var recipe = await _recipeRepository.GetRecipeAsync(request.Id, userId);

            if (recipe is null)
            {
                return new NotFound();
            }

            return recipe.ToQueryRecipeDto();
        }
    }
}