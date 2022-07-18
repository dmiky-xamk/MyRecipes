namespace MyRecipes.Domain.Entities;

public class RecipeEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}