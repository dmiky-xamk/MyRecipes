using MediatR;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;

namespace MyRecipes.Application.Features.Recipes.Delete;

public class DeleteRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, ICurrentUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            int affectedRows = await _db.DeleteRecipeAsync(request.Id, userId);

            if (affectedRows == 0)
            {
                return Result<Unit>.Failure("Failed to delete the recipe.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}