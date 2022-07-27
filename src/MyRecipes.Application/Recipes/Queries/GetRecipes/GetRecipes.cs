using AutoMapper;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class GetRecipes
{
    public class Query : IRequest<Result<IEnumerable<QueryRecipeDto>>>
    {

    }

    public class Handler : IRequestHandler<Query, Result<IEnumerable<QueryRecipeDto>>>
    {
        private readonly ICrud _db;
        private readonly IMapper _mapper;

        public Handler(ICrud db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<QueryRecipeDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipes = (await _db.GetFullRecipesAsync())
                .Select(recipe => _mapper.Map<QueryRecipeDto>(recipe));

            // Returning an empty list is fine, the user just hasn't made any recipes yet.
            return Result<IEnumerable<QueryRecipeDto>>.Success(recipes);
        }
    }
}