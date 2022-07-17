using MyRecipes.Application.Common.Mappings;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Recipes.Queries.GetRecipes;

public class RecipeDto : IMapFrom<RecipeEntity>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}