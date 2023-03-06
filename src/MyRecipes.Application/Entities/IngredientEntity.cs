using System.Diagnostics.CodeAnalysis;

namespace MyRecipes.Domain.Entities;

public class IngredientEntity
{
    // Default constructor for Dapper.
    public IngredientEntity()
    {}

    // Signal the compiler that the constructor already initializes the 'Name'.
    [SetsRequiredMembers]
    public IngredientEntity(string recipeId, string name, string unit, string amount)
    {
        RecipeId = recipeId;
        Name = name; 
        Unit = unit;
        Amount = amount;
    }

    // Id will be created by the db.
    public int Id { get; set; } = default!;
    public required string RecipeId { get; set; }
    public required string Name { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
}