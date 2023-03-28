using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MyRecipes.Application.Infrastructure.Identity;
using MyRecipes.Application.Recipes.Queries;
using MyRecipes.Application.Users;
using OneOf;

namespace MyRecipes.Application.Features.Auth.Register;

public class RegisterUser
{
    public class Command : IRequest<OneOf<AuthResponse, ValidationResult, AuthenticationError>>
    {
        public required RegisterDto RegisterDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, OneOf<AuthResponse, ValidationResult, AuthenticationError>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterDto> _validator;

        public Handler(IIdentityService identityService, ITokenService tokenService, IValidator<RegisterDto> validator)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<OneOf<AuthResponse, ValidationResult, AuthenticationError>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.RegisterDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var result = await _identityService.RegisterAsync(request.RegisterDto);

            return result.Match<OneOf<AuthResponse, ValidationResult, AuthenticationError>>(
                userId =>
                {
                    var token = _tokenService.GenerateToken(request.RegisterDto.Email, userId);

                    return new AuthResponse(token, request.RegisterDto.Email, Enumerable.Empty<QueryRecipeDto>());
                },
                authError => authError);
        }
    }
}