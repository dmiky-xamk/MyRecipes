namespace MyRecipes.Domain.Entities;

public class IngredientEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Unit { get; set; }
    public int? Amount { get; set; }

    public IngredientEntity(int id, string name, string? unit, int? amount)
    {
        Id = id;
        Name = name;
        Unit = unit;
        Amount = amount;
    }
}