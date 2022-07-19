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

        // Ignore the id (will be created by the db).
        CreateMap<RecipeDto, RecipeEntity>()
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<IngredientEntity, IngredientDto>();

        // Ignore the id (will be created by the db).
        // Ignore the RecipeId (will be created after mapping).
        CreateMap<IngredientDto, IngredientEntity>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.RecipeId, opt => opt.Ignore());
    }
}