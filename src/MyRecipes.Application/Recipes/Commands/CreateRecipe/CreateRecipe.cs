using AutoMapper;
using FluentValidation;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Recipes.Queries.GetRecipes;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Recipes.Commands.CreateRecipe;

public class CreateRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public RecipeVm Recipe { get; set; }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly ICrud _db;
            private readonly IMapper _mapper;

            public Handler(ICrud db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Create recipe
                int recipeId = await _db.CreateRecipeAsync(_mapper.Map<RecipeEntity>(request.Recipe.Recipe));

                if (recipeId == 0)
                {
                    return Result<Unit>.Failure("Failed to create a recipe.");
                }

                // Add the recipe id to every ingredient before saving them to the database.
                List<IngredientEntity> ingredients = _mapper.Map<List<IngredientEntity>>(
                    request.Recipe.Ingredients,
                    opt => opt.AfterMap((_, dest) =>
                    {
                        dest.ForEach(ing => ing.RecipeId = recipeId);
                    }));

                ingredients.ForEach(async ingredient =>
                {
                    await _db.CreateIngredientAsync(ingredient);
                });

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}