namespace MyRecipes.Domain.Entities;

public class RecipeEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public List<IngredientEntity> Ingredients { get; set; }

    public RecipeEntity()
    { }

    public RecipeEntity(int id, string name, string? description, string? image, List<IngredientEntity> ingredients)
    {
        Id = id;
        Name = name;
        Description = description;
        Image = image;
        Ingredients = ingredients;
    }
}