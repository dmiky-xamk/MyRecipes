﻿using AutoMapper;
using FluentValidation;
using IdGen;
using MediatR;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Recipes.Commands.CreateRecipe;

public class UpdateRecipe
{
    public class Command : IRequest<Result<Unit>>
    {
        public RecipeDto Recipe { get; set; } = default!;
        public string Id { get; set; } = string.Empty;
    }

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
            RecipeEntity recipe = _mapper.Map<RecipeEntity>(request.Recipe,
                opt =>
                {
                    opt.Items["RecipeId"] = request.Id;
                });

            var affectedRows = await _db.UpdateRecipeAsync(recipe);

            if (affectedRows == 0)
            {
                return Result<Unit>.Failure("Failed to update a recipe with the given ID.");
            }

            await _db.UpdateIngredientsAsync(recipe.Ingredients);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}