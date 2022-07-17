using AutoMapper;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class GetRecipes
{
    public class Query : IRequest<Result<List<RecipeDto>>>
    {

    }

    public class Handler : IRequestHandler<Query, Result<List<RecipeDto>>>
    {
        private readonly ICrud _db;
        private readonly IMapper _mapper;

        public Handler(ICrud db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<List<RecipeDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipes = (await _db.GetRecipesAsync())
                .Select(recipe => _mapper.Map<RecipeDto>(recipe))
                .ToList();

            return Result<List<RecipeDto>>.Success(recipes);
        }
    }
}