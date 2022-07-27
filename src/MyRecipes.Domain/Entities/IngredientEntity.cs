namespace MyRecipes.Domain.Entities;

public class IngredientEntity
{
    public int Id { get; set; }
    public string RecipeId { get; set; } = string.Empty;
    public string Name { get; set; } = default!;
    public string? Unit { get; set; }

    // TODO: Convert to a string or a double.
    public int? Amount { get; set; }
}