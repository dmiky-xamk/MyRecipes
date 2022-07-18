using AutoMapper;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Ingredients;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class GetRecipe
{
    public class Query : IRequest<Result<RecipeVm>?>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<RecipeVm>?>
    {
        private readonly ICrud _db;
        private readonly IMapper _mapper;

        public Handler(ICrud db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<RecipeVm>?> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipe = await _db.GetRecipeAsync(request.Id);
            
            if (recipe is null)
            {
                return null;
            }

            var ingredients = await _db.GetIngredientsAsync(recipe.Id);

            if (!ingredients.Any())
            {
                // TODO: Log? Throw an exception? Should not happen.
                return Result<RecipeVm>.Failure("Failed to find any ingredients associated with the recipe.");
            }

            RecipeVm fullRecipe = new()
            {
                Recipe = _mapper.Map<RecipeDto>(recipe),
                Ingredients = _mapper.Map<List<IngredientDto>>(ingredients)
            };

            return Result<RecipeVm>.Success(fullRecipe);
        }
    }
}