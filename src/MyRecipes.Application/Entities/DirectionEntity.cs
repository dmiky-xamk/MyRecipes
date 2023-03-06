using System.Diagnostics.CodeAnalysis;

namespace MyRecipes.Application.Entities;

public class DirectionEntity
{
    // Default constructor for Dapper.
    public DirectionEntity()
    { }

    // Signal the compiler that the constructor already initializes the 'Name'.
    [SetsRequiredMembers]
    public DirectionEntity(string recipeId, string step)
    {
        RecipeId = recipeId;
        Step = step;
    }

    // Id will be created by the db.
    public int Id { get; set; } = default!;
    public required string RecipeId { get; set; }
    public string Step { get; set; } = string.Empty;
}
