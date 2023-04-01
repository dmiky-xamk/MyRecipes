using MediatR;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Application.Users;
using OneOf;
using OneOf.Types;

namespace MyRecipes.Application.Features.Auth.Current;

public class CurrentUser
{
    public class Query : IRequest<OneOf<AuthResponse, Error<string>>>
    {

    }

    public class Handler : IRequestHandler<Query, OneOf<AuthResponse, Error<string>>>
    {
        private readonly ICurrentUserService _userService;
        private readonly IRecipeRepository _recipeRepository;

        public Handler(ICurrentUserService currentUserService, IRecipeRepository recipeRepository)
        {
            _userService = currentUserService;
            _recipeRepository = recipeRepository;
        }

        public async Task<OneOf<AuthResponse, Error<string>>> Handle(Query request, CancellationToken cancellationToken)
        {
            string? email = _userService.Email;
            string? userId = _userService.UserId;
            string? token = _userService.Token?["Bearer ".Length..];

            if (email is null || userId is null || token is null)
            {
                return new Error<string>("Invalid token");
            }

            var recipes = (await _recipeRepository.GetRecipesAsync(userId))
                .Select(recipe => recipe.ToQueryRecipeDto());

            return new AuthResponse(token, email, recipes);
        }
    }
}