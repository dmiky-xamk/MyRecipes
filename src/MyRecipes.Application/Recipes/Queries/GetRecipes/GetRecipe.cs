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
        private readonly ICurrentUserService _userService;

        public Handler(ICrud db, IMapper mapper, ICurrentUserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<QueryRecipeDto>?> Handle(Query request, CancellationToken cancellationToken)
        {
            string userId = _userService.UserId!;

            var recipe = await _db.GetFullRecipeAsync(request.Id, userId);

            if (recipe is null)
            {
                return null;
            }

            return Result<QueryRecipeDto>.Success(_mapper.Map<QueryRecipeDto>(recipe));
        }
    }
}