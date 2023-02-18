namespace MyRecipes.Domain.Entities;

public class RecipeEntity
{
    public required long Id { get; set; }
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public List<IngredientEntity> Ingredients { get; set; } = new();

}