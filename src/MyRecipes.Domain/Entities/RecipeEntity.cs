namespace MyRecipes.Domain.Entities;

public class RecipeEntity
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public IList<IngredientEntity> Ingredients { get; set; } = new List<IngredientEntity>();

}