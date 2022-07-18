namespace MyRecipes.Domain.Entities;

public class IngredientEntity
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public string? Unit { get; set; }
    public int? Amount { get; set; }
}