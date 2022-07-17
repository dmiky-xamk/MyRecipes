using AutoMapper;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class GetRecipe
{
    public class Query : IRequest<Result<RecipeDto>?>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<RecipeDto>?>
    {
        private readonly ICrud _db;
        private readonly IMapper _mapper;

        public Handler(ICrud db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<RecipeDto>?> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipe = await _db.GetRecipeAsync(request.Id);
            
            if (recipe is null)
            {
                //return Result<RecipeDto>.Failure("No recipe was found with the given ID.");
                return null;
            }

            return Result<RecipeDto>.Success(_mapper.Map<RecipeDto>(recipe));
        }
    }
}