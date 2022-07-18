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

    public async Task<RecipeEntity?> GetRecipeAsync(int id)
    {
        string sql = "SELECT * FROM Recipe WHERE Id = @Id";

        return await _db.QueryDataSingle<RecipeEntity, dynamic>(sql, new { Id = id });
    }

    public async Task CreateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "INSERT INTO Recipe (Name, Description, Image)"
            + " VALUES (@Name, @Description, @Image)";

        await _db.SaveData(sql, new { recipe.Name, recipe.Description, recipe.Image });
        
    }

    public async Task<List<IngredientEntity>> GetIngredientsAsync(int recipeId)
    {
        string sql = "SELECT i.* FROM Ingredient i"
            + " INNER JOIN Recipe r "
            + " ON r.Id = @RecipeId ";

        return await _db.QueryData<IngredientEntity, dynamic>(sql, new { RecipeId = recipeId });
    }
}