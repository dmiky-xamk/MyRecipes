using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.Application.Recipes.Commands.DeleteRecipe;

public class DeleteRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly ICrud _db;

        public Handler(ICrud db)
        {
            _db = db;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            int affectedRows = await _db.DeleteRecipeAsync(request.Id);

            if (affectedRows == 0)
            {
                return Result<Unit>.Failure("Failed to delete a recipe with the given ID");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}