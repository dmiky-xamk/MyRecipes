using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Infrastructure.Persistence;

public class Crud : ICrud
{
    private readonly IDataAccess _db;

    public Crud(IDataAccess db)
    {
       _db = db;
    }

    public async Task<List<RecipeEntity>> GetRecipesAsync()
    {
        string sql = "SELECT * FROM Recipe";

        return await _db.QueryData<RecipeEntity, dynamic>(sql, new { });
    }

    public async Task CreateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "INSERT INTO Recipe (Name, Description, Image)"
            + "VALUES (@Name, @Description, @Image)";

        await _db.SaveData(sql, new { recipe.Name, recipe.Description, recipe.Image });

        sql = "INSERT INTO Ingredient (Name, Unit, Amount)"
            + "VALUES (@Name, @Unit, @Amount)";

        await _db.SaveData(sql, recipe.Ingredients);
    }
}