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
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICurrentUserService _userService;

        public Handler(ICurrentUserService userService, IRecipeRepository recipeRepository)
        {
            _userService = userService;
            _recipeRepository = recipeRepository;
        }

        public async Task<OneOf<Success, NotFound, Error<string>>> Handle(Command request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            bool doesRecipeExist = await _recipeRepository.CheckIfRecipeExistsAsync(request.Id, userId);
            if (!doesRecipeExist)
            {
                return new NotFound();
            }

            bool isDeleteSuccess = await _recipeRepository.DeleteRecipeAsync(request.Id, userId);
            if (!isDeleteSuccess)
            {
                // Log
                return new Error<string>("An unexpected error happened while deleting your recipe.");
            }

            return new Success();
        }
    }
}