using AutoMapper;
using IdGen;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Recipes.Commands.CreateRecipe;

public class CreateRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public RecipeDto Recipe { get; set; } = default!;
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly ICrud _db;
        private readonly IMapper _mapper;
        private readonly IIdGenerator<long> _idGen;

        public Handler(ICrud db, IMapper mapper, IIdGenerator<long> idGen)
        {
            _db = db;
            _mapper = mapper;
            _idGen = idGen;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Create the recipe Snowflake Id
            long recipeId = _idGen.CreateId();

            RecipeEntity recipe = _mapper.Map<RecipeEntity>(request.Recipe,
                opt =>
                {
                    opt.Items["RecipeId"] = recipeId;
                });

            int affectedRows = await _db.CreateRecipeAsync(recipe);

            if (affectedRows == 0)
            {
                return Result<Unit>.Failure("Failed to create the recipe.");
            }

            await _db.CreateIngredientsAsync(recipe.Ingredients);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}