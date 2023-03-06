using MyRecipes.Application.Entities;

namespace MyRecipes.Domain.Entities;

public class RecipeEntity
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public List<IngredientEntity> Ingredients { get; set; } = new();
    public List<DirectionEntity> Directions { get; set; } = new();

}