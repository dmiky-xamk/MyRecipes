using AutoMapper;
using MyRecipes.Application.Ingredients;
using MyRecipes.Application.Recipes.Queries.GetRecipes;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Common.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RecipeEntity, RecipeDto>();
        CreateMap<IngredientEntity, IngredientDto>();
    }
}