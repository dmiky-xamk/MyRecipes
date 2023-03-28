using MediatR;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Infrastructure.Persistence;
using OneOf;
using OneOf.Types;

namespace MyRecipes.Application.Features.Recipes.Delete;

public class DeleteRecipe
{
    public class Command : IRequest<OneOf<Success, NotFound, Error<string>>>
    {
        public required string Id { get; init; }
    }

    public class Handler : IRequestHandler<Command, OneOf<Success, NotFound, Error<string>>>
    {
        private readonly ICrud _db;
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, ICurrentUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<OneOf<Success, NotFound, Error<string>>> Handle(Command request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            bool doesRecipeExist = await _db.CheckIfRecipeExists(request.Id, userId);
            if (!doesRecipeExist)
            {
                return new NotFound();
            }

            int affectedRows = await _db.DeleteRecipeAsync(request.Id, userId);

            if (affectedRows == 0)
            {
                // Log
                return new Error<string>("An unexpected error happened while deleting your recipe.");
            }

            return new Success();
        }
    }
}