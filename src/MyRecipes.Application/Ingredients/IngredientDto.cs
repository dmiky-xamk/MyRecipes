namespace MyRecipes.Application.Ingredients;

public class IngredientDto
{
    public string Name { get; set; } = string.Empty;
    public string? Unit { get; set; }

    // TODO: Convert to a string or a double.
    public int? Amount { get; set; }
}