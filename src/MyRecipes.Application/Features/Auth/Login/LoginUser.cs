using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Infrastructure.Identity;

namespace MyRecipes.Application.Features.Auth.Login;

public class LoginUser
{
    public class Query : IRequest<IdentificationResult<string>>
    {
        public LoginDto LoginDto { get; set; } = default!;
    }

    public class Handler : IRequestHandler<Query, IdentificationResult<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public Handler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<IdentificationResult<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(request.LoginDto);

            if (result.IsSuccess)
            {
                string token = _tokenService.GenerateToken(request.LoginDto.Username, result.UserId);

                return IdentificationResult<string>.Success(token);
            }

            return IdentificationResult<string>.Failure(result.IdentityError);
        }
    }
}