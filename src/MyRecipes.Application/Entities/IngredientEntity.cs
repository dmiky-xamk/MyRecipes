using System.Diagnostics.CodeAnalysis;

namespace MyRecipes.Domain.Entities;

public class IngredientEntity
{
    // Default constructor for Dapper.
    public IngredientEntity()
    {}

    // Signal the compiler that the constructor already initializes the 'Name'.
    [SetsRequiredMembers]
    public IngredientEntity(long recipeId, string name, string? unit, int? amount)
    {
        RecipeId = recipeId;
        Name = name; 
        Unit = unit;
        Amount = amount;
    }

    // Id will be created by the db.
    public int Id { get; set; } = default!;
    public long RecipeId { get; set; }
    public required string Name { get; set; }
    public string? Unit { get; set; }

    // TODO: Convert to a string or a double.
    public int? Amount { get; set; }
}