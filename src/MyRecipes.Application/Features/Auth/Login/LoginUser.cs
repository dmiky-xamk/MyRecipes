using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Infrastructure.Identity;

namespace MyRecipes.Application.Features.Auth.Login;

public class LoginUser
{
    public class Query : IRequest<Result<string, AuthError>>
    {
        public required LoginDto LoginDto { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<string, AuthError>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public Handler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<string, AuthError>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(request.LoginDto);

            if (result.IsSuccess)
            {
                string token = _tokenService.GenerateToken(request.LoginDto.Email, result.Value);

                return Result<string, AuthError>.Success(token);
            }

            return result;
        }
    }
}