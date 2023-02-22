using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Infrastructure.Identity;

namespace MyRecipes.Application.Features.Auth.Register;

public class RegisterUser
{
    public class Command : IRequest<Result<string, AuthError>>
    {
        public required RegisterDto RegisterDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<string, AuthError>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public Handler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<string, AuthError>> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(request.RegisterDto);

            if (result.IsSuccess)
            {
                string token = _tokenService.GenerateToken(request.RegisterDto.Email, result.Value);

                return Result<string, AuthError>.Success(token);
            }

            return result;
        }
    }
}