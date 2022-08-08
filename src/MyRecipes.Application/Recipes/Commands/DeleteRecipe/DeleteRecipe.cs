using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.Application.Recipes.Commands.DeleteRecipe;

public class DeleteRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; } = string.Empty;
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
                return Result<Unit>.Failure("Failed to delete a recipe with the given ID.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}