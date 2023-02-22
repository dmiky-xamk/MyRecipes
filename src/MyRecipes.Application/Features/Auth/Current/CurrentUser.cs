using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Infrastructure.Identity;
using System.Security.Claims;

namespace MyRecipes.Application.Features.Auth.Current;

public class CurrentUser
{
    public class Query : IRequest<Result<string>>
    {

    }

    public class Handler : IRequestHandler<Query, Result<string>>
    {
        private readonly ICurrentUserService _userService;

        public Handler(ICurrentUserService currentUserService)
        {
            _userService = currentUserService;
        }

        public async Task<Result<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            string? email = _userService.Email;

            if (email is null)
            {
                return Result<string>.Failure("Invalid token.");
            }

            return Result<string>.Success(email);
        }
    }
}