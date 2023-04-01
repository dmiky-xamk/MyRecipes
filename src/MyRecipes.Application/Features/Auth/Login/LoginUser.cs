using MediatR;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Infrastructure.Identity;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Application.Users;
using OneOf;

namespace MyRecipes.Application.Features.Auth.Login;

public class LoginUser
{
    public class Query : IRequest<OneOf<AuthResponse, AuthenticationError>>
    {
        public required LoginDto LoginDto { get; set; }
    }

    public class Handler : IRequestHandler<Query, OneOf<AuthResponse, AuthenticationError>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly IRecipeRepository _recipeRepository;

        public Handler(IIdentityService identityService, ITokenService tokenService, IRecipeRepository recipeRepository)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _recipeRepository = recipeRepository;
        }

        public async Task<OneOf<AuthResponse, AuthenticationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(request.LoginDto);

            return await result.Match<Task<OneOf<AuthResponse, AuthenticationError>>>(
                async userId => {
                    var token = _tokenService.GenerateToken(request.LoginDto.Email, userId);

                    var recipes = (await _recipeRepository.GetRecipesAsync(userId))
                        .Select(recipe => recipe.ToQueryRecipeDto());

                    return new AuthResponse(token, request.LoginDto.Email, recipes);
                    },
                authError => Task.FromResult<OneOf<AuthResponse, AuthenticationError>>(authError));
        }
    }
}
