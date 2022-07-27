using AutoMapper;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class GetRecipe
{
    public class Query : IRequest<Result<QueryRecipeDto>?>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class Handler : IRequestHandler<Query, Result<QueryRecipeDto>?>
    {
        private readonly ICrud _db;
        private readonly IMapper _mapper;

        public Handler(ICrud db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<QueryRecipeDto>?> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipe = await _db.GetFullRecipeAsync(request.Id);

            if (recipe is null)
            {
                return null;
            }

            return Result<QueryRecipeDto>.Success(_mapper.Map<QueryRecipeDto>(recipe));
        }
    }
}