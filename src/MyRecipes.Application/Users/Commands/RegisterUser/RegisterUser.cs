using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Common.Enums;

namespace MyRecipes.Application.Users.Commands.RegisterUser;

public class RegisterUser
{
    public class Command : IRequest<IdentificationResult<string>>
    {
        public RegisterDto RegisterDto { get; set; } = default!;
    }

    public class Handler : IRequestHandler<Command, IdentificationResult<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public Handler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<IdentificationResult<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(request.RegisterDto);

            if (result.IsSuccess)
            {
                string token = _tokenService.GenerateToken(request.RegisterDto.Username, result.UserId);

                return IdentificationResult<string>.Success(token);
            }

            if (result.IdentityError == IdentificationError.UnknownError)
            {
                return IdentificationResult<string>.Failure(result.IdentityError, result.Error);
            }

            return IdentificationResult<string>.Failure(result.IdentityError, result.ErrorState);
        }
    }
}