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
        string sql = "SELECT * FROM Recipe;";

        return await _db.QueryData<RecipeEntity, dynamic>(sql, new { });
    }

    public async Task<RecipeEntity?> GetRecipeAsync(int id)
    {
        string sql = "SELECT * FROM Recipe WHERE Id = @Id;";

        return await _db.QueryDataSingle<RecipeEntity, dynamic>(sql, new { Id = id });
    }

    public async Task<List<IngredientEntity>> GetIngredientsAsync(int recipeId)
    {
        string sql = "SELECT i.*" +
                    " FROM Ingredient i" +
                    " INNER JOIN Recipe r" +
                    " ON r.Id = i.RecipeId" +
                    " WHERE i.RecipeId = @RecipeId";

        return await _db.QueryData<IngredientEntity, dynamic>(sql, new { RecipeId = recipeId });
    }

    /// <summary>
    /// Create a new recipe.
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns>The id of the recipe created. Returns 0 if the creation failed.</returns>
    public async Task<int> CreateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "INSERT INTO Recipe (Name, Description, Image)" +
                    " VALUES (@Name, @Description, @Image);";

        return await _db.SaveData(sql, new { recipe.Name, recipe.Description, recipe.Image });
    }

    public async Task CreateIngredientAsync(IngredientEntity ingredient)
    {
        string sql = "INSERT INTO Ingredient (RecipeId, Name, Unit, Amount)" +
                    " VALUES (@RecipeId, @Name, @Unit, @Amount);";

        await _db.SaveData(sql, new { ingredient.RecipeId, ingredient.Name, ingredient.Unit, ingredient.Amount });
    }

    /// <summary>
    /// Delete a recipe along with its ingredients.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteRecipeAsync(int id)
    {
        // Delete all the ingredients as well.
        string sql = "PRAGMA foreign_keys=ON; DELETE FROM Recipe WHERE Id = @Id";

        return await _db.ExecuteStatement(sql, new { Id = id });
    }
}