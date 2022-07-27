using AutoMapper;
using MyRecipes.Application.Ingredients;
using MyRecipes.Application.Recipes;
using MyRecipes.Application.Recipes.Queries;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Common.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RecipeEntity, QueryRecipeDto>();

        CreateMap<RecipeDto, RecipeEntity>()
            .ForMember(d => d.Id, opt =>
            opt.MapFrom((_, _, _, context) => context.Items["RecipeId"]))
            .ReverseMap();

        // Ignore the id (will be created by the db).
        CreateMap<IngredientDto, IngredientEntity>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.RecipeId, opt =>
            opt.MapFrom((_, _, _, context) => context.Items["RecipeId"]))
            .ReverseMap();

        //CreateMap<IngredientEntity, IngredientDto>();

        //CreateMap<QueryRecipeDto, IngredientEntity>()
        //    .ForMember(d => d.Id, opt => opt.Ignore())
        //    .ForMember(d => d.RecipeId, opt => opt.Ignore());
    }
}